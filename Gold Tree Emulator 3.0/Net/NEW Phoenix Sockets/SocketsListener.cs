using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using GoldTree.Core;
namespace GoldTree.Net
{
    internal sealed class Class114
    {
        private const int int_0 = 1;
        private Socket socket_0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private bool bool_0;
        private AsyncCallback asyncCallback_0;
        private int int_1 = Process.GetCurrentProcess().Id;
        private SocketsManager class113_0;

        public Class114(string string_0, int Port, SocketsManager Manager)
        {
            this.socket_0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(GoldTree.GetConfig().data["game.tcp.bindip"].Replace(",", ".")), Port);
            this.socket_0.Bind(localEP);
            this.socket_0.Listen(0x3e8);
            this.asyncCallback_0 = new AsyncCallback(this.method_4);
            this.class113_0 = Manager;
            AntiDDosSystem.SetupTcpAuthorization(0x4e20);
            Logging.WriteLine("Listening for connections on port: " + Port);
        }
        public void method_0()
        {
            if (!this.bool_0)
            {
                this.bool_0 = true;
                this.socket_0.BeginAccept(this.asyncCallback_0, this.socket_0);
            }
        }
        public void method_1()
        {
            if (this.bool_0)
            {
                this.bool_0 = false;
                try
                {
                    this.socket_0.Close();
                }
                catch
                {
                }
                Console.WriteLine("Listener -> Stopped!");
            }
        }
        public void method_2()
        {
            this.method_1();
            this.socket_0 = null;
            this.class113_0 = null;
        }
        private void method_3()
        {
            try
            {
                this.socket_0.BeginAccept(this.asyncCallback_0, this.socket_0);
            }
            catch
            {
            }
        }
        private void method_4(IAsyncResult iasyncResult_0)
        {
            if (this.bool_0)
            {
                try
                {
                    int num = this.class113_0.method_4();
                    if (num > -1)
                    {
                        Socket socket = ((Socket)iasyncResult_0.AsyncState).EndAccept(iasyncResult_0);
                        if (!AntiDDosSystem.CheckConnection(socket))
                        {
                            try
                            {
                                socket.Close();
                                socket.Dispose();

                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            this.class113_0.method_5(socket.DuplicateAndClose(this.int_1), num);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogException("[TCPListener.OnRequest]: Could not handle new connection request: " + ex.ToString());
                }
                finally
                {
                    this.method_3();
                }
            }
        }
        internal void method_5()
        {
            this.bool_0 = false;
            try
            {
                this.socket_0.Shutdown(SocketShutdown.Both);
                this.socket_0.Close();
                this.socket_0.Dispose();
            }
            catch
            {
            }
        }
        public bool Boolean_0
        {
            get
            {
                return this.Boolean_0;
            }
        }
    }
}