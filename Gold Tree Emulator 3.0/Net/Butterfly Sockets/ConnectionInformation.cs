//using System;
//using System.Net.Sockets;
//using SharedPacketLib;
//using System.IO;
//using System.Text;
//using System.Diagnostics;
//using GoldTree.Messages;
//using GoldTree.Net.Messages;
//using GoldTree.Net.LoggingSystem;
//using GoldTree.Core;

//namespace GoldTree.Net
//{
//    public class ConnectionInformation : IDisposable
//    {
//        #region declares
//        /// <summary>
//        /// The socket this connection is based upon
//        /// </summary>
//        private Socket dataSocket;

//        /// <summary>
//        /// The manager which created this class
//        /// </summary>
//        private SocketManager manager;

//        /// <summary>
//        /// The ip of this connection
//        /// </summary>
//        private string ip;

//        /// <summary>
//        /// The id of this connection
//        /// </summary>
//        private int connectionID;

//        /// <summary>
//        /// Boolean indicating of this instance is connected to the user or not
//        /// </summary>
//        private bool isConnected;



//        /// <summary>
//        /// Is used when a connection state changes
//        /// </summary>
//        /// <param name="state">The new state of the connection</param>
//        public delegate void ConnectionChange(ConnectionInformation information, ConnectionState state);
//        /// <summary>
//        /// Is triggered when the user connects/disconnects
//        /// </summary>
//        public event ConnectionChange connectionChanged;

//        /// <summary>
//        /// Buffer of the connection
//        /// </summary>
//        private byte[] buffer;

//        /// <summary>
//        /// This item contains the data parser for the connection
//        /// </summary>
//        public IDataParser parser { get; set; }

//        private AsyncCallback sendCallback;

//        public static bool disableSend = false;
//        public static bool disableReceive = false;

//        #endregion

//        #region constructor

//        /// <summary>
//        /// Creates a new Connection witht he given information
//        /// </summary>
//        /// <param name="dataStream">The Socket of the connection</param>
//        /// <param name="connectionID">The id of the connection</param>
//        public ConnectionInformation(Socket dataStream, int connectionID, SocketManager manager, IDataParser parser, string ip)
//        {
//            this.parser = parser;
//            this.buffer = new byte[GameSocketManagerStatics.BUFFER_SIZE];
//            this.manager = manager;
//            this.dataSocket = dataStream;
//            this.dataSocket.SendBufferSize = GameSocketManagerStatics.BUFFER_SIZE;
//            this.ip = ip;
//            this.connectionID = connectionID;
//            this.sendCallback = new AsyncCallback(sentData);

//            if (connectionChanged != null)
//                connectionChanged.Invoke(this, ConnectionState.open);

//            MessageLoggerManager.AddMessage(null, connectionID, LogState.ConnectionOpen);
//        }

//        /// <summary>
//        /// Starts this item packet processor
//        /// MUST be called before sending data
//        /// </summary>
//        public void startPacketProcessing()
//        {
//            if (!isConnected)
//            {
//                this.isConnected = true;
//                //Console.WriteLine("Starting packet processsing of client [" + this.connectionID + "]");
//                try
//                {
//                    this.dataSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(incomingDataPacket), dataSocket);
//                }
//                catch
//                {
//                    this.disconnect();
//                }
//            }
//        }

//        #endregion

//        #region getters

//        /// <summary>
//        /// Returns the ip of the current connection
//        /// </summary>
//        /// <returns>The ip of this connection</returns>
//        public string getIp()
//        {
//            return ip;
//        }

//        /// <summary>
//        /// Returns the connection id
//        /// </summary>
//        /// <returns>The id of the connection</returns>
//        public int getConnectionID()
//        {
//            return connectionID;
//        }

//        #endregion

//        #region methods

//        #region connection management
//        /// <summary>
//        /// Disconnects the current connection
//        /// </summary>
//        internal void disconnect()
//        {
//            try
//            {
//                if (isConnected)
//                {
//                    this.isConnected = false;
//                    MessageLoggerManager.AddMessage(null, connectionID, LogState.ConnectionClose);

//                    //Console.WriteLine("Connection [" + this.connectionID + "] has been disconnected");

