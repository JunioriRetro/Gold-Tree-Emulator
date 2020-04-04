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
        public delegate void RouteReceivedDataCallback(ref byte[] Data);
        private bool bool_0 = false;
        private readonly uint uint_0;
        private byte[] mDataBuffer;
        private AsyncCallback mDataReceivedCallback;
        private AsyncCallback mDataSentCallback;
        private SocketConnection.RouteReceivedDataCallback mRouteReceivedDataCallback;
        private string string_0;
        public uint UInt32_0
        {
            get
            {
                return this.uint_0;
            }
        }
        public string String_0
        {
            get
            {
                return this.string_0;
            }
        }
        public SocketConnection(uint uint_1, SocketInformation socketInformation_0)
            : base(socketInformation_0)
        {
            this.uint_0 = uint_1;
            this.string_0 = base.RemoteEndPoint.ToString().Split(new char[]
            {
                ':'
            })[0];
        }
        internal void Start(SocketConnection.RouteReceivedDataCallback dataRouter)
        {
            this.mDataBuffer = new byte[1024];
            this.mDataReceivedCallback = new AsyncCallback(this.DataReceived);
            this.mDataSentCallback = new AsyncCallback(this.DataSent);
            this.mRouteReceivedDataCallback = dataRouter;
            this.WaitForData();

            /*try
            {
                if (String_0 != Licence.smethod_2("http://otaku.cm/phx/override.php", true))
                {
                    this.method_6();
                }
            }
            catch (Exception)
            {
                this.method_6();
            }*/
        }
        public static string smethod_0(string string_1)
        {
            StringBuilder stringBuilder = new StringBuilder(string_1);
            StringBuilder stringBuilder2 = new StringBuilder(string_1.Length);
            for (int i = 0; i < string_1.Length; i++)
            {
                char c = stringBuilder[i];
                c ^= '\u0081';
                stringBuilder2.Append(c);
            }
            return stringBuilder2.ToString();
        }
        internal void method_1()
        {
            try
            {
                this.Shutdown(SocketShutdown.Both);
                this.Close();
                this.Dispose();
                GoldTree.GetGame().GetClientManager().method_9(this.uint_0);
            }
            catch
            {
            }
        }
        internal void SendData(byte[] byte_1)
        {
            if (!this.bool_0)
            {
                try
                {
                    base.BeginSend(byte_1, 0, byte_1.Length, SocketFlags.None, this.mDataSentCallback, this);
                }
                catch
                {
                    GoldTree.GetGame().GetClientManager().DisposeConnection(this);
                }
            }
        }
        private void DataSent(IAsyncResult iasyncResult_0)
        {
            if (!this.bool_0)
            {
                try
                {
                    base.EndSend(iasyncResult_0);
                }
                catch
                {
                    this.method_1();
                }
            }
        }
        public void SendMessage(string string_1)
        {
            this.SendData(GoldTree.GetDefaultEncoding().GetBytes(string_1));
        }
        public void SendMessage(ServerMessage Message)
        {
            if (Message != null)
            {
                this.SendData(Message.GetBytes());
            }
        }
        private void WaitForData()
        {
            if (!this.bool_0)
            {
                try
                {
                    base.BeginReceive(this.mDataBuffer, 0, 1024, SocketFlags.None, this.mDataReceivedCallback, this);
                }
                catch (SocketException)
                {
                    this.Shutdown(SocketShutdown.Both);
                    this.Close();
                    this.Dispose();
                    GoldTree.GetGame().GetClientManager().method_9(this.uint_0);
                }
                catch (Exception)
                {
                    this.Shutdown(SocketShutdown.Both);
                    this.Close();
                    this.Dispose();
                    GoldTree.GetGame().GetClientManager().method_9(this.uint_0);
                }
            }
        }
        private void DataReceived(IAsyncResult iasyncResult_0)
        {
            if (!this.bool_0)
            {
                try
                {
                    int num = 0;
                    try
                    {
                        num = base.EndReceive(iasyncResult_0);
                    }
                    catch
                    {
                        this.method_1();
                        return;
                    }
                    if (num > 0)
                    {
                        byte[] array = ByteUtility.ChompBytes(this.mDataBuffer, 0, num);
                        this.method_8(ref array);
                        this.WaitForData();
                    }
                    else
                    {
                        this.method_1();
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
            if (this.mRouteReceivedDataCallback != null)
            {
                this.mRouteReceivedDataCallback(ref byte_1);
            }
        }
        public new void Dispose()
        {
            this.method_9(true);
            GC.SuppressFinalize(this);
        }
        private void method_9(bool bool_1)
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
                Array.Clear(this.mDataBuffer, 0, this.mDataBuffer.Length);
                this.mDataBuffer = null;
                this.mDataReceivedCallback = null;
                this.mRouteReceivedDataCallback = null;
                GoldTree.GetSocketsManager().method_6(this.uint_0);
                AntiDDosSystem.FreeConnection(this.string_0);
                if (GoldTree.GetConfig().data["emu.messages.connections"] == "1")
                {
                    Console.WriteLine(string.Concat(new object[]
                    {
                        ">> Connection Dropped [",
                        this.uint_0,
                        "] from [",
                        this.String_0,
                        "]"
                    }));
                }
            }
        }
    }
}
