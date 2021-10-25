using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using XmlManagement;

namespace Deserializer
{
    public static class ListExtention
    { 
        public static string ExtractData(this List<char> buff)
        {
            return string.Join("", buff.ToArray()).Substring((int)Defines.DATA_BEGIN);
        }
    }

    public class Deserializer
    {
        public static LoginRequest DeserializeLoginRequest(List<char> buff)
        {
            LoginRequest obj;
            Dictionary<string, string> parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }
        public static SignupRequest DeserializeSignupRequest(List<char> buff)
        {
            SignupRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }
        public static SignoutRequest DeserializeSignoutRequest(List<char> buff)
        {
            SignoutRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            return obj;
        }
        public static LoadChatRequest DeserializeLoadChatRequest(List<char> buff)
        {
            LoadChatRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.chatID = int.Parse(parsed["ChatID"]);
            obj.offset = int.Parse(parsed["Offset"]);
            return obj;
        }
        public static GetAllChatsRequest DeserializeGetAllChatsRequest(List<char> buff)
        {
            GetAllChatsRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.userID = int.Parse(parsed["UserID"]);
            return obj;
        }
        public static SendMessageRequest DeserializeSendMessageRequest(List<char> buff)
        {
            SendMessageRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.userID = int.Parse(parsed["UserID"]);
            obj.chatID = int.Parse(parsed["ChatID"]);
            obj.content = parsed["Content"];
            return obj;
        }
        public static CreateChatRequest DeserializeCreateChatRequest(List<char> buff)
        {
            CreateChatRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.chatName = parsed["ChatName"];
            obj.adminID = Convert.ToInt32(parsed["AdminID"]);
            return obj;
        }
        public static AddUserRequest DeserializeAddUserRequest(List<char> buff)
        {
            AddUserRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.chatID = Convert.ToInt32(parsed["ChatID"]);
            obj.nickname = parsed["Nickname"];
            return obj;
        }
    }
}
