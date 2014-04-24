using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pandora.Helpers;

namespace Pandora.Http
{
    public class HttpCall
    {
        public HttpRequestHeader RequestHeader { get; set; }
        public byte[] RawRequestBody { get; set; }
        public HttpResponseHeader ResponseHeader { get; set; }
    }

    public class HttpHeader
    {
        public string Version { get; set; }

        public Dictionary<string, string> Rows { get; private set; }

        public HttpHeader()
        {
            Rows = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddRow(KeyValuePair<string, string> headerRow)
        {
            Rows.Add(headerRow.Key.Trim(), headerRow.Value.Trim());
        }
    }

    public class HttpRequestHeader : HttpHeader
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public int HeaderLength { get; set; }
    }

    public class HttpResponseHeader : HttpHeader
    {
        public string ResponseCode { get; set; }

        public HttpResponseHeader()
        {
            Rows.Add("Server", "Pandora/0.1");
        }

        public byte[] ToByteArray()
        {
            using (var outStream = new MemoryStream())
            {
                outStream.WriteLine(ResponseCode);
                outStream.WriteRows(Rows);
                return outStream.ToArray();
            }
        }
    }

    public static class StreamExtensions
    {
        public static void WriteLine(this Stream source, string str)
        {
            var buffer = string.Concat(str, Environment.NewLine).ConvertStringToISO_8859_1ByteArray();
            source.Write(buffer, 0, buffer.Length);
        }

        public static void WriteRows(this Stream source, Dictionary<string, string> rows)
        {
            foreach (var row in rows)
            {
                var buffer = string.Concat(row.Key, ":", row.Value, Environment.NewLine).ConvertStringToISO_8859_1ByteArray();
                source.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
