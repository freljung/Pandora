using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace SocketListenerPlay
{
    public static class EncodedBufferExtensions
    {
        public static string ConvertEncodingToString(this byte[] source, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetString(source);
        }

        public static string ConvertISO_8859_1ToString(this byte[] source)
        {
            return source.ConvertEncodingToString("ISO-8859-1");
        }

        public static string ConvertUTF8ToString(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }
    }
}
