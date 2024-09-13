using System;
using System.Windows;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Attributes.JetBrains;


namespace Resto.Front.Api.MomsRecipes
{
    internal sealed class OrderExtender : IDisposable
    {
        [NotNull]
        private readonly IDisposable subscription;
        private IObservable<IOrder> iObservable;
        private IOperationService _operationService;
        
        public OrderExtender([NotNull] IObservable<IOrder> orderObservable, IOperationService operationService)
        {
            //UiDispatcher.Execute(() => MessageBox.Show("start"));
            if (orderObservable == null)
                throw new ArgumentNullException("orderObservable");

            _operationService = operationService;
            subscription = orderObservable.Subscribe(ChangeOrderExtensions);
        }
       

        public void Dispose()
        {
            subscription.Dispose();
        }


        private void ChangeOrderExtensions([NotNull] IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException("billCheque");
            if (order.Status.Equals(OrderStatus.Closed))
            {
                /*UiDispatcher.Execute(() => MessageBox.Show("Чек закрыт."));
                String tmpStr = String.Format("Данные чека:Официант - {0};\r\n", order.Waiter.Name);
                tmpStr += String.Format("Сумма - {0};\r\n", order.ResultSum);
                var productList = order.GetProducts(_operationService);
                if (!productList.IsEmpty())
                {
                    foreach (var p in productList)
                    {
                        tmpStr += String.Format("Название - {0};", p.Product.FullName);
                        tmpStr += String.Format("Цена - {0};", p.Cost);
                        tmpStr += String.Format("Кол-во - {0};\r\n", p.Amount);
                    }
                }
                UiDispatcher.Execute(() => MessageBox.Show(tmpStr));*/
            }
        }
    }
}
