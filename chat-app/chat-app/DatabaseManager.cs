using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

// Todo: add mutex
namespace Server
{
    internal static class SqliteDatabase
    {
        private static SQLiteCommand m_db;
        private static SQLiteConnection m_connection;

        /// <summary>
        /// Method will start the DB
        /// </summary>
        public static void InitDatabase()
        {
            string cs = @"URI=file:ChatsApp.db";
            m_connection = new SQLiteConnection(cs);
            m_connection.Open();

            m_db = new SQLiteCommand(m_connection);
            try
            {
                CreateTables();
                InitDefaults();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Method will create empty tables in case that the DB doesn't exist
        /// </summary>
        private static void CreateTables()
        {
            // Create user table
            m_db.CommandText = @"CREATE TABLE tUser(id INTEGER NOT NULL PRIMARY KEY, username TEXT, password TEXT, salt TEXT, nickname TEXT, status TEXT);";
            m_db.ExecuteNonQuery();

            // Create chat table
            m_db.CommandText = @"CREATE TABLE tChat(id INTEGER NOT NULL PRIMARY KEY, name TEXT, adminID INTEGER);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = @"CREATE TABLE tParticipants(id INTEGER NOT NULL PRIMARY KEY, userID INTEGER, roomID INTEGER);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = @"CREATE TABLE tMessage(id INTEGER NOT NULL PRIMARY KEY, roomID INTEGER, userID INTEGER, message TEXT);";
            m_db.ExecuteNonQuery();
        }

        /// <summary>
        /// Method will set default example values in the tables
        /// </summary>
        private static void InitDefaults()
        {
            // Example users
            AddUser("Bob", "Bob");
            AddUser("Alice", "Alice");

            // Example chat
            m_db.CommandText = "INSERT INTO tChat(name, adminID) VALUES ('cool chat', 1);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = "INSERT INTO tParticipants(userID, roomID) VALUES (1, 1);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = "INSERT INTO tParticipants(userID, roomID) VALUES (2, 1);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = "INSERT INTO tMessage(roomID, userID, message) VALUES (1, 1, 'hi alice');";
            m_db.ExecuteNonQuery();

            m_db.CommandText = "INSERT INTO tMessage(roomID, userID, message) VALUES (1, 2, 'hi bob');";
            m_db.ExecuteNonQuery();
        }

        /// <summary>
        /// Method will perform query execution
        /// </summary>
        /// <param name="query"> The query as a string </param>
        private static void Execute(string query)
        {
            m_db.CommandText = query;
            m_db.ExecuteNonQuery();
        }

        /// <summary>
        /// Method will check if user with such username already exists in the DB
        /// </summary>
        /// <param name="username"> The username </param>
        /// <returns> Status </returns>
        public static bool DoesUserExist(string username)
        {
            m_db.CommandText = string.Format("SELECT username FROM tUser");

            SQLiteDataReader reader = m_db.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    if (reader.GetValue(i).ToString().Equals(username))
                    {
                        reader.Close();
                        return true;
                    }
            }
            reader.Close();
            return false;
        }

        /// <summary>
        /// Method will sign a new user to the DB
        /// </summary>
        /// <param name="username"> The user's username </param>
        /// <param name="password"> The user's password </param>
        public static void AddUser(string username, string password)
        {
            string salt = Guid.NewGuid().ToString();
            password = HashString(password, salt);
            Execute(string.Format("INSERT INTO tUser(username, password, salt, nickname, status) VALUES ('{0}', '{1}', '{2}', '{0}', 'Amogus');", username, password, salt));
        }

        /// <summary>
        /// Method will perform hash function on a given password+salt
        /// </summary>
        /// <param name="text"> The user's password </param>
        /// <param name="salt"> Random salt: A random string which we attach to the password in order to hash them together </param>
        /// <returns> The hashed password </returns>
        private static string HashString(string text, string salt)
        {
            using (var sha = new SHA256Managed())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }

        /// <summary>
        /// Method will grab the user's salt from the DB
        /// </summary>
        /// <param name="username"> The username </param>
        /// <returns> The users salt </returns>
        public static string GetUserSalt(string username)
        {
            string salt;
            m_db.CommandText = string.Format("SELECT salt FROM tUser WHERE username='{0}';", username);
            SQLiteDataReader reader = m_db.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                throw new Exception("User not found");
            }

            reader.Read();
            salt = reader.GetValue(0).ToString();
            reader.Close();
            return salt;
        }

        /// <summary>
        /// Method will validate a login request
        /// </summary>
        /// <param name="username"> Username </param>
        /// <param name="password"> Password </param>
        /// <returns> Status </returns>
        public static List<int> LoginUser(string username, string password)
        {
            string hashedPassword = HashString(password, GetUserSalt(username));
            List<int> res = new List<int>();
            m_db.CommandText = string.Format("SELECT password, id FROM tUser WHERE username='{0}';", username);
            SQLiteDataReader reader = m_db.ExecuteReader();
            if (!reader.HasRows) throw new Exception("User not found");

            reader.Read();
            res.Add(Convert.ToInt32(reader.GetValue(0).ToString().Equals(hashedPassword)));
            if (res[0] == 1) res.Add(Convert.ToInt32(reader.GetValue(1).ToString()));
            reader.Close();
            return res;
        }

