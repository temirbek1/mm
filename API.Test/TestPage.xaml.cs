using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace API.Test
{
    public partial class TestPage : ContentPage
    {
        private static ILogService _logger;
        CustomerAnswer Customer;

        public TestPage()
        {
            InitializeComponent();
            _logger = new NlogLogService();
            APIWorker.GetInstance(_logger);
        }

        private void OnGetOrderClicked(object sender, EventArgs e)
        {
            var res = APIWorker.GetInstance().GetDeleteOrder(123);
            ResultLabel.Text += res.ToString();
        }

        private void OnGetCustomerByIdClicked(object sender, EventArgs e)
        {
            Customer = APIWorker.GetInstance().GetCustomerById("1");
            ResultLabel.Text += Customer.ToString();
        }

        private void OnCreateOrderClicked(object sender, EventArgs e)
        {
            if (Customer == null)
            {
                OnGetCustomerByIdClicked(null, null);
            }

            APIOrderInfo order = new APIOrderInfo()
            {
                customer_id = Customer.data.id,
                count = 1,
                status = OrderStatus.New.ToString(),
                sum = 1,
                delivery = "Common",
                delivery_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm"),
                phone = "+71234567890",
                name = "Иванов Иван Иванович",
                payment_status = PaymentStatus.New.ToString(),
                comments = "comments",
                manager_comments = "manager_comments",
                products = new List<APIProduct>()
                {
                    new APIProduct()
                    {
                        product_guid = "6689bcd9-7e51-4fb6-897c-052390fd717c",
                        count = 1,
                        price = 10.2M,
                        sum = 10.2M,
                        title = "не знаю"
                    }
                }
            };

            var res = APIWorker.GetInstance().GetCreateOrder(order);
            ResultLabel.Text += res.ToString();
        }

        private void OnCancelOperationClicked(object sender, EventArgs e)
        {
            var res = APIWorker.GetInstance().GetAuth();
            ResultLabel.Text += res.ToString();
        }

        private void OnUpdateOrderClicked(object sender, EventArgs e)
        {
            APIOrderInfo order = new APIOrderInfo()
            {
                customer_id = Customer.data.id,
                count = 1,
                status = OrderStatus.New.ToString(),
                sum = 1,
                delivery = "Common",
                delivery_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm"),
                phone = "+71234567890",
                name = "Иванов Иван Иванович",
                payment_status = PaymentStatus.New.ToString(),
                comments = "comments",
                manager_comments = "manager_comments",
                products = new List<APIProduct>()
                {
                    new APIProduct()
                    {
                        product_guid = "6689bcd9-7e51-4fb6-897c-052390fd717c",
                        count = 1,
                        price = 10.2M,
                        sum = 10.2M,
                        title = "не знаю"
                    }
                }
            };

            var res = APIWorker.GetInstance().GetUpdateOrder(order, 6303);
            ResultLabel.Text += res.ToString();
        }

        private void OnGetCustomerByPhoneClicked(object sender, EventArgs e)
        {
            var customer = APIWorker.GetInstance().GetCustomerByPhone("+996700126646").data[0];
            ResultLabel.Text += customer.ToString();
        }
    }
}
