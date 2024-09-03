using Api.MomsRecipes;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;
using System;

namespace API.MomsRecipes
{


    public class APIWorker
    {
        private ILogService _logger;

        public Settings _settings;
        private static APIWorker _APIWorker;
        private DataFromSite _dataFromSite;
        public String AuthToken;

        public static APIWorker GetInstance(ILogService logger)
        {

            if (_APIWorker == null)
            {
                lock (typeof(APIWorker))
                {
                    if (_APIWorker == null)
                    {
                        _APIWorker = new APIWorker(logger);
                        _APIWorker.Init();
                    }
                }
            }
            return _APIWorker;
        }

        public static APIWorker GetInstance()
        {
            return _APIWorker;
        }



        public void Init()
        {
            _settings = new Settings(_logger);
            _dataFromSite = DataFromSite.GetInstance(_logger);            
        }

        public APIWorker(ILogService logger)
        {
            _logger = logger;
        }

        public AuthJSONAnswer GetAuth()
        {
            String context = String.Empty;
            String url = URL.GetAuthUrl();
            AuthJSONRequest postData = new AuthJSONRequest() { email = _settings.pluginSettings.Login, password = _settings.pluginSettings.Pass };
            context = _dataFromSite.PostDataToUrl(url, postData.ToString()).text;

            AuthJSONAnswer result = null;
            if (!String.IsNullOrEmpty(context))
            {
                result = AuthJSONAnswer.deserialize<AuthJSONAnswer>(context);
                AuthToken = result.token;
                _logger.Info(result.ToString());
            }
            else
            {
                AuthToken = null;
            }
            return result;
        }

        public CustomerAnswer GetCustomerById(String CustomerId)
        {
            if (string.IsNullOrEmpty(AuthToken))
            {
                GetAuth();
            }
            String context = String.Empty;
            String url = URL.GetCustomerByIdUrl(CustomerId);
            CustomerAnswer result = null;
            var answer = _dataFromSite.getDataFromUrl(url);
            context = answer.text;
            if (answer.result != APIAnswer.OkRequest)
            {
                AuthToken = null;
                result = CustomerAnswer.deserialize<CustomerAnswer>(context);
                return result;
            }
                        
            if (!String.IsNullOrEmpty(context))
            {
                result = CustomerAnswer.deserialize<CustomerAnswer>(context);
                _logger.Info(result.ToString());
            }
            else {
                AuthToken = null;
            }
            return result;
        }
        public CustomerSearchAnswer GetCustomerByPhone(String phone)
        {
            phone = phone.Replace("+", "%2B");
            
            if (string.IsNullOrEmpty(AuthToken))
            {
                GetAuth();
            }
            String context = String.Empty;
            String url = URL.GetCustomerByPhoneUrl(phone);
            CustomerSearchAnswer result = null;
            var answer = _dataFromSite.getDataFromUrl(url);
            context = answer.text;
            if (answer.result != APIAnswer.OkRequest)
            {
                AuthToken = null;
                result = CustomerSearchAnswer.deserialize<CustomerSearchAnswer>(context);
                return result;
            }
                        
            if (!String.IsNullOrEmpty(context))
            {
                result = CustomerSearchAnswer.deserialize<CustomerSearchAnswer>(context);
                _logger.Info(result.ToString());
            }
            else
            {
                AuthToken = null;
            }
            return result;
        }

        public CreaterOrderAnswer GetCreateOrder(APIOrderInfo postData)
        {
           
            if (string.IsNullOrEmpty(AuthToken))
            {
                GetAuth();
            }
            String context = String.Empty;
            String url = URL.GetCreateOrderUrl();
            var answer = _dataFromSite.PostDataToUrl(url, postData.ToString());
            CreaterOrderAnswer result = null;
            context = answer.text;
            if (answer.result != APIAnswer.OkRequest)
            {
                AuthToken = null;
                result = CreaterOrderAnswer.deserialize<CreaterOrderAnswer>(context);
                return result;
            }
            
            
            if (!String.IsNullOrEmpty(context))
            {
                result = CreaterOrderAnswer.deserialize<CreaterOrderAnswer>(context);
                _logger.Info(result.ToString());
            }
            else
            {
                AuthToken = null;
            }
            return result;
        }

        public CreaterOrderAnswer GetUpdateOrder(APIOrderInfo postData, int OrderId)
        {
            if (string.IsNullOrEmpty(AuthToken))
            {
                GetAuth();
            }
            String context = String.Empty;
            String url = URL.GetUpdateOrderUrl(OrderId);
            CreaterOrderAnswer result = null;
            var answer = _dataFromSite.PostDataToUrl(url, postData.ToString(), "PUT");
            context = answer.text;
            if (answer.result != APIAnswer.OkRequest)
            {
                AuthToken = null;
                result = CreaterOrderAnswer.deserialize<CreaterOrderAnswer>(context);
                return result;
            }
                        
            if (!String.IsNullOrEmpty(context))
            {
                result = CreaterOrderAnswer.deserialize<CreaterOrderAnswer>(context);
                _logger.Info(result.ToString());
            }
            else
            {
                AuthToken = null;
            }
            return result;
        }

        public DeleteOrderAnswer GetDeleteOrder(int TranssactionId)
        {
            if (string.IsNullOrEmpty(AuthToken))
            {
                GetAuth();
            }
            String context = String.Empty;
            String url = URL.GetDeleteOrderUrl(TranssactionId);
            DeleteOrderAnswer result = new DeleteOrderAnswer();
            var answer = _dataFromSite.PostDataToUrl(url, "{ \"force\": \"false\"}", "DELETE");
            context = answer.text;
            if (answer.result != APIAnswer.OkRequest)
            {
                AuthToken = null;
                return result;
            }
            
            if (!String.IsNullOrEmpty(context))
            {
                _logger.Info(result.ToString());
            }
            else
            {
                AuthToken = null;
            }
            return result;
        }
    }
}
