using System;
using System.Data.SQLite;
using System.IO;

namespace Server
{
    class DatabaseManager
    {
        private const string LOCATION = "D:\\test.db";
        public static void InitDatabase()
        {
            // So we don't accidently override existing database when starting the server.
            if (File.Exists(LOCATION)) return;

            string cs = @"URI=file:D:\test.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            CreateTables(cmd);
            InitDefaults(cmd);
        }
        private static void CreateTables(SQLiteCommand cmd)
        {
            // Create user table
            cmd.CommandText = @"CREATE TABLE tUser(id INTEGER NOT NULL PRIMARY KEY, username TEXT, password TEXT, nickname TEXT, status TEXT);";
            cmd.ExecuteNonQuery();

            // Create chat table
            cmd.CommandText = @"CREATE TABLE tChat(id INTEGER NOT NULL PRIMARY KEY, name TEXT, type BOOLEAN);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE tParticipants(id INTEGER NOT NULL PRIMARY KEY, userID INTEGER, roomID INTEGER);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE tMessage(id INTEGER NOT NULL PRIMARY KEY, roomID INTEGER, userID INTEGER, message TEXT);";
            cmd.ExecuteNonQuery();
        }
        private static void InitDefaults(SQLiteCommand cmd)
        {
            // Example users
            cmd.CommandText = "INSERT INTO tUser(username, password, nickname, status) VALUES ('bob', 'bob', 'Bob', 'hi im bob');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO tUser(username, password, nickname, status) VALUES ('alice', 'alice', 'Alice', 'hi im alice');";
            cmd.ExecuteNonQuery();


            // Example chat
            cmd.CommandText = "INSERT INTO tChat(name, type) VALUES ('cool chat', false);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO tParticipants(userID, roomID) VALUES (1, 1);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO tParticipants(userID, roomID) VALUES (2, 1);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO tMessage(roomID, userID, message) VALUES (1, 1, 'hi alice');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO tMessage(roomID, userID, message) VALUES (1, 2, 'hi bob');";
            cmd.ExecuteNonQuery();
        }
    }
}
