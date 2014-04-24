using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Helpers;
using Pandora.Http.Exceptions;

namespace Pandora.Http
{
    public class HttpCallController : IHttpCallController
    {
        Dictionary<string, IHttpCallProcessor> _callProcessors;
        IExceptionResponseHandler _exceptionHandler;

        public IDictionary<string, IHttpCallProcessor> CallProcessors
        {
            get
            {
                return _callProcessors;
            }
        }

        public HttpCallController(IExceptionResponseHandler exceptionHandler)
        {
            _callProcessors = new Dictionary<string, IHttpCallProcessor>();
            _exceptionHandler = exceptionHandler;
        }

        public void RegisterCallProcessor(IHttpCallProcessor callProcessor)
        {
            _callProcessors.Add(callProcessor.Method, callProcessor);
        }

        public void ProcessCall(INetworkStreamWrapper socketWrapper)
        {
            var result = socketWrapper.PeekToString(9);

            Exception caughtException = null;
            IHttpResponse response = null;
            try
            {
                IHttpCallProcessor callProcessor = VerifyAndExtractProcessor(result);
                response = callProcessor.ProcessCall(socketWrapper);
            }
            catch (Exception ex)
            {
                if (_exceptionHandler.HasResponse(ex.GetType().Name) == false)
                    throw;                
                response = _exceptionHandler.GetResponseFromException(caughtException);
            }

            SendResponse(response, socketWrapper);
        }

        private void SendResponse(IHttpResponse response, INetworkStreamWrapper socketWrapper)
        {
            socketWrapper.Send(response.GetResponseAsByteArray());
        }

        private IHttpCallProcessor VerifyAndExtractProcessor(string result)
        {
            var splitString = result.Split(' ');
            var callType = splitString.First();
            if (splitString.Skip(1).FirstOrDefault() == null)
                throw new BadRequestException("Incoming call malformated");

            IHttpCallProcessor callProcessor;
            if (_callProcessors.TryGetValue(callType, out callProcessor) == false)
                throw new BadRequestException(string.Format("No IHttpCallProcessor for verb {0}", callType));
            return callProcessor;
        }
    }
}