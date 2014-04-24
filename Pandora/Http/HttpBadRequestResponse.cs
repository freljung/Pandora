using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Http.Exceptions;

namespace Pandora.Http
{
    public class HttpBadRequestResponse : HttpExceptionResponseBase<BadRequestException>
    {
        public HttpBadRequestResponse()
        {
            Header.ResponseCode = "HTTP/1.1 400 Bad Request";
        }
    }
}
