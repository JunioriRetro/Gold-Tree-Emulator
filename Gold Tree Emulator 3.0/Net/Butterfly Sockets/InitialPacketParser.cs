//using SharedPacketLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GoldTree.Net
//{
//    class InitialPacketParser : IDataParser
//    {
//        public delegate void NoParamDelegate();
//        public event NoParamDelegate PolicyRequest;
//        public event NoParamDelegate SwitchParserRequest;
//        public byte[] currentData;

//        public void handlePacketData(byte[] packet)
//        {
//            if (packet[0] == 60 && PolicyRequest != null)
//            {
//                PolicyRequest.Invoke();
//            }
//            else if(packet[0] != 67  && SwitchParserRequest != null)
//            {
//                currentData = packet;
//                SwitchParserRequest.Invoke();
//            }
//        }

//        public void Dispose()
//        {
//            this.PolicyRequest = null;
//            this.SwitchParserRequest = null;
//        }

//        public object Clone()
//        {
//            return new InitialPacketParser();
//        }
//    }
//}
