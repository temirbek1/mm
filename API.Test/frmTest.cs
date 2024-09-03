using API.MomsRecipes;
using Resto.Front.Api.Core.Logging;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;
using System;
using System.Windows.Forms;

namespace API.Test
{
    public partial class frmTest : Form
    {
        private static ILogService _logger;
        CustomerAnswer Customer;
        public frmTest()
        {
            InitializeComponent();
            _logger = new NlogLogService();
            APIWorker.GetInstance(_logger);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var res = APIWorker.GetInstance().GetDeleteOrder(123);
            textBox1.Text += res.ToString();
        }
        
        private void frmTest_Load(object sender, EventArgs e)
        {

        }

        private void btnBalanceByNumber_Click(object sender, EventArgs e)
        {
            Customer = APIWorker.GetInstance().GetCustomerById("1");
            textBox1.Text += Customer.ToString();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            if (Customer == null)
            {
                btnBalanceByNumber_Click(null, null);
            }
            APIOrderInfo order = new APIOrderInfo()
            {
                customer_id = Customer.data.id,
                count = 1,
                status = OrderStatus.New.ToString(),
                sum = 1,
                delivery = "Common",
                delivery_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm"),
                //bonus_used = source.
                phone = "+71234567890",
                name = "Иванов Иван Иванович",
                payment_status = PaymentStatus.New.ToString(),
                comments = "comments",
                manager_comments = "manager_comments",
                products = new System.Collections.Generic.List<APIProduct>() { new APIProduct() { product_guid = "6689bcd9-7e51-4fb6-897c-052390fd717c", count = 1, price = 10.2M, sum = 10.2M, title = "не знаю" } }
            };
            
            var res = APIWorker.GetInstance().GetCreateOrder(order);
            textBox1.Text += res.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {


        }

        private void btnCancelOperation_Click(object sender, EventArgs e)
        {
            var res = APIWorker.GetInstance().GetAuth();
            textBox1.Text += res.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            APIOrderInfo order = new APIOrderInfo()
            {
                customer_id = Customer.data.id,
                count = 1,
                status = OrderStatus.New.ToString(),
                sum = 1,
                delivery = "Common",
                delivery_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm"),
                //bonus_used = source.
                phone = "+71234567890",
                name = "Иванов Иван Иванович",
                payment_status = PaymentStatus.New.ToString(),
                comments = "comments",
                manager_comments = "manager_comments",
                products = new System.Collections.Generic.List<APIProduct>() { new APIProduct() { product_guid = "6689bcd9-7e51-4fb6-897c-052390fd717c", count = 1, price = 10.2M, sum = 10.2M, title = "не знаю" } }
            };

            var res = APIWorker.GetInstance().GetUpdateOrder(order, 6303);
            textBox1.Text += res.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var  Customer = APIWorker.GetInstance().GetCustomerByPhone("+996700126646").data[0];
            textBox1.Text += Customer.ToString();
        }
    }
}
