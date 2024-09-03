using API;
using System;

namespace Resto.Front.Api.MomsRecipes
{

    public class CollectedData : JSONCommonObject
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public Guid OrderId;
        /// <summary>
        /// идентификатор транзакции на стороне API
        /// </summary>
        public int TranssactionId;
        /// <summary>
        /// Сколько стоил заказ
        /// </summary>
        public Decimal bonus_used;
        /// <summary>
        /// дата/время создания транзакции
        /// </summary>
        public DateTime DTCreateTransaction;
        /// <summary>
        /// клиент из заказа
        /// </summary>
        public API.MomsRecipes.Customer customer;
    }
}