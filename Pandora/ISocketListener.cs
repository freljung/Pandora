using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora
{
    public interface ISocketListener
    {
        ServerSettings ServerSettings { get; set; }
        
        void Start();
        void Stop();
    }
}
