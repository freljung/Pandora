using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpGetCallParser : HttpCallProcessor
    {
        public override string Method
        {
            get { return "GET"; }
        }

        protected override int MaximumRequestSize
        {
            get { return 16184; } // TODO: Move to setting
        }

        protected override int TypicalRequestSize 
        {
            get { return 4096;  } // TODO: Mostly used for default buffer sizes. Better logic?
        }

        protected override int _verbLength { get { return 3; } }

        public HttpGetCallParser(IHttpHeaderParser headerParser, IHttpGetCallResponder[] getCallResponders)
            : base(headerParser, getCallResponders)
        {

        }
    }
}
