using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Http.Exceptions;

namespace Pandora.Http
{
    public interface IHttpExceptionResponse : IHttpResponse
    {
        string ExceptionTypeName { get; }
    }
}
