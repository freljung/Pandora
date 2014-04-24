using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Http.Exceptions;

namespace Pandora.Http
{
    public class ExceptionResponseHandler : IExceptionResponseHandler
    {
        Dictionary<string, IHttpExceptionResponse> _exceptionResponses;

        public ExceptionResponseHandler()
        {
            _exceptionResponses = new Dictionary<string, IHttpExceptionResponse>();
        }

        public bool HasResponse(string exceptionName)
        {
            return _exceptionResponses.ContainsKey(exceptionName);
        }

        public void RegisterResponse(IHttpExceptionResponse response)
        {
            _exceptionResponses.Add(response.ExceptionTypeName, response);
        }

        public void RegisterRepsonseRange(IEnumerable<IHttpExceptionResponse> responses)
        {
            foreach (var response in responses)
            {
                RegisterResponse(response);
            }
        }

        public IHttpResponse GetResponseFromException(Exception ex)
        {
            IHttpExceptionResponse response = null;
            if (_exceptionResponses.TryGetValue(ex.GetType().Name, out response) == false)
            {
                return new HttpResponse();
            }

            return response;
        }
    }
}
