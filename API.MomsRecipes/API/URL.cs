using API;
using API.MomsRecipes;
using System;

namespace Api.MomsRecipes
{
    public class URL
    {
        private static string Base = "https://{0}/api/";
        private static string GetLogin = "login";
        private static string GetCustomerById = "customers/{0}";
        private static string GetCustomerByPhone = "customers?where=phone(=){0}";
        
        private static string CreateOrder = "orders";
        private static string UpdateOrder = "orders/{0}";
        private static string DeleteOrder = "orders/{0}";

        public static string getBaseUrl()
        {
            return String.Format(Base, APIWorker.GetInstance()._settings.pluginSettings.APIUrl);
        }

        public static string GetAuthUrl()
        {
            return getBaseUrl() + GetLogin;
        }
        public static string GetCustomerByIdUrl(string CustomerId)
        {
            return getBaseUrl() + String.Format(GetCustomerById, CustomerId);
        }

        public static string GetCreateOrderUrl()
        {
            return getBaseUrl() + CreateOrder;
        }
        public static string GetUpdateOrderUrl(int TranssactionId)
        {
            return getBaseUrl() + String.Format(UpdateOrder, TranssactionId); 
        }
        public static string GetDeleteOrderUrl(int TranssactionId)
        {
            return getBaseUrl() + String.Format(DeleteOrder, TranssactionId);
        }

        public static string GetCustomerByPhoneUrl(string phone)
        {
            return getBaseUrl() + String.Format(GetCustomerByPhone, phone);
        }
    }
}
