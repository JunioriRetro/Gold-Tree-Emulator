using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Pets;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.RoomBots;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Pets
{
	internal sealed class PlacePetMessageEvent : Interface
	{
		public void Handle(GameClient session, ClientMessage message)
		{
			Room room = GoldTree.GetGame().GetRoomManager().GetRoom(session.GetHabbo().CurrentRoomId);

            if (room != null && (room.AllowPet || room.CheckRights(session, true)))
			{
				uint petId = message.PopWiredUInt();

				Pet pet = session.GetHabbo().GetInventoryComponent().GetPetById(petId);

				if (pet != null && !pet.PlacedInRoom)
				{
					int num = message.PopWiredInt32();
					int num2 = message.PopWiredInt32();

					if (room.method_30(num, num2, 0.0, true, false))
					{
						if (room.PetCount >= ServerConfiguration.PetsPerRoomLimit)
						{
							session.SendNotification(GoldTreeEnvironment.GetExternalText("error_maxpets") + ServerConfiguration.PetsPerRoomLimit);
						}
						else
						{
							pet.PlacedInRoom = true;
							pet.RoomId = room.Id;

							List<RandomSpeech> list = new List<RandomSpeech>();
							List<BotResponse> list2 = new List<BotResponse>();

							room.method_4(new RoomBot(pet.PetId, pet.RoomId, AIType.const_0, "freeroam", pet.Name, "", pet.Look, num, num2, 0, 0, 0, 0, 0, 0, ref list, ref list2, 0), pet);
                            
                            if (room.CheckRights(session, true))
                                session.GetHabbo().GetInventoryComponent().RemovePetById(pet.PetId);
						}
					}
				}
			}
		}
	}
}
