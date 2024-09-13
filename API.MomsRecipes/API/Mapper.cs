using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.MomsRecipes
{
    public static class Mapper
    {
        public static APIProduct Map(IOrderProductItem source)
        {
            if (source == null)
            {
                return null;
            }
            return new APIProduct()
            {
                product_guid = source.Product.Id.ToString(),
                title = source.Product.Name,
                count = source.Amount,
                price = source.Product.Price,
                sum = source.Amount * source.Product.Price
            };
        }

        public static IEnumerable<APIProduct> Map(IEnumerable<IOrderProductItem> source)
        {
            return source?.Select(Map);
        }

        private static List<string> BuildPaymentsGUID(IOrder order)
        {
            return order.Payments
                .Where(payment => payment != null
                                  && payment.Status != PaymentStatus.Cancelled
                                  && payment.Status != PaymentStatus.Storned)
                .Select(payment => payment.Type.Id.ToString()).ToList();
        }

        public static APIOrderInfo Map(IOrder source)
        {
            if (source == null)
            {
                return null;
            }

            var paymentStatus = source.Payments.Count > 0 ? source.Payments[0].Status.ToString() : "";
            var deliveryDate = (source.CloseTime ?? source.BillTime ?? source.OpenTime)?.ToString("yyyy-MM-dd HH:mm") ?? "";

            var dest = new APIOrderInfo()
            {
                guid = source.Id.ToString(),
                count = source.Items?.Count ?? 0,
                status = Enum.GetName(typeof(OrderStatus), source.Status),
                delivery = "Common",
                delivery_sum = 0,
                payment_status = paymentStatus,
                payment_guid = BuildPaymentsGUID(source),
                comments = string.IsNullOrEmpty(source.Comment) ? "Заказ из кассы" : source.Comment,
                manager_comments = source.Comment,
                products = Map(source.Items.OfType<IOrderProductItem>())?.ToList(),
                delivery_date = deliveryDate
            };

            ///test
#if DEBUG
            if (dest.products != null && dest.products.Count > 0)
                dest.products[0].product_guid = "6689bcd9-7e51-4fb6-897c-052390fd717c";

            if (source.Payments.Count > 0)
                dest.payment_guid[0] = "09322f46-578a-d210-add7-eec222a08871";

            if (source.Payments.Count > 1)
                dest.payment_guid[1] = "861c0432-94a0-43d3-bfa7-02c1254450fd";
#endif

            return dest;
        }
    }
}
