using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Pandora.Helpers;

namespace Pandora
{
    public sealed class NetworkStreamWrapper : IDisposable, INetworkStreamWrapper
    {
        public Socket Socket { get; private set; }
        public NetworkStream Stream { get; private set; }

        public bool DataAvailable
        {
            get
            {
                return Stream.DataAvailable;
            }
        }

        public NetworkStreamWrapper(Socket wrappedSocket)
        {
            Socket = wrappedSocket;
            Stream = new NetworkStream(Socket, true);
        }

        public int Receive(byte[] buffer)
        {
            return Stream.Read(buffer, 0, buffer.Length);
        }

        public int Receive(byte[] buffer, int offset)
        {
            return Stream.Read(buffer, offset, buffer.Length - offset);
        }

        public int Peek(byte[] buffer)
        {
            return Socket.Receive(buffer, SocketFlags.Peek);
        }

        public void Send(Byte[] buffer)
        {
            Stream.Write(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
            try
            {
                //Stream.Close();
                //Socket.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                //Stream.Dispose();
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
            int read = Peek(buffer);
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
            int read = Peek(buffer);
            return buffer.ConvertISO_8859_1ToString();
        }
    }
}
