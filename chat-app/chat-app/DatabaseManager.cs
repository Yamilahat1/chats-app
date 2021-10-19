using System;
using System.Data.SQLite;
using System.IO;

// Todo: add mutex
namespace Server
{
    static class SqliteDatabase
    {
        private static SQLiteCommand m_db;
        private static SQLiteConnection m_connection;
        private const string LOCATION = "D:\\test.db";
        public static void InitDatabase()
        {
            // So we don't accidently override existing database when starting the server.

            string cs = @"URI=file:D:\test.db";
            m_connection = new SQLiteConnection(cs);
            m_connection.Open();

            m_db = new SQLiteCommand(m_connection);
            if (File.Exists(LOCATION)) return;
            
            CreateTables();
            InitDefaults();
        }
        static void CreateTables()
        {
            // Create user table
            m_db.CommandText = @"CREATE TABLE tUser(id INTEGER NOT NULL PRIMARY KEY, username TEXT, password TEXT, nickname TEXT, status TEXT);";
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
            Execute(string.Format("INSERT INTO tUser(username, password, nickname, status) VALUES ('{0}', '{1}', '{0}', 'Amogus');", username, password));
        }
    }
}
