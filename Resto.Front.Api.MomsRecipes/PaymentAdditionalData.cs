using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Resto.Front.Api.MomsRecipes
{
    [Serializable]
    public sealed class PaymentAdditionalData
    {
        public bool SilentPay { get; set; }
    }

    internal static class Serializer
    {
        internal static string Serialize<T>(T data) where T : class
        {
            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, data);
                return sw.ToString();
            }
        }
    }
}
