using API.MomsRecipes;
using Newtonsoft.Json;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Data.Organization;
using Resto.Front.Api.Data.Payments;
using Resto.Front.Api.Data.Security;
using Resto.Front.Api.Data.View;
using Resto.Front.Api.Exceptions;
using Resto.Front.Api.Extensions;
using Resto.Front.Api.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using JetBrains.Annotations;

namespace Resto.Front.Api.MomsRecipes
{
    internal sealed class ExternalPaymentProcessor : MarshalByRefObject, IFrontPlugin, IDisposable, IPaymentProcessor
    {
#if DEBUG
        private const string paymentSystemKey = "Loymax";
        public const string paymentSystemName = "Loymax Api Payment";
#else
        private const string paymentSystemKey = "MomsRecipes";
        public const string paymentSystemName = "MomsRecipes Api Payment";
#endif

        protected static object SyncRoot = new object();
        public string PaymentSystemKey => paymentSystemKey;
        public string PaymentSystemName => paymentSystemName;

        private APIWorker _MomsRecipesAPI;
        public IOperationService _operationService;

        // Note http://msdn.microsoft.com/en-us/library/23bk23zc(v=vs.100).aspx
        public override object InitializeLifetimeService() { return null; }

        public ExternalPaymentProcessor(APIWorker MomsRecipesAPI, IOperationService operationService)
        {
            _MomsRecipesAPI = MomsRecipesAPI;
            _operationService = operationService;
            MomsRecipesPlugin.subscriptions = new CompositeDisposable
            {
                 PluginContext.Notifications.OrderChanged.Subscribe((v) => OnOrderChanged(v.Entity, _MomsRecipesAPI, operationService))

            };

        }

