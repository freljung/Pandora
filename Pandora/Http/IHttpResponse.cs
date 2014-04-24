using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public interface IHttpResponse
    {
        HttpResponseHeader Header { get; }
        byte[] GetResponseAsByteArray();
    }
}
