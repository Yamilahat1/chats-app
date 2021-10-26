using System;
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

        /// <summary>
        /// Method will check if the request matches the current handler according to the ID
        /// </summary>
        /// <param name="req"> The request </param>
        /// <returns> If the request is valid or not </returns>
        public override bool Validation(RequestInfo req)
        {
            return req.id == (uint)Codes.LOGIN || req.id == (uint)Codes.SIGNUP;
        }

        /// <summary>
        /// Method will handle the request by redirecting it to the matching method
        /// </summary>
        /// <param name="req"> The request </param>
        /// <returns> The server's response </returns>
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
                handler.response = Serializer.Serializer.SerializeResponse(new ErrorResponse(e));
            }
            return handler;
        }

        /// <summary>
        /// Method will handle a login request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The result of the login request </returns>
        private RequestResult Login(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            LoginResponse loginRes;
            LoginRequest loginReq;
            res.newHandler = null;
            loginReq = Deserializer.Deserializer.DeserializeLoginRequest(reqInfo.buffer); // Deserializing the request so that we can access the info easily
            int id = LoginManager.Login(loginReq.username, loginReq.password); // Performing login in the database
            if (id != -1) // Means the login went fine
            {
                loginRes.status = 1;
                loginRes.id = id;
                res.response = Serializer.Serializer.SerializeResponse(loginRes); // Serializing the response so we can send it back to the client
                res.newHandler = new MainHandler(); // Give the client a new handler, since it has passed the login phase
            }
            else
            {
                res.response = Serializer.Serializer.SerializeResponse(new ErrorResponse("Login failed")); // Serialize an error response
            }
            return res;
        }

        /// <summary>
        /// Method will handle a signup request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
        private RequestResult Signup(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            SignupResponse signupRes;
            SignupRequest signupReq;
            res.newHandler = null;
            signupReq = Deserializer.Deserializer.DeserializeSignupRequest(reqInfo.buffer);
            if (LoginManager.Signup(signupReq.username, signupReq.password))
            {
                signupRes.status = 1;
                res.response = Serializer.Serializer.SerializeResponse(signupRes);
            }
            else
            {
                res.response = Serializer.Serializer.SerializeResponse(new ErrorResponse("User already exists!"));
            }
            return res;
        }
    }
}
