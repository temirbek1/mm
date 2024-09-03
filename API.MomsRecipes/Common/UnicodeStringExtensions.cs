using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api
{
    public static class UnicodeStringExtensions
    {

        public static string DecodeEncodedNonAsciiCharacters(this string value)
          => Regex.Replace(value,/*language=regexp*/@"(?:\\u[a-fA-F0-9]{4})+", Decode);

        static readonly string[] Splitsequence = new[] { @"\u" };
        private static string Decode(Match m)
        {
            var bytes = m.Value.Split(Splitsequence, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => ushort.Parse(s, NumberStyles.HexNumber)).SelectMany(BitConverter.GetBytes).ToArray();
            return Encoding.Unicode.GetString(bytes);
        }
    }
}