//                    try
//                    {
//                        if (this.dataSocket != null && this.dataSocket.Connected)
//                        {
//                            dataSocket.Shutdown(SocketShutdown.Both);
//                            dataSocket.Close();
//                        }
//                    }
//                    catch { }

//                    dataSocket.Dispose();
//                    parser.Dispose();

//                    try
//                    {
//                        if (connectionChanged != null)
//                            connectionChanged.Invoke(this, ConnectionState.closed);
//                    }
//                    catch (Exception Ex)
//                    {
//                        Console.WriteLine(Ex.ToString());
//                    }
//                    this.connectionChanged = null;

//                    AntiDDosSystem.FreeConnection(this.getIp());
//                    if (GoldTree.GetConfig().data["emu.messages.connections"] == "1")
//                    {
//                        Console.WriteLine(string.Concat(new object[]
//                                        {
//                                            ">> Connection Dropped [",
//                                            this.connectionID,
//                                            "] from [",
//                                            this.getIp(),
//                                            "]"
//                                        }));
//                    }
//                }
//                else
//                {
//                    //Console.WriteLine("Connection [" + this.connectionID + "] has already been disconnected - ignoring disconnect call");
//                }
//            }
//            catch { }
//        }


//        /// <summary>
//        /// Disposes the current item
//        /// </summary>
//        public void Dispose()
//        {
//            if (isConnected)
//            {
//                this.disconnect();
//            }
//        }
//        #endregion

//        #region data receiving

//        /// <summary>
//        /// Receives a packet of data and processes it
//        /// </summary>
//        /// <param name="iAr">The interface of an async result</param>
//        private void incomingDataPacket(IAsyncResult iAr)
//        {
            
//            //Console.WriteLine("Packet received from client [" + this.connectionID + "]");
//            int bytesReceived;
//            try
//            {
//                //The amount of bytes received in the packet
//                bytesReceived = dataSocket.EndReceive(iAr);
//            }
//            #region socket error / closed
//            catch (ObjectDisposedException) { disconnect(); return; }
//            catch (SocketException ex)
//            {
//                Console.WriteLine("SocketException :\r\n" + ex.Message + "\r\n" + ex.StackTrace);
//                disconnect(); return;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception :\r\n" + ex.Message + "\r\n" + ex.StackTrace);
//                disconnect();
//                return;
//            }
//            //catch (Exception e)
//            //{
//            //    Console.WriteLine("Error 0x003: " + e.ToString());
//            //    disconnect();
//            //    return;
//            //}

//            if (bytesReceived == 0)
//            {
//                //Console.WriteLine("Socket send 0 bytes, asuming closure of socket");
//                //if (lastSent != null)
//                //    Console.WriteLine("Error 0x000a: Client closed. Last sent packet: " + System.Text.Encoding.Default.GetString(lastSent));

//                //else
//                //    Console.WriteLine("Error 0x000b: Client closed");
//                disconnect();
//                return;
//            }
//            #endregion
//            //Console.WriteLine("Packet size is [" + bytesReceived + "] from client [" + this.connectionID + "]");
//            try
//            {

//                if (!disableReceive)
//                {
//                    byte[] packet = new byte[bytesReceived];
//                    Array.Copy(buffer, packet, bytesReceived);

//                    //if (logShit)
//                    //{
//                    //    string packetData = System.Text.Encoding.Default.GetString(packet);
//                    //    LogMessage(string.Format("Data from client => [{0}]", packetData));
//                    //}

//                    MessageLoggerManager.AddMessage(packet, connectionID, LogState.Normal);
//                    handlePacketData(packet);
//                }
//            }
//            #region exception handeling and continueing receiving
//            //catch //(Exception e)
//            //{
//            //    //Console.WriteLine("Error 0x002: " + e.ToString());
//            //    disconnect();
//            //}
//            catch (PacketMalformedException ex)
//            {
//                Console.WriteLine("Packet was malformed: " + ex.Message);
//                if (this.connectionChanged != null)
//                    connectionChanged.Invoke(this, ConnectionState.malfunctioning_packet);
//            }

//            catch (SocketException ex)
//            {
//                Console.WriteLine("SocketException :\r\n" + ex.Message + "\r\n" + ex.StackTrace);
//                this.disconnect();
//                return;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error occured in the connection manager:\r\n" + ex.Message + "\r\n" + ex.StackTrace);
//                this.disconnect();
//                return;
//            }

