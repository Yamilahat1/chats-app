using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using Deserializer;
using Managers;
using Serializer;

namespace Handlers
{
    class MainHandler : IHandler
    {
        public MainHandler()
        {

        }
        public override bool Validation(RequestInfo req)
        {
            return req.id == (uint)Codes.SIGNOUT;
        }
        public override RequestResult HandleRequest(RequestInfo req)
        {
            RequestResult handler = new RequestResult();
            handler.newHandler = null;

            try
            {
                switch(req.id)
                {
                    case (uint)Codes.SIGNOUT:
                        return Signout(req);
                }
            }
            catch(Exception e)
            {
                handler.response = Serializer.Serializer.SerializeResponse(new ErrorResponse(e));
            }
            return handler;
        }
        private RequestResult Signout(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            SignoutResponse signoutRes;
            SignoutRequest signoutReq;
            res.newHandler = null;
            signoutReq = Deserializer.Deserializer.deserializeSignoutRequest(reqInfo.buffer);

            if (LoginManager.IsOnline(signoutReq.username))
            {
                signoutRes.status = 1;
                res.response = Serializer.Serializer.SerializeResponse(signoutRes);
                LoginManager.Signout(signoutReq.username);
                res.newHandler = new LoginHandler();
            }
            else
            {
                res.response = Serializer.Serializer.SerializeResponse(new ErrorResponse("User is not connected"));
            }

            return res;
        }
        private RequestResult SendMessage(RequestInfo reqInfo)
        {

        }
    }
}
