//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GoldTree.Net
//{
//    class ConnectionHandeling
//    {
//        private SocketManager manager;
//        private Hashtable liveConnections;

//        public ConnectionHandeling(int port, int maxConnections, int connectionsPerIP, bool enabeNagles)
//        {
//            liveConnections = new Hashtable();
//            manager = new SocketManager();
//            manager.init(port, maxConnections, connectionsPerIP, new InitialPacketParser(), !enabeNagles);
//        }

//        internal void init()
//        {
//            manager.connectionEvent += new SocketManager.ConnectionEvent(manager_connectionEvent);
//        }

//        private void manager_connectionEvent(ConnectionInformation connection)
//        {
//            liveConnections.Add(connection.getConnectionID(), connection);
//            connection.connectionChanged += connectionChanged;
//            GoldTree.GetGame().GetClientManager().CreateAndStartClient((uint)connection.getConnectionID(), connection);

//        }

//        private void connectionChanged(ConnectionInformation information, ConnectionState state)
//        {
//            if (state == ConnectionState.closed)
//            {
//                CloseConnection(information.getConnectionID());
//                liveConnections.Remove(information.getConnectionID());
//            }
//        }

//        internal void Start()
//        {
//            manager.initializeConnectionRequests();
//        }

//        internal void CloseConnection(int p)
//        {
//            try
//            {
//                Object info = liveConnections[p];
//                if (info != null)
//                {
//                    ConnectionInformation con = (ConnectionInformation)info;
//                    con.Dispose();
//                    GoldTree.GetGame().GetClientManager().method_9((uint)p);
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        internal void Destroy()
//        {
//            manager.destroy();
//        }
//    }
//}
