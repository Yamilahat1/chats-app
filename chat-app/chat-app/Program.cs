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
                Thread t = new Thread(() => Server.Start());
                t.Start();
            } catch (Exception e)
            {
                Console.WriteLine($"Server error: {e}");
            }
        }
    }
}
