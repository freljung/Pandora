using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Http
{
    public class HttpDefaultResponder : HttpCallResponderBase
    {
        public HttpDefaultResponder()
        {
            Active = true;
        }

        protected override bool WillRespond(HttpCall call)
        {
            return true;
        }

        // TODO: Must be first responder to be called.
        protected override IHttpResponse InternalRespond(HttpCall call)
        {
            if (_nextResponder == null)
                return new HttpNotImplementedResponse();

            return _nextResponder.Respond(call);
        }
    }
}
