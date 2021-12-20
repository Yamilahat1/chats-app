using System;
using System.Collections.Generic;

namespace Utilities
{
    public struct GetChatDetailsRequest
    {
        public int chatID;
    }
    public struct GetChatDetailsResponse
    {
        public string chatName;
        public string users;
    }
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
        public int offset;
    }

    public struct LoadChatResponse
    {
        public Dictionary<string, string> msg; // sender/content
    }

    public struct GetAllChatsRequest
    {
        public int userID;
    }

    public struct GetAllChatsResponse
    {
        public Dictionary<string, string> chats;
    }

    public struct AddUserRequest
    {
        public string Tag;
        public int chatID;
    }

    public struct AddUserResponse
    {
        public int status;
    }
    public struct RemoveUserRequest
    {
        public string tag;
        public int chatID;
        public int senderID;
    }
    public struct RemoveUserResponse
    {
        public int status;
    }
    public struct CreateChatRequest
    {
        public string chatName;
        public int adminID;
    }

    public struct CreateChatResponse
    {
        public int chatID;
    }

    public struct SendMessageRequest
    {
        public int userID;
        public int chatID;
        public string content;
    }

    public struct SendMessageResponse
    {
        public int status;
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
        ERROR, LOGIN, SIGNUP, SIGNOUT, LOAD_CHAT, GET_ALL_CHATS, SEND_MESSAGE, CREATE_CHAT, ADD_USER, REMOVE_USER, GET_CHAT_DETAILS
    }

    public enum Defines
    {
        LEN_END = 5,
        DATA_BEGIN = LEN_END,
        LEN_BEGIN = 1,
        MSG_CODE = 0
    }
}