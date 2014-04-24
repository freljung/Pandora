using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public abstract class HttpCallResponderBase : IHttpCallResponder
    {
        protected IHttpCallResponder _nextResponder;
        public bool Active { get; set; }

        public IHttpResponse Respond(HttpCall call)
        {
            if (WillRespond(call))
                return InternalRespond(call);

            return _nextResponder.Respond(call);
        }

        public void SetNextResponder(IHttpCallResponder nextResponder)
        {
            _nextResponder = nextResponder;
        }

        abstract protected bool WillRespond(HttpCall call);
        abstract protected IHttpResponse InternalRespond(HttpCall call);
    }

    public interface IHttpGetCallResponder : IHttpCallResponder
    { 
    }

    public interface IHttpCallResponder
    {
        bool Active { get; }
        IHttpResponse Respond(HttpCall call);
        void SetNextResponder(IHttpCallResponder nextResponder);
    }

    public abstract class HttpCallProcessor : IHttpCallProcessor
    {
        abstract public string Method { get; }
        abstract protected int _verbLength { get; }
        abstract protected int MaximumRequestSize { get; }
        abstract protected int TypicalRequestSize { get; }

        protected INetworkStreamWrapper _socketWrapper = null;
        protected IHttpHeaderParser _headerParser = null;
        protected IHttpCallResponder _firstCallResponder = null;

        public HttpCallProcessor(IHttpHeaderParser headerParser, IHttpCallResponder[] responders)
	    {
            _headerParser = headerParser;
            PrepareResponders(ref responders);
	    }

        virtual public IHttpResponse ProcessCall(INetworkStreamWrapper socketWrapper)
        {
            _socketWrapper = socketWrapper;
            _socketWrapper.SkipByteCount(_verbLength + 1);

            var httpCall = new HttpCall();
            httpCall.RequestHeader = GetHeaderFromSocket(socketWrapper);
            httpCall.RawRequestBody = GetRequestBodyFromSocket(socketWrapper);

            return _firstCallResponder.Respond(httpCall);
        }

        private HttpRequestHeader GetHeaderFromSocket(INetworkStreamWrapper socketWrapper)
        {
            var buffer = new byte[_headerParser.MaximumHeaderLength];
            _socketWrapper.Peek(buffer);
            var header = _headerParser.Parse(buffer, Method);

            _socketWrapper.SkipByteCount(header.HeaderLength);
            return header;
        }


        private void PrepareResponders(ref IHttpCallResponder[] responders)
        {
            //ArrayExtensions.Prepend(ref responders, new HttpDefaultResponder());

            var lastResponder = _firstCallResponder = new HttpDefaultResponder();
            foreach (var responder in responders.Where(r => r.Active))
            {
                if (responder.Active == false)
                    continue;

                if (lastResponder != null)
                    lastResponder.SetNextResponder(responder);

                lastResponder = responder;
            }
        }

        virtual protected byte[] GetRequestBodyFromSocket(INetworkStreamWrapper socketWrapper)
        {
            if (socketWrapper.DataAvailable == false)
                return new byte[0];

            var returnBuffer = new byte[TypicalRequestSize];
            int currentPosition = 0;
            while (socketWrapper.DataAvailable)
            {
                if (currentPosition > 0)
                    Array.Resize(ref returnBuffer, currentPosition + TypicalRequestSize);

                currentPosition += _socketWrapper.Receive(returnBuffer, currentPosition);
            }
            return returnBuffer;
        }
    }

    public static class ArrayExtensions
    {
        public static void Append<T>(ref T[] source, T entity)
        {
            Array.Resize(ref source, source.Length + 1);
            source[source.Length - 1] = entity;
        }

        public static void Prepend<T>(ref T[] source, T entity)
        {
            Array.Reverse(source);
            Append(ref source, entity);
            Array.Reverse(source);
        }
    }

}