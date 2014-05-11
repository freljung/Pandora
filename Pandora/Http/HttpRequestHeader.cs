using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpRequestHeader : HttpHeader
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public int HeaderLength { get; set; }
    }
}
