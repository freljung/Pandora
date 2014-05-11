using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpCall
    {
        public ServerSettings ServerSettings { get; set; }
        public HttpRequestHeader RequestHeader { get; set; }
        public byte[] RawRequestBody { get; set; }
        public HttpResponseHeader ResponseHeader { get; set; }
    }
}
