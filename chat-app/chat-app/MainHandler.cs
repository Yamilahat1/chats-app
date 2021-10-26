using Managers;
using System;
using Utilities;

namespace Handlers
{
    internal class MainHandler : IHandler
    {
        /// <summary>
        /// Method will check if the request is valid in the current context according to the id
        /// </summary>
        /// <param name="req"> The reuqest </param>
        /// <returns> If the request is valid or not </returns>
        public override bool Validation(RequestInfo req)
        {
            return (uint)Codes.SIGNOUT <= req.id && req.id <= (uint)Codes.ADD_USER;
        }

        /// <summary>
        /// Method will handle request by redirecting it to the matching method
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
                    case (uint)Codes.SIGNOUT:
                        return Signout(req);

                    case (uint)Codes.LOAD_CHAT:
                        return LoadChat(req);

                    case (uint)Codes.GET_ALL_CHATS:
                        return GetAllChats(req);

                    case (uint)Codes.SEND_MESSAGE:
                        return SendMessage(req);

                    case (uint)Codes.CREATE_CHAT:
                        return CreateChat(req);

                    case (uint)Codes.ADD_USER:
                        return AddUserToChat(req);
                }
            }
            catch (Exception e)
            {
                handler.response = Serializer.Serializer.SerializeResponse(new ErrorResponse(e));
            }
            return handler;
        }

        /// <summary>
        /// Method will handle a signout request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
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

        /// <summary>
        /// Method will handle a SendMessage request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
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

        /// <summary>
        /// Method will handle a LoadChat request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
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

        /// <summary>
        /// Method will handle a GetAllChats request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
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

        /// <summary>
        /// Method will handle a CreateChat request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
        private RequestResult CreateChat(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            CreateChatResponse createChatRes;
            CreateChatRequest createChatReq;
            res.newHandler = null;
            createChatReq = Deserializer.Deserializer.DeserializeCreateChatRequest(reqInfo.buffer);
            createChatRes.chatID = ChatManager.CreateChat(createChatReq.chatName, createChatReq.adminID);
            res.response = Serializer.Serializer.SerializeResponse(createChatRes);
            return res;
        }

        /// <summary>
        /// Method will handle an AddUserToChat request
        /// </summary>
        /// <param name="reqInfo"> The request </param>
        /// <returns> The server's response </returns>
        private RequestResult AddUserToChat(RequestInfo reqInfo)
        {
            RequestResult res = new RequestResult();
            AddUserResponse addUserRes;
            AddUserRequest addUserReq;
            res.newHandler = null;
            addUserReq = Deserializer.Deserializer.DeserializeAddUserRequest(reqInfo.buffer);
            addUserRes.status = ChatManager.AddUserToChat(addUserReq.nickname, addUserReq.chatID);
            res.response = Serializer.Serializer.SerializeResponse(addUserRes);
            return res;
        }
    }
}