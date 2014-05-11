using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public interface IHttpCallProcessor
    {
        string Method { get; }
        ServerSettings ServerSettings { get; set; }
        IHttpResponse ProcessCall(INetworkStreamWrapper socketWrapper);
    }
}
