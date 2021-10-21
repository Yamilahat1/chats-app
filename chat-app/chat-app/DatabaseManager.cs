using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Managers;
// Todo: add mutex
namespace Server
{
    static class SqliteDatabase
    {
        private static SQLiteCommand m_db;
        private static SQLiteConnection m_connection;
        public static void InitDatabase()
        {
            string cs = @"URI=file:D:\test.db";
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
        static void CreateTables()
        {
            // Create user table
            m_db.CommandText = @"CREATE TABLE tUser(id INTEGER NOT NULL PRIMARY KEY, username TEXT, password TEXT, salt TEXT, nickname TEXT, status TEXT);";
            m_db.ExecuteNonQuery();

            // Create chat table
            m_db.CommandText = @"CREATE TABLE tChat(id INTEGER NOT NULL PRIMARY KEY, name TEXT, type BOOLEAN);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = @"CREATE TABLE tParticipants(id INTEGER NOT NULL PRIMARY KEY, userID INTEGER, roomID INTEGER);";
            m_db.ExecuteNonQuery();

            m_db.CommandText = @"CREATE TABLE tMessage(id INTEGER NOT NULL PRIMARY KEY, roomID INTEGER, userID INTEGER, message TEXT);";
            m_db.ExecuteNonQuery();
        }
        static void InitDefaults()
        {
            // Example users
            AddUser("Bob", "Bob");
            AddUser("Alice", "Alice");

            // Example chat
            m_db.CommandText = "INSERT INTO tChat(name, type) VALUES ('cool chat', false);";
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
        static void Execute(string query)
        {
            m_db.CommandText = query;
            m_db.ExecuteNonQuery();
        }
        public static bool DoesUserExist(string username)
        {
            // m_db.CommandText = string.Format("SELECT * FROM tUser WHERE username = {0}", username);
            m_db.CommandText = string.Format("SELECT username FROM tUser");

            SQLiteDataReader reader = m_db.ExecuteReader();
            while(reader.Read())
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
        public static void AddUser(string username, string password)
        {
            string salt = Guid.NewGuid().ToString();
            password = HashString(password, salt);
            Execute(string.Format("INSERT INTO tUser(username, password, salt, nickname, status) VALUES ('{0}', '{1}', '{2}', '{0}', 'Amogus');", username, password, salt));
        }
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
        public static bool LoginUser(string username, string password)
        { 
            string hashedPassword = HashString(password, GetUserSalt(username));
            bool match = false;
            m_db.CommandText = string.Format("SELECT password FROM tUser WHERE username='{0}';", username);
            SQLiteDataReader reader = m_db.ExecuteReader();
            if (!reader.HasRows) throw new Exception("User not found");

            reader.Read();
            match = reader.GetValue(0).ToString().Equals(hashedPassword);
            reader.Close();
            return match;
        }
        public static void SendMessage(int roomID, int senderID, string msgContent)
        {
            Execute(string.Format("INSERT INTO tMessage(roomID, userID, message) VALUES ({0}, {1}, {2});", roomID.ToString(), senderID.ToString(), msgContent));
        }
        public static List<Message> LoadMessages(int roomID)
        {
            // CONTINUE
            return new List<Message>();
        }
    }
}
