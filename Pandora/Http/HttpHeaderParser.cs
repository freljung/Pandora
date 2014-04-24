using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Helpers;
using Pandora.Http.Exceptions;
using System.IO;

namespace Pandora.Http
{
    public class HttpHeaderParser : IHttpHeaderParser
    {
        public int MaximumHeaderLength { get { return 8192; } }
        public int MaximumUriLength { get { return 16384; } }
        private int _headerLength;

        public HttpRequestHeader Parse(byte[] buffer, string method)
        {
            VerifyHeaderLength(buffer);
            var bufferRows = PrepareBufferForParsing(buffer);
            var header = ParseRequestLine(method, bufferRows.First());

            foreach (var bufferRow in bufferRows.Skip(1))
            {
                var row = HeaderLine(bufferRow);
                header.AddRow(row);
            }

            return header;
        }

        private KeyValuePair<string, string> HeaderLine(byte[] bufferRow)
        {
            const byte colon = 0x3A;
            if (bufferRow.Contains(colon) == false)
                throw new BadRequestException("Incorrect header format");

            var keyvalue = bufferRow.ConvertISO_8859_1ToString().Split(':');
            var key = keyvalue.First();
            var value = keyvalue.Skip(1).First();
            return new KeyValuePair<string, string>(key, value);
        }


        // Go over the buffer and return a byte array per row
        private List<byte[]> PrepareBufferForParsing(byte[] buffer)
        {
            const short carriageReturn = 13;
            const short lineFeed = 10;

            var _list = new List<byte[]>();

            short current = short.MinValue;
            short last = short.MinValue;

            MemoryStream stream = null;
            try
            {
                int headerLength = 0;
                foreach (var b in buffer)
                {
                    if (stream == null)
                        stream = new MemoryStream(400);

                    last = current;
                    current = b;
                    stream.WriteByte(b);
                    headerLength++;
                    if (last == carriageReturn && current == lineFeed)
                    {
                        var headerRow = stream.ToArray();
                        // if this row is only CRLF then it's the end of the header
                        if (headerRow.Length == 2)
                            break;

                        _list.Add(headerRow);
                        stream.Dispose();
                        stream = null;
                    }
                }

                _headerLength = headerLength;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return _list;
        }

        private void VerifyHeaderLength(byte[] buffer)
        {
            var header = buffer.ConvertISO_8859_1ToString();

            if (header.Contains(Environment.NewLine) == false)
                throw new BadRequestException("No header");

            if (header.Contains(Environment.NewLine + Environment.NewLine) == false)
                throw new BadRequestException("Header to Big");
            // Return 414?
        }

        private HttpRequestHeader ParseRequestLine(string method, byte[] buffer)
        {
            var requestLine = buffer.Take(MaximumUriLength).ToArray().ConvertISO_8859_1ToString();
            if (requestLine.Contains(Environment.NewLine) == false)
                throw new BadRequestException("Uri to long");
            // Return 414?

            var uri = requestLine.Substring(0, requestLine.IndexOf(' '));
            var version = requestLine.Substring(uri.Length + 1, requestLine.IndexOf(Environment.NewLine));
            return new HttpRequestHeader() { Method = method, Uri = uri, Version = version, HeaderLength = _headerLength };
        }
    }
}
