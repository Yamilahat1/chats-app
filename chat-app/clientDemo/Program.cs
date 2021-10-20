using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{

	class Program
	{

		static void Main(string[] args)
		{
			ExecuteClient();
		}
		static void Send(Socket socket, string msg)
        {
			byte[] messageSent = Encoding.ASCII.GetBytes(msg); // Create message
			socket.Send(messageSent);
		}
		static string Recv(Socket socket)
        {
			byte[] messageReceived = new byte[1024];
			int byteRecv = socket.Receive(messageReceived);
			return Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
		}
		static void ExecuteClient()
		{
			try
			{
				IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
				IPAddress ipAddr = ipHost.AddressList[0];
				IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8888);

				Socket socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				string input;

				try
				{
					while(!socket.Connected)
                    {
						try
						{
							socket.Connect(localEndPoint);
						}
						catch { }
                    }
					Console.WriteLine("Connected!");

					while(true)
                    {
						Console.Write("Send: ");
						input = Console.ReadLine();
						if (input == "quit") break;
						Send(socket, input);
						Console.WriteLine("Received: " + Recv(socket));
                    }

					socket.Shutdown(SocketShutdown.Both);
					socket.Close();
				}

				// Manage of Socket's Exceptions
				catch (ArgumentNullException ane)
				{

					Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
				}

				catch (SocketException se)
				{

					Console.WriteLine("SocketException : {0}", se.ToString());
				}

				catch (Exception e)
				{
					Console.WriteLine("Unexpected exception : {0}", e.ToString());
				}
			}

			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}
		}
	}
}
