using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public interface IHttpCallProcessor
    {
        string Method { get; }
        IHttpResponse ProcessCall(INetworkStreamWrapper socketWrapper);
    }
}
