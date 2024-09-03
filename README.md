CollectData - выводим запрос на данные клиента(GetCustomerById/GetCustomerByPhone) + запрашиваем сколько денег списать.
OnPaymentAdded - добавляем сумму указанную ранее(CollectData) в заказ + GetCreateOrder - отправляем введённую пользователем сумму.
Pay - фиксация оплаты - GetCreateOrder - ищём наш тип оплаты и берём из него сумму введённую или 0.
Далее ждём когда в OnOrderChanged:
-прилетит статус заказа Closed - то есть всё он закрыт и шлём GetUpdateOrder в котором ищём наш тип оплаты и берём из него сумму введённую или 0.
-прилетит статус заказа Deleted - шлём GetDeleteOrder