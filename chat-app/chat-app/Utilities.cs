using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Managers;

namespace Utilities
{
    public struct SignoutRequest
    {
        public string username;
    }
    public struct SignoutResponse
    {
        public uint status;
    }
    public struct LoadChatRequest
    {
        public int chatID;
    }
    public struct LoadChatResponse
    {
        public List<Message> chatLog;
    }
    public struct GetAllChatsRequest
    {
        public int userID;
    }
    public struct GetAllChatsResponse
    {
        public Dictionary<string, string> chats;
    }
    public struct RequestInfo
    {
        public uint id;
        public List<char> buffer;
    }

    public struct RequestResult
    {
        public List<char> response;
        public Handlers.IHandler newHandler;
    }

    public struct LoginResponse
    {
        public uint status;
        public int id;
    }

    public struct SignupResponse
    {
        public uint status;
    }

    public struct LoginRequest
    {
        public string username;
        public string password;
    }

    public struct SignupRequest
    {
        public string username;
        public string password;
    }
    public struct ErrorResponse
    {
        public string msg;

        public ErrorResponse(string e) : this()
        {
            this.msg = e;
        }
        public ErrorResponse(Exception e) : this()
        {
            this.msg = e.Message;
        }
    }
    public enum Codes
    {
        ERROR, LOGIN, SIGNUP, SIGNOUT, LOAD_CHAT, GET_ALL_CHATS, 
    }
    public enum Defines
    {
        LEN_END = 5,
        DATA_BEGIN = LEN_END,
        LEN_BEGIN = 1,
        MSG_CODE = 0
    }
}
