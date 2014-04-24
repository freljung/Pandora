using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace SocketListenerPlay
{
    public class SocketListener
    {
        public IObservable<Socket> IncomingRequests { get; private set; }

        private IHttpCallController _callController;
        private TcpListener _listener;
        private int _listenPort = 81;
        private IPAddress _listenAddress;

        public SocketListener(IHttpCallController callController)
        {
            _listenAddress = Dns.GetHostEntry("localhost").AddressList[0];
            _listener = new TcpListener(localaddr: _listenAddress, port: _listenPort);
            _callController = callController;

            // TODO: Move registration external from this class. Perhaps via MEF?
            callController.RegisterCallProcessor(new HttpGetCallParser());
        }

        public void Start()
        {
            _listener.Start();

            IncomingRequests = _listener.GetContextsAsObservable().ObserveOn(NewThreadScheduler.Default);

            IncomingRequests.Subscribe(
                (s) =>
                {
                    using (var socketWrapper = new SocketWrapper(s))
                    {
                        _callController.ProcessCall(socketWrapper);
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