        /// <summary>
        /// Method will add a new message to the database
        /// </summary>
        /// <param name="roomID"> Where the message has been sent </param>
        /// <param name="senderID"> Who sent the message </param>
        /// <param name="msgContent"> The message </param>
        /// <returns></returns>
        public static int SendMessage(int roomID, int senderID, string msgContent)
        {
            try
            {
                Execute(string.Format("INSERT INTO tMessage(roomID, userID, message) VALUES ({0}, {1}, \"{2}\");", roomID.ToString(), senderID.ToString(), msgContent));
            }
            catch { return 0; }
            return 1;
        }

        /// <summary>
        /// Method will return the n message from a certain chat
        /// </summary>
        /// <param name="roomID"> The chat </param>
        /// <param name="offset"> Indicates which message to read </param>
        /// <returns> The message </returns>
        public static Dictionary<string, string> LoadMessage(int roomID, int offset = 0)
        {
            var msg = new Dictionary<string, string> { };
            m_db.CommandText = string.Format("SELECT userID, message, id FROM tMessage WHERE roomID={0} LIMIT 1 OFFSET {1}", roomID, offset);
            SQLiteDataReader reader = m_db.ExecuteReader();
            reader.Read();
            if (!reader.HasRows)
            {
                reader.Close();
                return new Dictionary<string, string> { };
            }
            int userID = Convert.ToInt32(reader.GetValue(0).ToString());
            string content = reader.GetValue(1).ToString();
            string msgID = reader.GetValue(2).ToString();
            reader.Close();

            msg.Add("Sender", GetNickname(userID));
            msg.Add("Content", content);
            msg.Add("MessageID", msgID);
            return msg;
        }

        /// <summary>
        /// Method will get user's nickname by his id
        /// </summary>
        /// <param name="id"> The user's id </param>
        /// <returns> The user's nickname </returns>
        public static string GetNickname(int id)
        {
            m_db.CommandText = string.Format("SELECT nickname FROM tUser WHERE id={0}", id);
            SQLiteDataReader reader = m_db.ExecuteReader();
            reader.Read();
            string nick = reader.GetValue(0).ToString();
            reader.Close();
            return nick;
        }

        /// <summary>
        /// Method will return all the chats of a user
        /// </summary>
        /// <param name="userID"> The user's id </param>
        /// <returns> Dictionary of all the chats names + id </returns>
        public static Dictionary<string, string> GetAllChats(int userID)
        {
            m_db.CommandText = string.Format("SELECT name, id FROM tChat WHERE id IN (SELECT roomID FROM tParticipants WHERE userID = {0});", userID);
            SQLiteDataReader reader = m_db.ExecuteReader();
            Dictionary<string, string> chats = new Dictionary<string, string>();
            try
            {
                while (reader.Read())
                {
                    chats.Add(reader.GetValue(1).ToString(), reader.GetValue(0).ToString());
                }
            }
            catch
            {
                reader.Close();
            }
            reader.Close();
            return chats;
        }

        /// <summary>
        /// Method will add a new chat to the database
        /// </summary>
        /// <param name="chatName"> The new chat name </param>
        /// <param name="admin"> The chat's admin </param>
        /// <returns> The new chat's id </returns>
        public static int CreateChat(string chatName, int admin)
        {
            Execute(string.Format("INSERT INTO tChat(name, adminID) VALUES (\"{0}\", {1});", chatName, admin.ToString()));

            m_db.CommandText = "SELECT last_insert_rowid();";
            Int64 id = (Int64)m_db.ExecuteScalar();
            int chatID = (int)id;
            AddUserToChat(chatID, GetNickname(admin));
            return chatID;
        }

        /// <summary>
        /// Method will check if user exists in a certain chat
        /// </summary>
        /// <param name="chatID"> The chat's id </param>
        /// <param name="userID"> The user's id </param>
        /// <returns> If the user exists or not </returns>
        public static bool IsUserInChat(int chatID, int userID)
        {
            m_db.CommandText = $"SELECT userID FROM tParticipants WHERE roomID = {chatID.ToString()}";
            SQLiteDataReader reader = m_db.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetValue(i).ToString().Equals(chatID.ToString()))
                    {
                        reader.Close();
                        return true;
                    }
                }
            }
            reader.Close();
            return false;
        }

        /// <summary>
        /// Method will add a user to a chat
        /// </summary>
        /// <param name="chatID"> The chat's id </param>
        /// <param name="nickname"> The nickname of the user </param>
        /// <returns> Status </returns>
        public static int AddUserToChat(int chatID, string nickname)
        {
            int userID = GetIDByNick(nickname);
            if (IsUserInChat(chatID, userID)) return 0;
            Execute(string.Format("INSERT INTO tParticipants(userID, roomID) VALUES ({0}, {1});", userID.ToString(), chatID.ToString()));
            return 1;
        }

        /// <summary>
        /// Method will find the ID of a user according to his nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        private static int GetIDByNick(string nickname)
        {
            m_db.CommandText = string.Format("SELECT id FROM tUser WHERE nickname=\"{0}\"", nickname);
            SQLiteDataReader reader = m_db.ExecuteReader();
            reader.Read();
            int id = Convert.ToInt32(reader.GetValue(0).ToString());
            reader.Close();
            return id;
        }

        /// <summary>
        /// Method will remove user from chat
        /// </summary>
        /// <param name="chatID"> The user's id </param>
        /// <param name="userID"> The chat's id </param>
        public static void RemoveUserFromChat(int chatID, int userID)
        {
            if (!IsUserInChat(chatID, userID)) return;
            Execute(string.Format("DELETE FROM tParticipants WHERE roomID = {0} AND userID = {1};", chatID.ToString(), userID.ToString()));
        }
    }
}