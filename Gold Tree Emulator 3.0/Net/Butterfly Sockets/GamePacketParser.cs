//using GoldTree.Messages;
//using GoldTree.Util;
//using SharedPacketLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GoldTree.Net
//{
//    public class GamePacketParser : IDataParser
//    {
//        private ConnectionInformation con;

//        public delegate void HandlePacket(ref byte[] Data);
//        public event HandlePacket onNewPacket;

//        public void SetConnection(ConnectionInformation con)
//        {
//            this.con = con;
//            this.onNewPacket = null;
//        }

//        public void handlePacketData(byte[] data)
//        {
//            if (data != null)
//            {
//                onNewPacket.Invoke(ref data);
//            }
//        }

//        public void Dispose()
//        {
//            this.onNewPacket = null;
//        }

//        public object Clone()
//        {
//            return new GamePacketParser();
//        }
//    }
//}
