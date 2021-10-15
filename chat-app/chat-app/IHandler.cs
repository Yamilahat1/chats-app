using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Handlers
{
    public class IHandler
    {
        // Check if the request is valid
        public virtual bool Validation(RequestInfo req)
        {
            return true;
        }
        
        // Handle a new request
        public virtual RequestResult HandleRequest(RequestInfo req)
        {
            return new RequestResult();
        }
    }
}