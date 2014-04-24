using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace SocketListenerPlay
{
    public sealed class SocketWrapper : IDisposable, ISocketWrapper
    {
        public Socket Socket { get; private set; }

        public SocketWrapper(Socket wrappedSocket)
        {
            Socket = wrappedSocket;
        }

        public int Receive(byte[] buffer)
        {
            return Socket.Receive(buffer);

        }

        public int Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return Socket.Receive(buffer, socketFlags);
        }

        public void Dispose()
        {
            try
            {
                Socket.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                Socket.Dispose();
            }
        }

        public bool SkipByteCount(int count)
        {
            var buffer = new byte[count];

            try
            {
                if (Receive(buffer) == count)
                    return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// Assumes it's reading ISO-8859-1 encoded text
        /// </summary>
        /// <param name="bufferLength">How much of the buffer should be peeked</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns></returns>
        public string PeekToString(int bufferLength, string encoding)
        {
            var buffer = new byte[bufferLength];
            int read = Receive(buffer, SocketFlags.Peek);
            return buffer.ConvertEncodingToString(encoding);
        }

        /// <summary>
        /// Assumes it's reading ISO-8859-1 encoded text
        /// </summary>
        /// <param name="stringLength">How much of the buffer should be peeked</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string PeekToString(int bufferLength)
        {
            var buffer = new byte[bufferLength];
            int read = Receive(buffer, SocketFlags.Peek);
            return buffer.ConvertISO_8859_1ToString();
        }
    }
}
