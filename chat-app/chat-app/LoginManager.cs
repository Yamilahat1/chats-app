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
    public class LoginManager : SqliteDatabase
    {
        private static List<User> m_loggedUsers = new List<User>();

        public static bool Signup(string username, string password)
        {
            if (!SqliteDatabase.DoesUserExist(username))
            {
                SqliteDatabase.AddUser(username, password);
                return true;
            }
            return false;
        }
    }
}
