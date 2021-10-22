using System;
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
        public static int Login(string username, string password)
        {
            if (IsOnline(username)) return -1;
            var res = SqliteDatabase.LoginUser(username, password);
            int id = -1;
            if (res[0] == 1)
            {
                m_loggedUsers.Add(username, new User(username, password));
                id = res[1];
            }
            return id;
        }
        private static bool IsOnline(string username) => m_loggedUsers.ContainsKey(username);
        public static bool Signout(string username)
        {
            if (IsOnline(username)) m_loggedUsers.Remove(username);
            else return false;
            return true;
        }
    }
}
