using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using Utilities;

namespace Server
{
    class Server
    {
        private const int PORT = 8888;
        private const int MAX_CLIENTS = 10;
        private const int BUFFER_SIZE = 1024;
        public static void Start()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, PORT);

            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(MAX_CLIENTS);
                Console.WriteLine($">> Server is up to snuff on port {PORT}");

                while (true)
                {
                    Socket clientSocket = listener.Accept();
                    Console.WriteLine($">> A wild {clientSocket} appears!");
                    // Just create a thread to handle the new client and detach
                    Thread t = new Thread(() => HandleClient(clientSocket));
                    t.Start();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static void HandleClient(Socket clientSocket)
        {
            RequestResult handler;
            handler.newHandler = null;
            try
            {
                while (true)
                {

                }
            } catch(Exception)
            {
                // If client disconnects or something
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

    }
}
