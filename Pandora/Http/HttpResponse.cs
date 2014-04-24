using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponseHeader Header { get; private set; }

        public HttpResponse()
        {
            Header = new HttpResponseHeader();
        }

        public byte[] GetResponseAsByteArray()
        {
            return Header.ToByteArray();
        }
    }
}
