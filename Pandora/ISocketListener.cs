using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora
{
    public interface ISocketListener
    {
        void Start();
        void Stop();
    }
}
