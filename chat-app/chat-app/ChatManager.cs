using Server;
using System.Collections.Generic;

namespace Managers
{
    public struct Message
    {
        public int senderID;
        public int chatID;
        public string content;

        public Message(int sender, int chat, string content)
        {
            senderID = sender;
            chatID = chat;
            this.content = content;
        }
    }

    internal class ChatManager
    {
        /// <summary>
        /// Method will call the sql query to send message
        /// </summary>
        /// <param name="msg"> The message </param>
        /// <returns> status </returns>
        public static int SendMessage(Message msg) => SqliteDatabase.SendMessage(msg.chatID, msg.senderID, msg.content);

        /// <summary>
        /// Method will call the sql query to load message
        /// </summary>
        /// <param name="chatID"> The id of the chat </param>
        /// <param name="offset"> DB offset </param>
        /// <returns></returns>
        public static Dictionary<string, string> LoadMessage(int chatID, int offset) => SqliteDatabase.LoadMessage(chatID, offset);

        /// <summary>
        /// Method will call the sql query to get chats of user
        /// </summary>
        /// <param name="userID"> The user </param>
        /// <returns> Dictionary of all the chats </returns>
        public static Dictionary<string, string> GetAllChats(int userID) => SqliteDatabase.GetAllChats(userID);

        /// <summary>
        /// Method will call the sql query to create a new chat
        /// </summary>
        /// <param name="chatName">  The new chat name </param>
        /// <param name="admin"> The id of the chat's admin </param>
        /// <returns> status </returns>
        public static int CreateChat(string chatName, int admin) => SqliteDatabase.CreateChat(chatName, admin);

        /// <summary>
        /// Method will call the sql query to add user to chat
        /// </summary>
        /// <param name="nickname"> The user's nickname </param>
        /// <param name="chatID"> The id of the chat </param>
        /// <returns></returns>
        public static int AddUserToChat(string tag, int chatID) => SqliteDatabase.AddUserToChat(chatID, tag);

        public static int RemoveUserFromChat(int senderID, string tag, int chatID)
        {
            if (SqliteDatabase.GetChatAdmin(chatID) != senderID) return 0;
            return SqliteDatabase.RemoveUserFromChat(chatID, tag);
        }
    }
}