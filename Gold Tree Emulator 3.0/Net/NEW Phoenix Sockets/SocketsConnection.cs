using System;
using System.Net.Sockets;
using System.Text;
using System.Net;

using GoldTree.Util;
using GoldTree.Messages;

namespace GoldTree.Net
{
    public sealed class SocketConnection : Socket, IDisposable
    {
        public delegate void GDelegate0(ref byte[] Data);
        private bool bool_0;

        public readonly uint Id;

        private byte[] Buffer;

        private AsyncCallback asyncCallback_0;
        private AsyncCallback asyncCallback_1;
        private SocketConnection.GDelegate0 gdelegate0_0;

        public string Address;

        public SocketConnection(uint pSockID, SocketInformation socketInformation_0)
            : base(socketInformation_0)
        {
            this.bool_0 = false;
            this.Id = pSockID;

            this.Address = base.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
        }

        internal void Initialise(SocketConnection.GDelegate0 gdelegate0_1)
        {
            this.Buffer = new byte[1024];

            this.asyncCallback_0 = new AsyncCallback(this.OnReceive);
            this.asyncCallback_1 = new AsyncCallback(this.OnSend);

            this.gdelegate0_0 = gdelegate0_1;

            this.BeginReceive();
        }

        public static string smethod_0(string string_1)
        {
            StringBuilder stringBuilder = new StringBuilder(string_1);
            StringBuilder stringBuilder2 = new StringBuilder(string_1.Length);
            for (int i = 0; i < string_1.Length; i++)
            {
                char c = stringBuilder[i];
                c ^= (char)(c ^ '\x0081');
                stringBuilder2.Append(c);
            }
            return stringBuilder2.ToString();
        }

        internal void method_1()
        {
            try
            {
                this.Close();
                GoldTree.GetGame().GetClientManager().method_9(this.Id);
            }
            catch
            {
            }
        }

        internal void SendData(byte[] bytes)
        {
            if (!this.bool_0)
            {
                try
                {
                    base.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, this.asyncCallback_1, this);
                }
                catch
                {
                    GoldTree.GetGame().GetClientManager().DisposeConnection(this);
                }
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            if (!this.bool_0)
            {
                try
                {
                    base.EndSend(ar);
                }
                catch
                {
                    this.method_1();
                }
            }
        }

        public void SendMessage(string str)
        {
            this.SendData(GoldTree.GetDefaultEncoding().GetBytes(str));
        }

        public void SendMessage(ServerMessage message)
        {
            if (message != null)
            {
                this.SendData(message.GetBytes());
            }
        }

        private void BeginReceive()
        {
            if (!this.bool_0)
            {
                try
                {
                    base.BeginReceive(this.Buffer, 0, 0x400, SocketFlags.None, this.asyncCallback_0, this);
                }
                catch (Exception)
                {
                    this.method_1();
                }
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            if (!this.bool_0)
            {
                try
                {
                    int num = 0;

                    try
                    {
                        num = base.EndReceive(ar);
                    }
                    catch
                    {
                        this.method_1();
                        return;
                    }

                    if (num < 0)
                    {
                        this.method_1();
                    }
                    else
                    {
                        byte[] array = ByteUtility.ChompBytes(this.Buffer, 0, num);
                        this.method_8(ref array);
                        this.BeginReceive();
                    }
                }
                catch
                {
                    this.method_1();
                }
            }
        }

        private void method_8(ref byte[] byte_1)
        {
            if (this.gdelegate0_0 != null)
            {
                this.gdelegate0_0(ref byte_1);
            }
        }

        private new void Dispose(bool bool_1)
        {
            if (!this.bool_0 && bool_1)
            {
                this.bool_0 = true;
                try
                {
                    base.Shutdown(SocketShutdown.Both);
                    base.Close();
                    base.Dispose();
                }
                catch
                {
                }
                Array.Clear(this.Buffer, 0, this.Buffer.Length);
                this.Buffer = null;
                this.asyncCallback_0 = null;
                this.gdelegate0_0 = null;
                GoldTree.GetSocketsManager().method_6(this.Id);
                AntiDDosSystem.FreeConnection(this.Address);

                if (GoldTree.GetConfig().data["emu.messages.connections"] == "1")
                {
                    Console.WriteLine(string.Concat(new object[]
                                    {
                                        ">> Connection Dropped [",
                                        this.Id,
                                        "] from [",
                                        this.Address,
                                        "]"
                                    }));
                }
            }
        }
    }
}