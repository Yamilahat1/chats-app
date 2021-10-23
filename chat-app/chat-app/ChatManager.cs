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
        public static int SendMessage(Message msg)
        {
            return SqliteDatabase.SendMessage(msg.chatID, msg.senderID, msg.content);
        }
        public static Dictionary<string, string> LoadMessage(int chatID, int offset) => SqliteDatabase.LoadMessage(chatID, offset);
        public static Dictionary<string, string> GetAllChats(int userID) => SqliteDatabase.GetAllChats(userID);
    }
}
