using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpNotImplementedResponse : HttpExceptionResponseBase<NotImplementedException>
    {
        public HttpNotImplementedResponse()
        {
            Header.ResponseCode = "HTTP/1.1 501 Not Implemented";
        }
    }
}