        public void OnOrderChanged(IOrder order, APIWorker MomsRecipesAPI, IOperationService operationService)
        {
            PluginContext.Log.Info($"OnOrderChanged");
            PluginContext.Log.Warn($"Order changed - {order.Id}, new status '{order.Status}'");
            if (order.Status == OrderStatus.Closed)
            {

                CollectedData data = null;
                data = data = LoadOrder(order.Id);
                //data = CollectedData.deserialize<CollectedData>(context.Getcu());
                try
                {
                    if (data != null)
                    {
                        PluginContext.Log.Info($"data {data.ToString()}");
                        if (data.customer.id > 0 && data.TranssactionId > 0)
                        {                            
                            var sendOrder = Mapper.Map(order);
                            sendOrder.sum = order.ResultSum;

                            sendOrder.customer_id = data.customer.id;
                            sendOrder.phone = data.customer.phone;
                            sendOrder.name = data.customer.name;
                            sendOrder.bonus_used = 0;
                            foreach (var d in order.Payments)
                            {
                                if (d.Type.Name.ToLower().Contains(paymentSystemKey.ToLower()))
                                {
                                    sendOrder.bonus_used = d.Sum;
                                }
                            }
                            if (data.TranssactionId > 0)
                            {
                                /// что-то уже отправляли раньше                    
                                var createOrder = _MomsRecipesAPI.GetUpdateOrder(sendOrder, data.TranssactionId);
                                if (!String.IsNullOrEmpty(createOrder.message))
                                {                                    
                                    //throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                                    //throw new PaymentActionCancelledException();
                                }
                            }
                            else
                            {
                                var createOrder = _MomsRecipesAPI.GetCreateOrder(sendOrder);
                                if (!String.IsNullOrEmpty(createOrder.message))
                                {
                                    //throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                                }
                                else
                                {
                                    if (createOrder != null)
                                    {
                                        data.TranssactionId = createOrder.id;
                                    }
                                }
                            }
                            SaveOrder(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (order.Status == OrderStatus.Deleted)
            {
                CollectedData data = null;
                data = LoadOrder(order.Id);
                try
                {
                    if (data != null && data.TranssactionId > 0)
                    {
                        PluginContext.Log.Info($"data {data.ToString()}");
                        if (data.OrderId != order.Id)
                        {
                            data.TranssactionId = 0;
                            return;
                        }
                        var Order = _MomsRecipesAPI.GetDeleteOrder(data.TranssactionId);
                        if (Order != null)
                        {
                            order = GetOrderSafe(order.Id);
                            SaveOrder(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }


        public void Dispose()
        {
        }
#region IPaymentProcessor
        public void CollectData(Guid orderId, Guid paymentTypeId, [NotNull] IUser cashier, IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context)
        {
            PluginContext.Log.Info("CollectData");
            PluginContext.Log.Info($"===>Получаем данные для заказа ({orderId})");
            var order = GetOrderSafe(orderId);
            CollectedData data = null;
            data = LoadOrder(order.Id);
            try
            {
                if (data != null)
                {
                    PluginContext.Log.Info($"data {data.ToString()}");
                }
            }
            catch { }
            /// если уже была попытка добавить этот тип оплаты в заказ.
            if (data.TranssactionId > 0)
            {
                var sendOrder = Mapper.Map(order);
                sendOrder.sum = order.ResultSum;

                sendOrder.customer_id = data.customer.id;
                sendOrder.phone = data.customer.phone;
                sendOrder.name = data.customer.name;
                sendOrder.bonus_used = 0;
                foreach (var d in order.Payments)
                {
                    if (d.Type.Name.ToLower().Contains(paymentSystemKey.ToLower()))
                    {
                        sendOrder.bonus_used = d.Sum;
                    }
                }
                /// что-то уже отправляли раньше                    
                var createOrder = _MomsRecipesAPI.GetUpdateOrder(sendOrder, data.TranssactionId);
                if (!String.IsNullOrEmpty(createOrder.message))
                {

                    //throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                    viewManager.ShowErrorPopup($"Ошибка ответа от API" + Environment.NewLine + createOrder.message);
                    throw new PaymentActionCancelledException();
                }
            }
            String strRequest = String.Empty;
            ///Ввод номера телефона
            var settings = new ExtendedInputDialogSettings
            {                
                EnableCardSlider = true,
                EnablePhone = true,
                EnableBarcode = true,
                TabTitlePhone = "Введите номер телефона клиента.",
                TabTitleBarcode = "Отсканируйте QR код"

            };
            var phoneRequestResult = viewManager.ShowExtendedInputDialog("Ввод данных для Moms.",
                "Введите номер телефона покупателя/QR или нажмите отмена.", settings);
            Customer client = null;
            PluginContext.Log.Info($"===>phoneRequestResult - {phoneRequestResult} .");
            switch (phoneRequestResult)
            {
                case null: // user cancelled the dialog
                    PluginContext.Log.Info($"===>Покупатель отказался использовать 'Moms' .");
                    //throw new PaymentActionFailedException($"Покупатель отказался использовать 'Moms' .");
                    viewManager.ShowErrorPopup($"===>Покупатель отказался использовать 'Moms' .");
                    throw new PaymentActionCancelledException();
                    break;
                case BarcodeInputDialogResult barcode:
                    PluginContext.Log.Info($"===>Пользователь ввёл barcode - {barcode.Barcode}");
                    strRequest = barcode.Barcode;
                    client = _MomsRecipesAPI.GetCustomerById(strRequest).data;
                    break;
                case CardInputDialogResult card:
                    break;
                case StringInputDialogResult phone:
                    PluginContext.Log.Info($"===>Пользователь ввёл StringInputDialogResult - {phone.Result}");
                    strRequest = phone.Result;
                    client = _MomsRecipesAPI.GetCustomerByPhone(strRequest).data[0];
                    break;
                case PhoneInputDialogResult phone:
                    PluginContext.Log.Info($"===>Пользователь ввёл номер телефона - {phone.PhoneNumber}");
                    strRequest = phone.PhoneNumber;
                    client = _MomsRecipesAPI.GetCustomerByPhone(strRequest).data[0];
                    break;
                default:
                    PluginContext.Log.Info($"===>Пользователь ничего не ввёл default");
                    break;
            };

            if ((client != null) && (!String.IsNullOrEmpty(client?.qr)))
            {
                PluginContext.Log.Info($"===>При запросе клиента вернулся клиент.");
                data.customer = client;
                data.bonus_used = 0;
                if (client.bonus > 0)
                {
                    PluginContext.Log.Info($"===>Есть что списывать");
                    /// сколько можно списать
                    /// Списать можно не больше чем стоимость заказа или сколько есть бонусов
                    int howMuch = (int)(client.bonus > order.ResultSum ? order.ResultSum - order.ProcessedPaymentsSum : client.bonus);
                    decimal _inputValue = 0;
                    do
                    {
                        var inputResult = viewManager.ShowInputDialog($"Введите сумму списания." + Environment.NewLine + $"Доступно к списанию - {client.bonus}", InputDialogTypes.Number, howMuch);
                        if (inputResult == null)
                        {
                            //Cancel button pressed, cancel operation silently.
                            PluginContext.Log.InfoFormat($"Кассир нажал кнопку отмена при вводе суммы списания.");
                            ///throw new PaymentActionFailedException($"Добавление Loymax отменено кассиром.");
                            _inputValue = 0;///throw new PaymentActionFailedException($"Списание бонусов было отменено оператором.");
                        }
                        else
                        {
                            _inputValue = Convert.ToDecimal((inputResult as NumberInputDialogResult).Number);//(inputResult as NumberInputDialogResult).Number;
                        }
                        PluginContext.Log.InfoFormat($"while {_inputValue} > {order.ResultSum})");
                        /// нельзя вводить для списания бонусов больше чем есть на счёте.
                    } while ((_inputValue > howMuch) | ((_inputValue > order.ResultSum)));
                    data.bonus_used = _inputValue;
                }
                else
                {
                    viewManager.ShowOkPopup("Информация о карте.", "У клиента нулевой баланс");
                }
                SaveOrder(data);
            }
            else
            {
                PluginContext.Log.Info($"===>При запросе клиента вернулся пустой");
                //throw new PaymentActionFailedException($"Клиент не найден" + Environment.NewLine + "Обратитесь к системному администратору.");
                viewManager.ShowErrorPopup($"Клиент не найден" + Environment.NewLine + "Обратитесь к системному администратору.");
                throw new PaymentActionCancelledException();
            }
        }

        public void Pay(decimal sum, [NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, [NotNull] IOperationService operationService, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            PluginContext.Log.Info($"Pay");
            CollectedData data = null;
            data = LoadOrder(order.Id);
            if (data == null)
            {
                //throw new PaymentActionFailedException($"Ошибка при получение данных(Pay) при фиксации оплаты." + Environment.NewLine + "Обратитесь к системному администратору.");
                viewManager.ShowErrorPopup($"Ошибка при получение данных(Pay) при фиксации оплаты." + Environment.NewLine + "Обратитесь к системному администратору.");
                throw new PaymentActionCancelledException();
            }
            try
            {
                if (data != null)
                {
                    PluginContext.Log.Info($"data {data.ToString()}");
                }
            }
            catch { }
            if (data.OrderId != order.Id)
            {
                data.TranssactionId = 0;
            }
            PluginContext.Log.Info($"Список оплат в заказ(Pay):");
            foreach (var d in order.Payments)
            {
                PluginContext.Log.Info($"====>order.Payments - {d.Id} - {d.Type.Kind} - {d.Type.Name} - {d.Sum} - {d.Status}");
            }
            PluginContext.Log.Info($"Список скидок в заказ(Pay):");
            foreach (var d in order.Discounts)
            {
                PluginContext.Log.Info($"====>order.Discount - {d.DiscountType.Name} - {d.DiscountType.IsActive}");
            }
            PluginContext.Log.Info($"Список продуктов в заказе(Pay):");
            foreach (var p in order.Items.OfType<IOrderProductItem>().ToList())
            {
                PluginContext.Log.Info($"====>{p.Id} - {p.Product.Name} - {p.Amount}");
                if (p.AssignedModifiers != null)
                {
                    foreach (var t in p.AssignedModifiers)
                    {
                        PluginContext.Log.Info($"AssignedModifiers    {t.Id} - {t.Product.Name} - {t.Amount} - {t.Price}");
                    }
                }
            }
            var sendOrder = Mapper.Map(order);
            sendOrder.sum = order.ResultSum;
            sendOrder.customer_id = data.customer.id;
            sendOrder.phone = data.customer.phone;
            sendOrder.name = data.customer.name;
            sendOrder.bonus_used = 0;
            foreach (var d in order.Payments)
            {
                if (d.Type.Name.ToLower().Contains(paymentSystemKey.ToLower()))
                {
                    sendOrder.bonus_used = d.Sum;
                }
            }

            if (data.TranssactionId > 0)
            {
                /// что-то уже отправляли раньше                    
                var createOrder = _MomsRecipesAPI.GetUpdateOrder(sendOrder, data.TranssactionId);
                if (!String.IsNullOrEmpty(createOrder.message))
                {
                    ///throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                    viewManager.ShowErrorPopup($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                    throw new PaymentActionCancelledException();
                }
            }
            else
            {
                /// впервые видим этот заказ
                var createOrder = _MomsRecipesAPI.GetCreateOrder(sendOrder);
                if (!String.IsNullOrEmpty(createOrder.message))
                {
                    //throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                    viewManager.ShowErrorPopup($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                    throw new PaymentActionCancelledException();
                }
                else
                {
                    if (createOrder != null)
                    {
                        data.TranssactionId = createOrder.id;
                    }
                }
            }
            SaveOrder(data);
        }

        public void EmergencyCancelPayment(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context)
        {
            PluginContext.Log.Info("EmergencyCancelPayment");
        }

        public void ReturnPayment(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            PluginContext.Log.Info("ReturnPayment");
            PluginContext.Log.Info($"Возврат платежа {sum} за заказ  {orderId}");
            var order = GetOrderSafe(orderId);
            CollectedData data = null;
            data = LoadOrder(order.Id);
            if (data == null)
            {
                ///throw new PaymentActionFailedException($"Ошибка при получение данных(GetCustomData) при возврате оплаты." + Environment.NewLine + "Обратитесь к системному администратору.");
                viewManager.ShowErrorPopup($"Ошибка при получение данных(GetCustomData) при возврате оплаты." + Environment.NewLine + "Обратитесь к системному администратору.");
                throw new PaymentActionCancelledException();
            }
            try
            {
                if (data != null)
                {
                    PluginContext.Log.Info($"data {data.ToString()}");
                }
            }
            catch { }

            var delOrder = _MomsRecipesAPI.GetDeleteOrder(data.TranssactionId);
            if (!String.IsNullOrEmpty(delOrder.message))
            {
                ///throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + delOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                viewManager.ShowErrorPopup($"Ошибка ответа от API" + Environment.NewLine + delOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                throw new PaymentActionCancelledException();
            }
        }

        public void OnPaymentAdded([NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, [NotNull] IUser cashier, [NotNull] IOperationService operationService, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            PluginContext.Log.Info("OnPaymentAdded");
            CollectedData data = null;

            data = LoadOrder(order.Id);
            if (data == null)
            {
                ///throw new PaymentActionFailedException($"Ошибка при получение данных(OnPaymentAdded) при добавление типа оплаты." + Environment.NewLine + "Обратитесь к системному администратору.");
                viewManager.ShowErrorPopup($"Ошибка при получение данных(OnPaymentAdded) при добавление типа оплаты." + Environment.NewLine + "Обратитесь к системному администратору.");
                throw new PaymentActionCancelledException();
            }
            try
            {
                if (data != null)
                {
                    PluginContext.Log.Info($"data {data.ToString()}");
                }
            }
            catch { }
           
            decimal _ResultSum = order.ResultSum;
            PluginContext.Log.Info($"===>Список скидок в заказ(OnPaymentAdded):");
            foreach (var d in order.Discounts)
            {
                PluginContext.Log.Info($"====>order.Discount - {d.DiscountType.Name} - {d.DiscountType.IsActive}");
            }
            PluginContext.Log.Info($"===>Список продуктов в заказ(OnPaymentAdded).");
            foreach (var p in order.Items.OfType<IOrderProductItem>().ToList())
            {
                PluginContext.Log.Info($"====>{p.Id} - {p.Product.Name} - {p.Amount} - {p.Deleted}");
                if (p.AssignedModifiers != null)
                {
                    foreach (var t in p.AssignedModifiers)
                    {
                        PluginContext.Log.Info($"AssignedModifiers    {t.Id} - {t.Product.Name} - {t.Amount} - {t.Price}");
                    }
                }
            }

            try
            {
                Decimal Rub = (Decimal)(data.bonus_used);
                order = GetOrderSafe(order.Id);
                switch (order.Status)
                {
                    case OrderStatus.New:
                        PluginContext.Log.Info($"OrderStatus.New - ResultSum={_ResultSum} ");
                        operationService.ChangePaymentItemSum((decimal)Rub, 0m, null, paymentItem, order, PluginContext.Operations.GetCredentials());
                        break;
                    case OrderStatus.Bill:
                        PluginContext.Log.Info($"OrderStatus.Bill - ResultSum={_ResultSum} / Rub={Rub}");
                        // we cannot edit order widely after bill, but at least we can set possible sum range and provide initial sum value
                        operationService.ChangePaymentItemSum((decimal)Rub, 0m, null, paymentItem, order, PluginContext.Operations.GetCredentials());
                        break;
                    default:
                        // we don't expect payment item to be added in statuses other than new and bill
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                PluginContext.Log.InfoFormat($"Exception(465)", e);
            }
            var sendOrder = Mapper.Map(order);
            sendOrder.sum = order.ResultSum;
            sendOrder.bonus_used = data.bonus_used;
            sendOrder.customer_id = data.customer.id;
            sendOrder.phone = data.customer.phone;
            sendOrder.name = data.customer.name;
            try
            {
                if (data.TranssactionId > 0)
                {
                    /// что-то уже отправляли раньше                    
                    var createOrder = _MomsRecipesAPI.GetUpdateOrder(sendOrder, data.TranssactionId);
                    if (!String.IsNullOrEmpty(createOrder.message))
                    {
                        ///throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                        viewManager.ShowErrorPopup($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                        throw new PaymentActionCancelledException();
                    }
                }
                else
                {
                    var createOrder = _MomsRecipesAPI.GetCreateOrder(sendOrder);
                    if (!String.IsNullOrEmpty(createOrder.message))
                    {
                        ///throw new PaymentActionFailedException($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                        viewManager.ShowErrorPopup($"Ошибка ответа от API" + Environment.NewLine + createOrder.message + Environment.NewLine + "Обратитесь к системному администратору.");
                        throw new PaymentActionCancelledException();
                    }
                    else {
                        if (createOrder != null)
                        {
                            data.TranssactionId = createOrder.id;
                        }
                    }
                }
                SaveOrder(data);
            }
            catch (Exception e)
            {
                //throw new PaymentActionFailedException($"Обнаружен следующий ответ от API - {e.ToString()}" + Environment.NewLine + "Обратитесь к системному администратору.");
                viewManager.ShowErrorPopup($"Обнаружен следующий ответ от API - {e.ToString()}" + Environment.NewLine + "Обратитесь к системному администратору.");
                throw new PaymentActionCancelledException();
            }
            
        }

        public bool OnPreliminaryPaymentEditing([NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, [NotNull] IUser cashier, [NotNull] IOperationService operationService, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            PluginContext.Log.Info("OnPreliminaryPaymentEditing");
            return true;

        }

        public void ReturnPaymentWithoutOrder(decimal sum, Guid? orderId, Guid paymentTypeId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, [NotNull] IViewManager viewManager)
        {
            PluginContext.Log.Info("ReturnPaymentWithoutOrder");
        }

        public void PaySilently(decimal sum, [NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, IPaymentDataContext context)
        {
            PluginContext.Log.Info("PaySilently");
        }

        public void EmergencyCancelPaymentSilently(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, IPaymentDataContext context)
        {
            PluginContext.Log.Info("EmergencyCancelPaymentSilently");
        }

        public bool CanPaySilently(decimal sum, Guid? orderId, Guid paymentTypeId, IPaymentDataContext context)
        {
            PluginContext.Log.Info("CanPaySilently");
            return false;

        }
#endregion

        [CanBeNull]
        private IOrder GetOrderSafe(Guid? orderId)
        {
            return orderId.HasValue ? PluginContext.Operations.TryGetOrderById(orderId.Value) : null;
        }

        private string GetOrderDataPath(Guid orderId)
        {
            var id = orderId.ToString();
            var directory = Path.Combine(PluginContext.Integration.GetDataStorageDirectoryPath(), id[0].ToString(), id[1].ToString(), id[2].ToString());
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return Path.Combine(directory, $"{id}.json");
        }

        public void SaveOrder(CollectedData data)
        {
            PluginContext.Log.Info($"Binding order #{data.OrderId}");
            lock (SyncRoot)
            {
                using (var sw = new StreamWriter(GetOrderDataPath(data.OrderId), false))
                {
                    sw.Write(JsonConvert.SerializeObject(data));
                }
            }
        }

        public CollectedData LoadOrder(Guid orderId)
        {
            lock (SyncRoot)
            {
                var dataFile = GetOrderDataPath(orderId);
                if (!File.Exists(dataFile))
                    return new CollectedData() { OrderId = orderId};
                using (var sr = new StreamReader(dataFile))
                {
                    return JsonConvert.DeserializeObject<CollectedData>(sr.ReadToEnd());
                }
            }
        }


    }
}
