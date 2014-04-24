using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public interface IHttpHeaderParser
    {
        int MaximumHeaderLength { get; }
        int MaximumUriLength { get; }

        HttpRequestHeader Parse(byte[] buffer, string method);
    }
}
