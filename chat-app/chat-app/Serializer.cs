using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using XmlManagement;
using Server;

namespace Serializer
{
    class Serializer
    {
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
        private static List<char> Serialize(string root, Dictionary<string, string> dict, Codes code)
        {
            return AddHeaders(XmlManagement.XmlManagement.Serialization(root, dict), (int)code);
        }
        public static List<char> SerializeResponse(SignupResponse res)
        {
            return Serialize("SignupResponse", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.SIGNUP);
        }
        public static List<char> SerializeResponse(SignoutResponse res)
        {
            return Serialize("SignoutResponse", new Dictionary<string, string> { { "Status", res.status.ToString() } }, Codes.SIGNOUT);
        }
        public static List<char> SerializeResponse(ErrorResponse res)
        {
            return Serialize("Error", new Dictionary<string, string> { { "Message", res.msg } }, Codes.ERROR);
        }
        public static List<char> SerializeResponse(LoginResponse res)
        {
            return Serialize("Login", new Dictionary<string, string> { { "Status", res.status.ToString() }, {"id", res.id.ToString() } }, Codes.LOGIN);
        }
        public static List<char> SerializeResponse(LoadChatResponse res)
        {
            Dictionary<string, string> messages = new Dictionary<string, string> { };
            foreach(var msg in res.chatLog) messages.Add(SqliteDatabase.GetNickname(msg.senderID), msg.content);
            return Serialize("Messages", messages, Codes.LOAD_CHAT);
        }
        public static List<char> SerializeResponse(GetAllChatsResponse res)
        {
            string chats = "";
            foreach(var chat in res.chats)
            {
                chats += chat.Key + "-" + chat.Value + ",";
            }
            return Serialize("Chats", new Dictionary<string, string> { { "Chats", chats } }, Codes.GET_ALL_CHATS);
        }
    }
}
