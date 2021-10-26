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
        /// <summary>
        /// This extention of the list data structor allows us to use custom method
        /// which helps in extracting data from message (ignoring the headers section)
        /// </summary>
        /// <param name="buff"> A char buffer </param>
        /// <returns> The data segment </returns>
        public static string ExtractData(this List<char> buff)
        {
            return string.Join("", buff.ToArray()).Substring((int)Defines.DATA_BEGIN);
        }
    }

    public class Deserializer
    {
        /// <summary>
        /// Method will deserialize a login request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized login request </returns>
        public static LoginRequest DeserializeLoginRequest(List<char> buff)
        {
            LoginRequest obj;
            Dictionary<string, string> parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }

        /// <summary>
        /// Method will deserialize a signup request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized signup request</returns>
        public static SignupRequest DeserializeSignupRequest(List<char> buff)
        {
            SignupRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }

        /// <summary>
        /// Method will deserialize a signout request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized signout request</returns>
        public static SignoutRequest DeserializeSignoutRequest(List<char> buff)
        {
            SignoutRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            return obj;
        }

        /// <summary>
        /// Method will deserialize a LoadChat request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized LoadChat request</returns>
        public static LoadChatRequest DeserializeLoadChatRequest(List<char> buff)
        {
            LoadChatRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.chatID = int.Parse(parsed["ChatID"]);
            obj.offset = int.Parse(parsed["Offset"]);
            return obj;
        }

        /// <summary>
        /// Method will deserialize a GetAllChat request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized GetAllChats request</returns>
        public static GetAllChatsRequest DeserializeGetAllChatsRequest(List<char> buff)
        {
            GetAllChatsRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.userID = int.Parse(parsed["UserID"]);
            return obj;
        }

        /// <summary>
        /// Method will deserialize a SendMessage request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized SendMessage request</returns>
        public static SendMessageRequest DeserializeSendMessageRequest(List<char> buff)
        {
            SendMessageRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.userID = int.Parse(parsed["UserID"]);
            obj.chatID = int.Parse(parsed["ChatID"]);
            obj.content = parsed["Content"];
            return obj;
        }

        /// <summary>
        /// Method will deserialize a CreateChat request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized CreateChat request</returns>
        public static CreateChatRequest DeserializeCreateChatRequest(List<char> buff)
        {
            CreateChatRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.chatName = parsed["ChatName"];
            obj.adminID = Convert.ToInt32(parsed["AdminID"]);
            return obj;
        }

        /// <summary>
        /// Method will deserialize an AddUser request
        /// </summary>
        /// <param name="buff"> The request buffer </param>
        /// <returns> Deserialized AddUser request</returns>
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
