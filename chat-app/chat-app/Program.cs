using System;
using System.Threading;

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
                Thread t = new Thread(() => Server.Start());
                t.Start();
            } catch (Exception e)
            {
                Console.WriteLine($"Server error: {e}");
            }
        }
    }
}
