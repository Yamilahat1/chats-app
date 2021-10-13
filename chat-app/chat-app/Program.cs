using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Server.Start();
            try
            { 
                DatabaseManager.InitDatabase();
            } catch (Exception e)
            {
                Console.WriteLine($"There was an error initializing the database: {e}");
            }
        }
    }
}
