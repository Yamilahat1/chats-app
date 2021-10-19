using System;
using System.Data.SQLite;
using System.IO;

// Todo: add mutex
namespace Server
{
    public class SqliteDatabase
    {
        private static SQLiteCommand m_db;
        private const string LOCATION = "D:\\test.db";
        protected SqliteDatabase()
        {
            // So we don't accidently override existing database when starting the server.
            if (File.Exists(LOCATION)) return;

            string cs = @"URI=file:D:\test.db";
            using var con = new SQLiteConnection(cs);
            con.Open();

            m_db = new SQLiteCommand(con);
            InitDatabase();
        }
        private static void InitDatabase()
        {
            CreateTables();
            InitDefaults();
        }
        protected static void CreateTables()
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
        protected static void InitDefaults()
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
        private static void Execute(string query)
        {
            m_db.CommandText = query;
            m_db.ExecuteNonQuery();
        }
        protected static bool DoesUserExist(string username)
        {
            m_db.CommandText = string.Format("SELECT * FROM tUser WHERE username = {0}", username);
            SQLiteDataReader reader = m_db.ExecuteReader();
            return reader.HasRows;
        }
        protected static void AddUser(string username, string password)
        {
            Execute(string.Format("INSERT INTO tUser(username, password, nickname, status) VALUES ('{0}', '{1}', '{0}', 'Amogus');", username, password));
        }
    }
}
