using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Server;

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
    class ChatManager
    {
        public static void SendMessage(Message msg)
        {
            SqliteDatabase.SendMessage(msg.chatID, msg.senderID, msg.content);
        }
        public static List<Message> LoadMessages(int chatID) => SqliteDatabase.LoadMessages(chatID);
        public static Dictionary<string, string> GetAllChats(int userID) => SqliteDatabase.GetAllChats(userID);
    }
}
