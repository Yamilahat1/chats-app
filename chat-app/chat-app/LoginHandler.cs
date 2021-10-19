﻿using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using Deserializer;
using Managers;
using Serializer;

namespace Handlers
{
    class LoginHandler : IHandler
    {
        public LoginHandler()
        {
        }
        public override bool Validation(RequestInfo req)
        {
            return req.id == (uint)Codes.LOGIN || req.id == (uint)Codes.SIGNUP;
        }
        public override RequestResult HandleRequest(RequestInfo req)
        {
            RequestResult handler = new RequestResult();
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
                Console.WriteLine("error in login handler");
                // handler.response = JsonResponsePacketSerializer::serializeResponse(ErrorResponse{ e });
            }
            return handler;
        }
        private RequestResult Login(RequestInfo reqInfo)
        {
            RequestResult res;
            LoginResponse loginRes;
            LoginRequest loginReq;

            loginReq = Deserializer.Deserializer.deserializeLoginRequest(reqInfo.buffer);
            // Todo: check if user is already logged


            return new RequestResult();
        }
        private RequestResult Signup(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            SignupResponse signupRes;
            SignupRequest signupReq;
            res.newHandler = null;
            signupReq = Deserializer.Deserializer.deserializeSignupRequest(reqInfo.buffer);
            if (LoginManager.Signup(signupReq.username, signupReq.password))
            {
                signupRes.status = 1;
                res.response = Serializer.Serializer.SerializeResponse(signupRes);
            }
            else
            {
                // Todo: add error response in case user exists
            }
            return res;
        }
    }
}
