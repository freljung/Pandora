using Pandora.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora
{
    public interface IHttpCallController
    {
        void ProcessCall(INetworkStreamWrapper socketWrapper);
        void RegisterCallProcessor(IHttpCallProcessor callProcessor);
    }
}
