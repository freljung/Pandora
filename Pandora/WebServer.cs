using Pandora.Http;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora
{
    public class WebServer
    {
        internal ISocketListener _listener;

        internal static void RegisterInstances()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AddAllTypesOf<IHttpCallProcessor>();
                    scan.AddAllTypesOf<IHttpGetCallResponder>();
                });

                x.For<ISocketListener>().Use<SocketListener>();
            });
        }

        private void CreateListener()
        {
            if (_listener != null)
                throw new Exception("Only one SocketListener is currently supported.");

            _listener = ObjectFactory.GetInstance<ISocketListener>();
        }

        public void Setup()
        {
            RegisterInstances();

            CreateListener();
        }

        public void Start()
        {
            if (_listener == null)
                throw new Exception("Must run Setup() on server before Start()");

            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}
