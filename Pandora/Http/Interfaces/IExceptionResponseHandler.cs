using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public interface IExceptionResponseHandler
    {
        void RegisterResponse(IHttpExceptionResponse response);
        void RegisterRepsonseRange(IEnumerable<IHttpExceptionResponse> responses);
        IHttpResponse GetResponseFromException(Exception ex);
        bool HasResponse(string exceptionName);
    }
}
