using System;
using System.Threading;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                SqliteDatabase.InitDatabase();
                Thread t = new Thread(() => Server.Start());
                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Server error: {e}");
            }
        }
    }
}