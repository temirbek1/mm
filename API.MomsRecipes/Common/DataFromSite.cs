using Api;
using API.MomsRecipes;
using Newtonsoft.Json;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace API
{
    public struct AnswerHttp
    {
        public String text;
        public APIAnswer result;
    }

    public class DataFromSite
    {
        private static ILogService _logger;
        private static DataFromSite _DataFromSite;     
        
        public static DataFromSite GetInstance(ILogService logger)
        {
            if (_DataFromSite == null)
            {
                lock (typeof(DataFromSite))
                {
                    if (_DataFromSite == null)
                    {
                        _DataFromSite = new DataFromSite(logger);
                        _DataFromSite.Init();
                    }
                }
            }
            return _DataFromSite;
        }

        public void Init()
        {
        }


        public DataFromSite(ILogService logger)
        {
            _logger = logger;
        }

        public AnswerHttp getDataFromUrl(string url, string typeMethod = "GET")
        {
            AnswerHttp tmpAnswerHttp = new AnswerHttp();
            try
            {
                return MakeRequestAsync(url, tmpAnswerHttp, typeMethod);
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Ошибка при запросе - " + url, e);
            }   
            ;           
            return tmpAnswerHttp;
        }

        private AnswerHttp MakeRequestAsync(string tmpUrl, AnswerHttp tmpAnswerHttp, string typeMethod = "GET")
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            try
            {
                _logger.Info("Запрос - " + tmpUrl);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(tmpUrl);
                httpWebRequest.Timeout = 30000;
                httpWebRequest.Method = typeMethod;
                httpWebRequest.UserAgent = "IIKO.MomsRecipe/1.1";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + APIWorker.GetInstance().AuthToken);
                _logger.Info($"Запрос - {tmpUrl} - {JsonConvert.SerializeObject(httpWebRequest)}");

                var asyncResult = httpWebRequest.BeginGetResponse(null, null);

                asyncResult.AsyncWaitHandle.WaitOne();

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult))
                {
                    using (var responseStreamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        String context = responseStreamReader.ReadToEnd();                        
                        context = UnicodeStringExtensions.DecodeEncodedNonAsciiCharacters(context);
                        _logger.Error($"От API - {context}");
                        tmpAnswerHttp.text = context;
                        tmpAnswerHttp.result = (APIAnswer)httpWebResponse.StatusCode;                        
                        return tmpAnswerHttp;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("Ошибка при запросе - " + e);
                if (e is WebException)
                {
                    var wex = (WebException)e;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)wex.Response;
                    using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        String context = reader.ReadToEnd();
                        context = UnicodeStringExtensions.DecodeEncodedNonAsciiCharacters(context);
                        _logger.Error($"От API - {context}");
                        tmpAnswerHttp.result = (APIAnswer)httpWebResponse.StatusCode;
                        tmpAnswerHttp.text = context;
                    };
                }
                
            }
            return tmpAnswerHttp;
        }

        public AnswerHttp PostDataToUrl(string tmpUrl, string postData, string typeMethod = "POST")
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            AnswerHttp tmpAnswerHttp = new AnswerHttp();
            try
            {
                _logger.Info("Запрос - " + tmpUrl);
                var request = (HttpWebRequest)WebRequest.Create(tmpUrl);
                request.ContentType = "application/json";
                request.Method = typeMethod;
                request.UserAgent = "IIKO.MomsRecipe/1.1";
                request.KeepAlive = true;
                request.Accept = "application/json";
                request.Headers.Add("Authorization", "Bearer " + APIWorker.GetInstance().AuthToken);
                _logger.Info($"Запрос - {tmpUrl} - {JsonConvert.SerializeObject(request)}");
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = postData;
                    _logger.Info("postData - " + postData);
                    streamWriter.Write(json);
                }

                HttpWebResponse _httpWebResponse = (HttpWebResponse)request.GetResponse();                
                using (StreamReader reader = new StreamReader(_httpWebResponse.GetResponseStream()))
                {
                    String context = reader.ReadToEnd();                    
                    context = UnicodeStringExtensions.DecodeEncodedNonAsciiCharacters(context);
                    _logger.Error($"От API - {context}");
                    tmpAnswerHttp.result = (APIAnswer)_httpWebResponse.StatusCode;
                    tmpAnswerHttp.text = context;
                };                
            }
            catch (Exception e)
            {
                _logger.Error($"Ошибка при выгрузке данных - {tmpUrl}", e);
                if (e is WebException)
                {
                    var wex = (WebException)e;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)wex.Response;
                    using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        String context = reader.ReadToEnd();
                        context = UnicodeStringExtensions.DecodeEncodedNonAsciiCharacters(context);
                        _logger.Error($"От API - {context}");
                        tmpAnswerHttp.result = (APIAnswer)httpWebResponse.StatusCode;
                        tmpAnswerHttp.text = context;
                    };
                }
            }
            return tmpAnswerHttp;
        }

    }
}
