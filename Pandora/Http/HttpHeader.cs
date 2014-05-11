using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpHeader
    {
        public string Version { get; set; }

        public Dictionary<string, string> Rows { get; private set; }

        public HttpHeader()
        {
            Rows = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddRow(KeyValuePair<string, string> headerRow)
        {
            Rows.Add(headerRow.Key.Trim(), headerRow.Value.Trim());
        }
    }
}