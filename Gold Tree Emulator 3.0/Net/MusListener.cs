using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GoldTree.Core;
namespace GoldTree.Net
{
	internal sealed class MusListener
	{
		public Socket ServerSocket;

		public string Address;
		public int Port;

		public HashSet<string> AllowedAddresses;

		public MusListener(string addr, int port, string[] allowedAddresses, int backlog)
		{
			this.Address = "localhost";
			this.Port = port;

			this.AllowedAddresses = new HashSet<string>();

			this.AllowedAddresses.Add(GoldTree.string_5);
			for (int i = 0; i < allowedAddresses.Length; i++)
			{
				string item = allowedAddresses[i];
				this.AllowedAddresses.Add(item);
			}

			try
			{
				this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				this.ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(addr), this.Port));
				this.ServerSocket.Listen(backlog);

				this.ServerSocket.BeginAccept(new AsyncCallback(this.OnAcceptCallback), this.ServerSocket);

				Logging.WriteLine("Listening for MUS on port: " + this.Port);
			}
			catch (Exception ex)
			{
				throw new Exception("Could not set up MUS socket:\n" + ex.ToString());
			}
		}

		public void OnAcceptCallback(IAsyncResult ar)
		{
            try
            {
                Socket socket = ((Socket)ar.AsyncState).EndAccept(ar);

                string item = socket.RemoteEndPoint.ToString().Split(new char[]
				{
					':'
				})[0];

                if (this.AllowedAddresses.Contains(item))
                    new MusHandler(socket);
                else
                    socket.Close();
            }
            catch (Exception) { }

			this.ServerSocket.BeginAccept(new AsyncCallback(this.OnAcceptCallback), this.ServerSocket);
		}
	}
}
