//using System;
//using System.Collections;
//using GoldTree.Net.LoggingSystem;

//namespace GoldTree.Net.Messages
//{
//    class MessageLoggerManager
//    {
//        private static Queue loggedMessages;
//        private static bool enabled;
//        private static DateTime timeSinceLastPacket;

//        internal static bool Enabled
//        {
//            get
//            {
//                return enabled;
//            }
//            set
//            {
//                enabled = value;
//                if (enabled)
//                {
//                    loggedMessages = new Queue();
//                }
//            }
//        }

//        internal static void AddMessage(byte[] data, int connectionID, LogState state)
//        {
//            if (!enabled)
//                return;

//            string message;
//            switch (state)
//            {
//                case LogState.ConnectionClose:
//                    {
//                        message = "CONOPEN";
//                        break;
//                    }

//                case LogState.ConnectionOpen:
//                    {
//                        message = "CONCLOSE";
//                        break;
//                    }

//                default:
//                    {
//                        message = System.Text.Encoding.Default.GetString(data);
//                        break;
//                    }
//            }

//            lock (loggedMessages.SyncRoot)
//            {
//                Message loggedMessage = new Message(connectionID, GenerateTimestamp(), message);
//                loggedMessages.Enqueue(loggedMessage);
//            }
//        }

//        private static int GenerateTimestamp()
//        {
//            DateTime now = DateTime.Now;
//            TimeSpan timeSpent = now - timeSinceLastPacket;
//            timeSinceLastPacket = now;

//            return (int)timeSpent.TotalMilliseconds;
//        }
//    }
//}
