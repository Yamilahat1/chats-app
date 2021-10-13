using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            { 
                DatabaseManager.InitDatabase();
            } catch (Exception e)
            {
                Console.WriteLine($"There was an error initializing the database: {e}");
            }

            try
            {
                Server.Start();
            } catch (Exception e)
            {
                Console.WriteLine($"Server error: {e}");
            }
        }
    }
}
