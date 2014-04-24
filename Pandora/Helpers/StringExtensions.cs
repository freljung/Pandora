using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pandora.Helpers
{
    public static class EncodedBufferExtensions
    {
        public static string ConvertEncodingToString(this byte[] source, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetString(source);
        }

        public static string ConvertISO_8859_1ToString(this byte[] source)
        {
            return source.ConvertEncodingToString("ISO-8859-1");
        }

        public static string ConvertUTF8ToString(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }

        public static byte[] ConvertStringToISO_8859_1ByteArray(this string source)
        {
            return Encoding.GetEncoding("ISO-8859-1").GetBytes(source);
        }
    }
}
