using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Handlers
{
    class LoginHandler : IHandler
    {
        public override bool Validation(RequestInfo req)
        {
            return req.id == (uint)Codes.LOGIN || req.id == (uint)Codes.SIGNUP;
        }
        public override RequestResult HandleRequest(RequestInfo req)
        {
            RequestResult handler;
            handler.newHandler = null;

            try
            {
                switch (req.id)
                {
                    case (uint)Codes.LOGIN:
                        return Login(req);

                    case (uint)Codes.SIGNUP:
                        return Signup(req);

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                // handler.response = JsonResponsePacketSerializer::serializeResponse(ErrorResponse{ e });
            }
            return handler;
        }
        private RequestResult Login(RequestInfo reqInfo)
        {
            RequestResult res;
            LoginResponse loginRes;
            LoginRequest loginReq;

            try
            {
                loginReq = 
            }

            return new RequestResult();
        }
        private RequestResult Signup(RequestInfo reqInfo)
        {
            return new RequestResult();
        }
    }
}