//            finally
//            {
//                try
//                {
//                    //Console.WriteLine("Started listening for next message from client [" + this.connectionID + "]");

//                    //and we keep looking for the next packet
//                    this.dataSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(incomingDataPacket), dataSocket);
//                }
//                catch (SocketException)
//                {
//                    Console.WriteLine("Socket exception while listening for next packet for client [" + this.connectionID + "]");
//                    disconnect();
//                }
//                catch (ObjectDisposedException)
//                {
//                    disconnect();
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine("Error 0x001: " + e.ToString());
//                    disconnect();
//                }
//            }
//            #endregion
//        }

//        /// <summary>
//        /// Handles packet data
//        /// </summary>
//        /// <param name="packet">The data received by the </param>
//        private void handlePacketData(byte[] packet)
//        {
//            if (parser != null)
//                parser.handlePacketData(packet);
//        }

//        #endregion

//        #region data sending
//        /// <summary>
//        /// Sends data to this instance
//        /// </summary>
//        /// <param name="Data">The data to be send</param>
//        //public void sendData(ServerOutgoingPacket data)
//        //{
//        //    try
//        //    {
//        //        Console.WriteLine("Sending packet [" + data.getOpCode().ToString() + "] to client [" + this.connectionID + "]");
//        //        byte[] dataBytes = data.GetPacketData();
//        //        this.dataSocket.BeginSend(dataBytes, 0, dataBytes.Length, 0, new AsyncCallback(sentData), this.dataSocket);
                
//        //    }
//        //    catch
//        //    {
//        //        disconnect();
//        //    }
//        //}

//        public void SendData(byte[] packet)
//        {
//            sendData(packet);
//        }

//        public void sendbData(byte[] packet)
//        {
//            sendData(packet);
//        }

//        //private byte[] lastSent;
//        private void sendData(byte[] packet)
//        {
//            try
//            {
//                //lastSent = packet;

//                //if (logShit)
//                //{
//                    //string packetData = System.Text.Encoding.Default.GetString(packet);

//                    //StackTrace stackTrace = new StackTrace();
//                    ////Console.WriteLine(stackTrace.ToString());
//                    //Console.ForegroundColor = ConsoleColor.Green;
//                    //Console.WriteLine(string.Format("Data from server => [{0}]", packetData));
//                    //Console.ForegroundColor = ConsoleColor.White;
//                //}
                
//                //Console.WriteLine("Sending byte packet of length [" + packet.Length + "] to client [" + this.connectionID + "]");
//                //this.dataSocket.BeginSend(packet, 0, packet.Length, 0, sendCallback, null);
//                SendUnsafeData(packet);
//            }
//            catch
//            {
//                disconnect();
//            }
//        }

//        public void SendUnsafeData(byte[] packet)
//        {
//            if (!isConnected || disableSend)
//                return;
//            //string packetData = System.Text.Encoding.Default.GetString(packet);
//            //Console.WriteLine(string.Format("Data from server => [{0}]", packetData));
//            this.dataSocket.BeginSend(packet, 0, packet.Length, 0, sendCallback, null);
//        }

//        /// <summary>
//        /// Same as sendData
//        /// </summary>
//        /// <param name="iAr">The a-synchronious interface</param>
//        private void sentData(IAsyncResult iAr)
//        {
//            try
//            {
//                dataSocket.EndSend(iAr);
//            }
//            catch
//            {
//                //Console.WriteLine("0x004: Socket send"); disconnect();
//                disconnect();
//            }
//        }

//        private void LogMessage(string message)
//        {
//            try
//            {
//                FileStream errWriter = new System.IO.FileStream("packetlog.txt", System.IO.FileMode.Append, System.IO.FileAccess.Write);
//                byte[] Msg = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + message);
//                errWriter.Write(Msg, 0, Msg.Length);
//                errWriter.Dispose();
//            }
//            catch
//            {
//                Console.WriteLine("UNABLE TO WRITE TO LOGFILE");
//            }
//        }
//        #endregion

//        #endregion

//        internal void SendMessage(ServerMessage Message)
//        {
//            if (Message == null)
//                return;
//            this.SendData(Message.GetBytes());
//        }
//    }
//}
