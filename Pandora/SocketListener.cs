using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using Pandora.Http;

namespace Pandora
{
    public class SocketListener : ISocketListener
    {
        public IObservable<Socket> IncomingRequests { get; private set; }

        private IHttpCallController _callController;
        private TcpListener _listener;
        private int _listenPort = 81;
        private IPAddress _listenAddress;

        public SocketListener(IHttpCallController callController, IHttpCallProcessor[] callProcessors)
        {
            _listenAddress = Dns.GetHostEntry("localhost").AddressList[0];
            _listener = new TcpListener(localaddr: _listenAddress, port: _listenPort);
            _callController = callController;

            foreach (var callProcessor in callProcessors)
            {
                callController.RegisterCallProcessor(callProcessor);
            }
        }

        public void Start()
        {
            _listener.Start();

            IncomingRequests = _listener.GetContextsAsObservable().ObserveOn(NewThreadScheduler.Default);

            IncomingRequests.Subscribe(
                (s) =>
                {
                    using (var networkStreamWrapper = new NetworkStreamWrapper(s))
                    {
                        _callController.ProcessCall(networkStreamWrapper);
                    }
                },
                (ex) => { },
                () => { /* CancellationToken */ });
        }

        public void Stop()
        {
            try
            {
                _listener.Stop();
            }
            catch (Exception e)
            {
            }
        }
    }
}
