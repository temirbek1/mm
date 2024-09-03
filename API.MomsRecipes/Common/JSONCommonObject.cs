using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API
{
    public class Errors
    {
        public List<string> guid { get; set; }
    }

    public abstract class JSONCommonObject
    {
        public string message { get; set; }
        public Errors errors { get; set; }
        public static T deserialize<T>(string value)
        {
            return String.IsNullOrEmpty(value)
                ? (T)Activator.CreateInstance(typeof(T))
                : JsonConvert.DeserializeObject<T>(value);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,                                
                                DateFormatString = "yyyy-MM-dd hh:mm:ssZ",
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            });
        }
    }
}
