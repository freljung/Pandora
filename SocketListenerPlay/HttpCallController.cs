using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Pandora
{
    public interface IHttpCallController
    {
        void ProcessCall(ISocketWrapper socketWrapper);
        void RegisterCallProcessor(IHttpCallProcessor callProcessor);
    }

    public class HttpCallController : IHttpCallController
    {
        Dictionary<string, IHttpCallProcessor> _callProcessors;
        IExceptionResponseHandler _exceptionHandler;

        public HttpCallController(IExceptionResponseHandler exceptionHandler)
        {
            _callProcessors = new Dictionary<string, IHttpCallProcessor>();
            _exceptionHandler = exceptionHandler;
        }

        public void RegisterCallProcessor(IHttpCallProcessor callProcessor)
        {
            _callProcessors.Add(callProcessor.Verb, callProcessor);
        }

        public void ProcessCall(ISocketWrapper socketWrapper)
        {
            var result = socketWrapper.PeekToString(9);

            Exception caughtException = null;
            // TODO: Add exception handlers for custom exceptions and pass
            //       the exceptions to the respective return processor
            //       Ex: BadRequestException -> HttpBadRequestReturnController
            try
            {
                IHttpCallProcessor callProcessor = VerifyAndExtractProcessor(result);
                callProcessor.ProcessCall(socketWrapper);
            }
            catch (Exception ex)
            {
                if (_exceptionHandler.HasResponse(ex.GetType().Name) == false)
                    throw;                
                caughtException = ex;
            }

            IHttpResponse response = null;
            if (caughtException != null)
                response = _exceptionHandler.GetResponseFromException(caughtException);
        }

        private IHttpCallProcessor VerifyAndExtractProcessor(string result)
        {
            var splitString = result.Split(' ');
            var callType = splitString.First();
            if (splitString.Skip(1).FirstOrDefault() == null)
                // Return bad request?
                throw new BadRequestException("Incoming call malformated");

            IHttpCallProcessor callProcessor;
            if (_callProcessors.TryGetValue(callType, out callProcessor) == false)
                // Return Bad Request?
                throw new BadRequestException(string.Format("No IHttpCallProcessor for verb {0}", callType));
            return callProcessor;
        }
    }

    public interface IHttpCallProcessor
    {
        IHttpResponse ProcessCall(ISocketWrapper socketWrapper);
        string Verb { get; }
    }

    public abstract class HttpCallProcessor : IHttpCallProcessor
    {
        public string Uri { get; private set; }
        abstract public string Verb { get; }
        abstract protected int _verbLength { get; }
        abstract public int MaximumUriLength { get; }
        protected ISocketWrapper _socketWrapper = null;


        virtual public IHttpResponse ProcessCall(ISocketWrapper socketWrapper)
        {
            _socketWrapper = socketWrapper;
            
            Socket test;
            _socketWrapper.SkipByteCount(_verbLength + 1);
            
            var buffer = new byte[MaximumUriLength];
            _socketWrapper.Receive(buffer);

            var uri = buffer.ConvertISO_8859_1ToString();
            if (uri.Contains(Environment.NewLine) == false)
                throw new BadRequestException("Uri to long");
                // Return 414?

            Uri = uri.Substring(0, uri.IndexOf(' '));
            return null;
        }
    }

    public class HttpGetCallParser : HttpCallProcessor
    {
        public override string Verb
        {
            get { return "GET"; }
        }

        protected override int _verbLength { get { return 3; } }
        public override int MaximumUriLength { get { return 8192; } }
        
    }

    public interface IExceptionResponseHandler
    {
        void RegisterResponse(IHttpExceptionResponse response);
        void RegisterRepsonseRange(IEnumerable<IHttpExceptionResponse> responses);
        IHttpResponse GetResponseFromException(Exception ex);
        bool HasResponse(string exceptionName);
    }

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

    public interface IHttpExceptionResponse : IHttpResponse
    {
        string ExceptionTypeName { get; }
    }

    public interface IHttpResponse
    {
    }

    public class HttpResponse : IHttpResponse
    {
    }

    public class HttpBadRequestResponse<TException> : HttpExceptionResponseBase<TException>
        where TException : Exception
    {
    }


    public class HttpExceptionResponseBase<TException> : IHttpExceptionResponse
        where TException : Exception
    {
        public string ExceptionTypeName
        {
            get { return typeof(TException).Name; }
        }
    }
}