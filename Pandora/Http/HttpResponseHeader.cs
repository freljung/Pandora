using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pandora.Http
{
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
}
