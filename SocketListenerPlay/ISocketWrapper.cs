using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace SocketListenerPlay
{
    public interface ISocketWrapper
    {
        Socket Socket { get; }

        int Receive(byte[] buffer);
        int Receive(byte[] buffer, SocketFlags socketFlags);

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
