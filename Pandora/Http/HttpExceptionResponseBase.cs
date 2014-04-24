using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    abstract public class HttpExceptionResponseBase<TException> : HttpResponse, IHttpExceptionResponse
            where TException : Exception
    {
        public string ExceptionTypeName
        {
            get { return typeof(TException).Name; }
        }
    }
}
