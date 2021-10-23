using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace client
{
    enum Code
    {
        Login = 1, Signup, Signout, LoadChat, GetAllChats, SendMessage
    }

    internal class Communicator
    {
        private static Socket m_sock;
        private static IPEndPoint m_endPoint;
        static Communicator()
        {

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            m_endPoint = new IPEndPoint(ipAddr, 8888);

            m_sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public static void Connect(bool waitForServer = false)
        {
            while (waitForServer)
            {
                try
                {
                    m_sock.Connect(m_endPoint);
                }
                catch { }
                if (m_sock.Connected) return;
            }
        }
        public static void Send(string msg)
        {
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
            m_sock.Send(msgBytes);
        }
        public static void Send(string root, Dictionary<string, string> dict, Code code)
        {
            Send(BuildMessage(SerializeXML(root, dict), code));
        }
        public static Dictionary<string,string> Recv()
        {
            byte[] messageReceived = new byte[1024];
            int byteRecv = m_sock.Receive(messageReceived);
            return DeserializeXML(Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
        }
        private static string SerializeXML(string root, Dictionary<string, string> data)
        {
            string str = string.Format("<{0}>", root);
            foreach (KeyValuePair<string, string> curr in data)
            {
                str += string.Format("<{0}>{1}</{0}>", curr.Key, curr.Value);
            }

            return string.Format("{0}</{1}>", str, root);
        }
        private static Dictionary<string, string> DeserializeXML(string xml)
        {
            var res = new Dictionary<string, string>();
            Regex rg = new Regex(@"<(?<Tag>\w+)>(?<Data>[^<]*)");
            MatchCollection matches = rg.Matches(xml);
            foreach (Match match in matches) if (match.Groups["Data"].Value != "") res.Add(match.Groups["Tag"].Value, match.Groups["Data"].Value);
            return res;
        }
        private static string BuildMessage(string data, Code code)
        {
            string headers = ((int)code).ToString();
            int lenSize = 4;
            while (lenSize --> data.Length.ToString().Length) headers += "0";
            headers += data.Length.ToString() + data;
            return headers;
        }
    }
}
