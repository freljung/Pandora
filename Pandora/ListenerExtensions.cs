using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Net.Sockets;
using System.Reactive.Linq;

namespace Pandora
{
    internal static class ListenerExtensions
    {
        private static IEnumerable<IObservable<Socket>> Listen(this TcpListener listener)
        {
            while (true)
            {
                yield return listener.AcceptSocketAsync().ToObservable();
            }
        }
        internal static IObservable<Socket> GetContextsAsObservable(this TcpListener listener)
        {
            return listener.Listen().Concat();
        }
    }
}
