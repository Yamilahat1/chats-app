using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using Utilities;
using Handlers;
using System.Text;
using System.IO;

namespace Server
{
    class Server
    {
        private const int PORT = 8888;
        private const int MAX_CLIENTS = 10;
        private const int BUFFER_SIZE = 1024;
        private static List<string> m_names;
        public static void Start()
        {
            string nick;
            m_names = new List<string>();
            LoadNames();
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
                    if (m_names.Count>0)
                    {
                        nick = m_names[0];
                        m_names.Remove(nick);
                    }
                    else
                    {
                        nick = "amogus";
                    }
                    Console.WriteLine($">> A wild {nick} appears!");
                    // Just create a thread to handle the new client and detach
                    Thread t = new Thread(() => HandleClient(clientSocket, nick));
                    t.Start();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static void HandleClient(Socket clientSocket, string nick)
        {
            IHandler handler = new LoginHandler();
            RequestInfo reqInfo;
            RequestResult res;
            string data = "";
            string headers;
            int msgLen;
            List<char> buffer = new List<char>();
            try
            {
                while (true)
                {
                    headers = "";
                    buffer.Clear();
                    headers = Recv(clientSocket, (int)Defines.LEN_END);
                    msgLen = Convert.ToInt32(headers.Substring((int)Defines.LEN_BEGIN, (int)Defines.LEN_END - (int)Defines.LEN_BEGIN));
                    if (msgLen > 0) data = Recv(clientSocket, msgLen);
                    reqInfo.id = (uint)headers[(int)Defines.MSG_CODE] - '0';
                    PushToBuffer(ref buffer, headers, data, msgLen);
                    reqInfo.buffer = buffer;

                    if (!handler.Validation(reqInfo)) continue;
                    res = handler.HandleRequest(reqInfo);
                    if (res.newHandler != null) handler = res.newHandler;

                    Send(clientSocket, string.Join("", res.response.ToArray()));
                }
            } catch(Exception)
            {
                Console.WriteLine($">> Goodbye {nick}");
                m_names.Add(nick);
                // If client disconnects or something
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
        private static void PushToBuffer(ref List<char> buffer, string headers, string msg, int msgLen)
        {
            for (int i = 0; i < (int)Defines.LEN_END; i++) buffer.Add(headers[i]);
            for (int i = 0; i < msgLen; i++) buffer.Add(msg[i]);
        }
        private static string Recv(Socket clientSocket, int size = BUFFER_SIZE)
        {
            byte[] bytes = new byte[size];
            string data = null;

            int numByte = clientSocket.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, numByte);
            return data;
        }
        private static void Send(Socket clientSocket, string msg)
        {
            byte[] message = Encoding.ASCII.GetBytes(msg);
            clientSocket.Send(message);
        }
        private static void LoadNames()
        {
            using (StreamReader sr = new StreamReader("names.txt"))
            {
                string line;
                while((line = sr.ReadLine()) != null) m_names.Add(line);
            }
            m_names.Shuffle();
        }
    }
    static class ExtensionsClass
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
