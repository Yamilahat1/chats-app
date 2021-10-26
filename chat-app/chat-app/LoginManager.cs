using Server;
using System.Collections.Generic;

namespace Managers
{
    internal class User
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

        /// <summary>
        /// Method will manage a signup request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> If the signup was successful </returns>
        public static bool Signup(string username, string password)
        {
            if (!SqliteDatabase.DoesUserExist(username))
            {
                SqliteDatabase.AddUser(username, password);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method will manage a login request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> If the login was successful </returns>
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

        /// <summary>
        /// Method will check if a user is currently online
        /// </summary>
        /// <param name="username"> The user </param>
        /// <returns> If the user is online </returns>
        private static bool IsOnline(string username) => m_loggedUsers.ContainsKey(username);

        /// <summary>
        /// Method will manage a signout request
        /// </summary>
        /// <param name="username"> The user to signout </param>
        /// <returns> If the signout was successful </returns>
        public static bool Signout(string username)
        {
            if (IsOnline(username)) m_loggedUsers.Remove(username);
            else return false;
            return true;
        }
    }
}