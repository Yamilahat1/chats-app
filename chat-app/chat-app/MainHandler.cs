﻿using System;
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
            return req.id == (uint)Codes.SIGNOUT || req.id == (uint)Codes.LOAD_CHAT || req.id == (uint)Codes.GET_ALL_CHATS || req.id == (uint)Codes.SEND_MESSAGE ;
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

                    case (uint)Codes.LOAD_CHAT:
                        return LoadChat(req);

                    case (uint)Codes.GET_ALL_CHATS:
                        return GetAllChats(req);

                    case (uint)Codes.SEND_MESSAGE:
                        return SendMessage(req);
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
            signoutReq = Deserializer.Deserializer.DeserializeSignoutRequest(reqInfo.buffer);

            signoutRes.status = 1;
            res.response = Serializer.Serializer.SerializeResponse(signoutRes);
            if (LoginManager.Signout(signoutReq.username))
            { 
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
            RequestResult res = new RequestResult();
            SendMessageRequest sendMessageRequest;
            SendMessageResponse sendMessageResponse;
            res.newHandler = null;

            sendMessageRequest = Deserializer.Deserializer.DeserializeSendMessageRequest(reqInfo.buffer);
            sendMessageResponse.status = ChatManager.SendMessage(new Message(sendMessageRequest.userID, sendMessageRequest.chatID, sendMessageRequest.content));
            res.response = Serializer.Serializer.SerializeResponse(sendMessageResponse);
            return res;
        }
        private RequestResult LoadChat(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            LoadChatResponse loadChatRes;
            LoadChatRequest loadChatReq;
            res.newHandler = null;
            loadChatReq = Deserializer.Deserializer.DeserializeLoadChatRequest(reqInfo.buffer);
            loadChatRes.msg = ChatManager.LoadMessage(loadChatReq.chatID, loadChatReq.offset);
            res.response = Serializer.Serializer.SerializeResponse(loadChatRes);
            return res;
        }
        private RequestResult GetAllChats(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            GetAllChatsResponse allChatsRes;
            GetAllChatsRequest allChatsReq;
            res.newHandler = null;
            allChatsReq = Deserializer.Deserializer.DeserializeGetAllChatsRequest(reqInfo.buffer);
            allChatsRes.chats = ChatManager.GetAllChats(allChatsReq.userID);
            res.response = Serializer.Serializer.SerializeResponse(allChatsRes);
            return res;
        }
    }
}
