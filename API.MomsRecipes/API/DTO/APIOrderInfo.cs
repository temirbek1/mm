using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace API.MomsRecipes
{
    [DataContract]
    public class APIProduct : JSONCommonObject
    {
        /// <summary>
        /// id товара
        /// </summary>
        [DataMember]
        public String id { get; set; }
        /// <summary>
        /// id товара в Laravel
        /// </summary>
        [DataMember]
        public String product_guid { get; set; }
        /// <summary>
        /// название товара
        /// </summary>
        [DataMember]
        public string title { get; set; }
        /// <summary>
        /// кол-во
        /// </summary>
        [DataMember]
        public decimal count { get; set; }
        /// <summary>
        /// цена
        /// </summary>
        [DataMember]
        public decimal price { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        [DataMember]
        public decimal sum { get; set; }
    }

    [DataContract]
    public class APIOrderInfo : JSONCommonObject
    {
        /// <summary>
        /// статус заказа
        /// </summary>
        [DataMember]
        public string guid { get; set; }
        /// <summary>
        /// id покупателя в Laravel
        /// </summary>
        [DataMember]
        public int customer_id { get; set; }
        /// <summary>
        /// кол-во товаров в заказе
        /// </summary>
        [DataMember]
        public int count { get; set; }
        /// <summary>
        /// статус заказа
        /// </summary>
        [DataMember]
        public string status { get; set; }
        /// <summary>
        /// сумма заказа
        /// </summary>
        [DataMember]
        public decimal sum { get; set; }
        /// <summary>
        /// начислено бонусов (не понадобится скорее всего вам)
        /// </summary>
        [DataMember]
        public Decimal bonus_added { get; set; }
        /// <summary>
        /// оплачено бонусами
        /// </summary>
        [DataMember]
        public Decimal bonus_used { get; set; }
        /// <summary>
        /// способ доставки
        /// </summary>
        [DataMember]
        public string delivery { get; set; }
        /// <summary>
        /// время доставки в формате datetime
        /// </summary>
        [DataMember]
        public string delivery_date { get; set; }
        /// <summary>
        /// стоимость доставки
        /// </summary>
        [DataMember]
        public Decimal delivery_sum { get; set; }
        [DataMember]
        public string address { get; set; }
        /// <summary>
        /// телефон покупателя
        /// </summary>
        [DataMember]
        public string phone { get; set; }
        /// <summary>
        /// имя покупателя
        /// </summary>
        [DataMember]
        public string name { get; set; }
        /// <summary>
        /// статус оплаты заказа
        /// </summary>
        [DataMember]
        public string payment_status { get; set; }
        /// <summary>
        /// список типов оплаты
        /// </summary>
        [DataMember]
        public List<String> payment_guid { get; set; }
        /// <summary>
        /// комментарий к заказу
        /// </summary>
        [DataMember]
        public string comments { get; set; }
        /// <summary>
        /// комментарий от сотрудника
        /// </summary>
        [DataMember]
        public string manager_comments { get; set; }
        [DataMember]
        public string additional { get; set; }
        /// <summary>
        /// список товаров
        /// </summary>
        [DataMember]
        public List<APIProduct> products { get; set; }
    }
}