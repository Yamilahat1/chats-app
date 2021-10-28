using System;
using System.Collections.Generic;
using Utilities;

namespace Serializer
{
    internal class Serializer
    {
        /// <summary>
        /// Method will add the necessary headers to the response
        /// </summary>
        /// <param name="msg"> The response data </param>
        /// <param name="code"> The response code </param>
        /// <returns> Buffer which represents the server's reponse </returns>
        private static List<char> AddHeaders(string msg, int code)
        {
            List<char> buff = new List<char>();
            string length = msg.Length.ToString();
            buff.Add(Convert.ToChar(code));
            for (int i = 0; i < 4 - length.Length; i++) buff.Add('0');
            for (int i = 0; i < length.Length; i++) buff.Add(length[i]);
            for (int i = 0; i < msg.Length; i++) buff.Add(msg[i]);
            return buff;
        }

        /// <summary>
        /// Method will serialize a message to an XML format
        /// </summary>
        /// <param name="root"> The root tag </param>
        /// <param name="dict"> Dictionary will be used in order to write the other tags </param>
        /// <param name="code"> The response code </param>
        /// <returns> Buffer which represents the response </returns>
        private static List<char> Serialize(string root, Dictionary<string, string> dict, Codes code)
        {
            return AddHeaders(XmlManagement.XmlManagement.Serialization(root, dict), (int)code);
        }

        /// <summary>
        /// Methyod will serialize a signup response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(SignupResponse res)
        {
            return Serialize("SignupResponse", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.SIGNUP);
        }

        /// <summary>
        /// Methyod will serialize a signout response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(SignoutResponse res)
        {
            return Serialize("SignoutResponse", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.SIGNOUT);
        }

        /// <summary>
        /// Methyod will serialize an error response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(ErrorResponse res)
        {
            return Serialize("Error", new Dictionary<string, string> { { "Message", res.msg } }, Codes.ERROR);
        }

        /// <summary>
        /// Methyod will serialize a login response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(LoginResponse res)
        {
            return Serialize("Login", new Dictionary<string, string> { { "Status", res.status.ToString() }, { "id", res.id.ToString() } }, Codes.LOGIN);
        }

        /// <summary>
        /// Methyod will serialize a LoadChat response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(LoadChatResponse res)
        {
            return Serialize("Messages", res.msg, Codes.LOAD_CHAT);
        }

        /// <summary>
        /// Methyod will serialize a GetAllChats response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(GetAllChatsResponse res)
        {
            string chats = "";
            foreach (var chat in res.chats)
            {
                chats += chat.Value + "-" + chat.Key + ",";
            }
            return Serialize("Chats", new Dictionary<string, string> { { "Chats", chats } }, Codes.GET_ALL_CHATS);
        }

        /// <summary>
        /// Methyod will serialize a SendMessage response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(SendMessageResponse res)
        {
            return Serialize("SendMessage", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.SEND_MESSAGE);
        }

        /// <summary>
        /// Methyod will serialize a CreateChat response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(CreateChatResponse res)
        {
            return Serialize("CreateChat", new Dictionary<string, string> { { "ChatID", res.chatID.ToString() } }, Codes.CREATE_CHAT);
        }

        /// <summary>
        /// Methyod will serialize an AddUser response
        /// </summary>
        /// <param name="res"> The response </param>
        /// <returns> A buffer which represents the reponse </returns>
        public static List<char> SerializeResponse(AddUserResponse res)
        {
            return Serialize("AddUser", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.CREATE_CHAT);
        }

        public static List<char> SerializeResponse(RemoveUserResponse res)
        {
            return Serialize("RemoveUser", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.REMOVE_USER);
        }

        public static List<char> SerializeResponse(GetChatDetailsResponse res)
        {
            return Serialize("ChatDetails", new Dictionary<string, string> { { "ChatName", res.chatName }, { "Participants", res.users } }, Codes.GET_CHAT_DETAILS);
        }
    }
}