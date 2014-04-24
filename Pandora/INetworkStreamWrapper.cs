using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Pandora
{
    public interface INetworkStreamWrapper
    {
        Socket Socket { get; }
        NetworkStream Stream { get; }
        bool DataAvailable { get; }

        int Receive(byte[] buffer);
        int Receive(byte[] buffer, int offset);
        int Peek(byte[] buffer);

        void Send(Byte[] buffer);
        bool SkipByteCount(int count);

        /// <summary>
        /// Assumes it's reading ISO-8859-1 encoded text
        /// </summary>
        /// <param name="stringLength">How much of the buffer should be peeked</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        string PeekToString(int bufferLength);

        /// <summary>
        /// Assumes it's reading ISO-8859-1 encoded text
        /// </summary>
        /// <param name="bufferLength">How much of the buffer should be peeked</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns></returns>
        string PeekToString(int bufferLength, string encoding);
    }
}
