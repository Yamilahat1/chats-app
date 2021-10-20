﻿using System;
using System.Collections.Generic;
using System.Text;
using Server;

namespace Managers
{
    class User
    {
        private string username;
        private string password;
        private string status;

        public User(string username, string password, string status = "Amogus")
        {
            this.username = username;
            this.password = password;
            this.status = status;
        }
    }
    public class LoginManager
    {
        private static Dictionary<string, User> m_loggedUsers = new Dictionary<string, User>();

        public static bool Signup(string username, string password)
        {
            if (!SqliteDatabase.DoesUserExist(username))
            {
                SqliteDatabase.AddUser(username, password);
                return true;
            }
            return false;
        }
        public static bool Login(string username, string password)
        {
            if (SqliteDatabase.LoginUser(username, password))
            {
                m_loggedUsers.Add(username, new User(username, password));
                return true;
            }
            return false;
        }
        public static bool IsOnline(string username)
        {
            return m_loggedUsers.ContainsKey(username);
        }
    }
}
