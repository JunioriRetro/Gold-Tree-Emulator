using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Catalog
{
    internal sealed class GetGiftWrappingConfigurationEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage Message = new ServerMessage(620u);
            Message.AppendInt32(1);
            Message.AppendInt32(1);
            Message.AppendInt32(10);
            for (int i = 187; i < 191; i++)
            {
                Message.AppendInt32(i);
            }
            Message.AppendInt32(7);
            Message.AppendInt32(0);
            Message.AppendInt32(1);
            Message.AppendInt32(2);
            Message.AppendInt32(3);
            Message.AppendInt32(4);
            Message.AppendInt32(5);
            Message.AppendInt32(6);
            Message.AppendInt32(11);
            Message.AppendInt32(0);
            Message.AppendInt32(1);
            Message.AppendInt32(2);
            Message.AppendInt32(3);
            Message.AppendInt32(4);
            Message.AppendInt32(5);
            Message.AppendInt32(6);
            Message.AppendInt32(7);
            Message.AppendInt32(8);
            Message.AppendInt32(9);
            Message.AppendInt32(10);
            Message.AppendInt32(1);
            Session.SendMessage(Message);
        }
    }
}