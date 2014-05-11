using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pandora.Helpers;

namespace Pandora
{
    public static class StreamExtensions
    {
        public static void WriteLine(this Stream source, string str)
        {
            var buffer = string.Concat(str, Environment.NewLine).ConvertStringToISO_8859_1ByteArray();
            source.Write(buffer, 0, buffer.Length);
        }

        public static void WriteRows(this Stream source, Dictionary<string, string> rows)
        {
            foreach (var row in rows)
            {
                var buffer = string.Concat(row.Key, ":", row.Value, Environment.NewLine).ConvertStringToISO_8859_1ByteArray();
                source.Write(buffer, 0, buffer.Length);
            }
        }
    }
}