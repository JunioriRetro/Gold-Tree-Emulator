using System;
using System.Collections.Generic;
using GoldTree.Communication.Messages.Avatar;
using GoldTree.Communication.Messages.Wired;
using GoldTree.Communication.Messages.Sound;
using GoldTree.Communication.Messages.Register;
using GoldTree.Communication.Messages.Inventory.Badges;
using GoldTree.Communication.Messages.Recycler;
using GoldTree.Communication.Messages.Users;
using GoldTree.Communication.Messages.Inventory.Trading;
using GoldTree.Communication.Messages.Help;
using GoldTree.Communication.Messages.Rooms.Action;
using GoldTree.Communication.Messages.Rooms.Furniture;
using GoldTree.Communication.Messages.Rooms.Avatar;
using GoldTree.Communication.Messages.Rooms.Chat;
using GoldTree.Communication.Messages.Rooms.Engine;
using GoldTree.Communication.Messages.Rooms.Pets;
using GoldTree.Communication.Messages.Rooms.Session;
using GoldTree.Communication.Messages.Rooms.Settings;
using GoldTree.Communication.Messages.Navigator;
using GoldTree.Communication.Messages.Handshake;
using GoldTree.Communication.Messages.Messenger;
using GoldTree.Communication.Messages.Catalog;
using GoldTree.Communication.Messages.Marketplace;
using GoldTree.Communication.Messages.Inventory.AvatarFX;
using GoldTree.Communication.Messages.Inventory.Furni;
using GoldTree.Communication.Messages.Inventory.Purse;
using GoldTree.Communication.Messages.Inventory.Achievements;
using GoldTree.Communication.Messages.Quest;
using GoldTree.Communication.Messages.Rooms.Polls;
using GoldTree.Communication.Messages.SoundMachine;
using GoldTree.Communication.Messages.FriendStream;
namespace GoldTree.Communication
{
	internal sealed class PacketManager
	{
		private Dictionary<uint, Interface> RequestHandlers;
		public PacketManager()
		{
			this.RequestHandlers = new Dictionary<uint, Interface>();
		}
		public bool Handle(uint PacketId, out Interface Event)
		{
			if (this.RequestHandlers.ContainsKey(PacketId))
			{
				Event = this.RequestHandlers[PacketId];
				return true;
			}
			else
			{
				Event = null;
				return false;
			}
		}

        /*
         * Cleaned up by Leon
         */

		public void Handshake()
		{
			this.RequestHandlers.Add(512, new DisconnectMessageEvent());
			this.RequestHandlers.Add(2002, new GenerateSecretKeyMessageEvent());
			this.RequestHandlers.Add(1817, new GetSessionParametersMessageEvent());
			this.RequestHandlers.Add(7, new InfoRetrieveMessageEvent());
			this.RequestHandlers.Add(206, new InitCryptoMessageEvent());
			this.RequestHandlers.Add(196, new PongMessageEvent());
			this.RequestHandlers.Add(415, new SSOTicketMessageEvent());
			this.RequestHandlers.Add(756, new TryLoginMessageEvent());
			this.RequestHandlers.Add(813, new UniqueIDMessageEvent());
			this.RequestHandlers.Add(1170, new VersionCheckMessageEvent());
		}
		public void Messenger()
		{
			this.RequestHandlers.Add(37, new AcceptBuddyMessageEvent());
			this.RequestHandlers.Add(38, new DeclineBuddyMessageEvent());
			this.RequestHandlers.Add(262, new FollowFriendMessageEvent());
			this.RequestHandlers.Add(15, new FriendsListUpdateEvent());
			this.RequestHandlers.Add(233, new GetBuddyRequestsMessageEvent());
			this.RequestHandlers.Add(41, new HabboSearchMessageEvent());
			this.RequestHandlers.Add(12, new MessengerInitMessageEvent());
			this.RequestHandlers.Add(40, new RemoveBuddyMessageEvent());
			this.RequestHandlers.Add(39, new RequestBuddyMessageEvent());
			this.RequestHandlers.Add(33, new SendMsgMessageEvent());
			this.RequestHandlers.Add(34, new SendRoomInviteMessageEvent());
			this.RequestHandlers.Add(490, new FindNewFriendsMessageEvent());
		}
		public void Navigator()
		{
			this.RequestHandlers.Add(19, new AddFavouriteRoomMessageEvent());
			this.RequestHandlers.Add(347, new CancelEventMessageEvent());
			this.RequestHandlers.Add(345, new CanCreateEventMessageEvent());
			this.RequestHandlers.Add(387, new CanCreateRoomMessageEvent());
			this.RequestHandlers.Add(346, new CreateEventMessageEvent());
			this.RequestHandlers.Add(29, new CreateFlatMessageEvent());
			this.RequestHandlers.Add(20, new DeleteFavouriteRoomMessageEvent());
			this.RequestHandlers.Add(348, new EditEventMessageEvent());
			this.RequestHandlers.Add(385, new GetGuestRoomMessageEvent());
			this.RequestHandlers.Add(380, new GetOfficialRoomsMessageEvent());
			this.RequestHandlers.Add(382, new GetPopularRoomTagsMessageEvent());
			this.RequestHandlers.Add(388, new GetPublicSpaceCastLibsMessageEvent());
			this.RequestHandlers.Add(151, new GetUserFlatCatsMessageEvent());
			this.RequestHandlers.Add(439, new LatestEventsSearchMessageEvent());
			this.RequestHandlers.Add(435, new MyFavouriteRoomsSearchMessageEvent());
			this.RequestHandlers.Add(432, new MyFriendsRoomsSearchMessageEvent());
			this.RequestHandlers.Add(436, new MyRoomHistorySearchMessageEvent());
			this.RequestHandlers.Add(434, new MyRoomsSearchMessageEvent());
			this.RequestHandlers.Add(430, new PopularRoomsSearchMessageEvent());
			this.RequestHandlers.Add(261, new RateFlatMessageEvent());
			this.RequestHandlers.Add(433, new RoomsWhereMyFriendsAreSearchMessageEvent());
			this.RequestHandlers.Add(431, new RoomsWithHighestScoreSearchMessageEvent());
			this.RequestHandlers.Add(438, new RoomTagSearchMessageEvent());
			this.RequestHandlers.Add(437, new RoomTextSearchMessageEvent());
			this.RequestHandlers.Add(483, new ToggleStaffPickMessageEvent());
			this.RequestHandlers.Add(384, new UpdateNavigatorSettingsMessageEvent());
			this.RequestHandlers.Add(386, new UpdateRoomThumbnailMessageEvent());
		}
		public void RoomsAction()
		{
			this.RequestHandlers.Add(440, new CallGuideBotMessageEvent());
			this.RequestHandlers.Add(441, new KickBotMessageEvent());
			this.RequestHandlers.Add(96, new AssignRightsMessageEvent());
			this.RequestHandlers.Add(97, new RemoveRightsMessageEvent());
			this.RequestHandlers.Add(155, new RemoveAllRightsMessageEvent());
			this.RequestHandlers.Add(95, new KickUserMessageEvent());
			this.RequestHandlers.Add(320, new BanUserMessageEvent());
			this.RequestHandlers.Add(98, new LetUserInMessageEvent());
		}
		public void RoomsAvatar()
		{
			this.RequestHandlers.Add(94, new WaveMessageEvent());
			this.RequestHandlers.Add(93, new DanceMessageEvent());
			this.RequestHandlers.Add(79, new LookToMessageEvent());
			this.RequestHandlers.Add(471, new ChangeUserNameMessageEvent());
			this.RequestHandlers.Add(470, new ChangeUserNameMessageEvent());
		}
		public void RoomsChat()
		{
			this.RequestHandlers.Add(52, new ChatMessageEvent());
			this.RequestHandlers.Add(55, new ShoutMessageEvent());
			this.RequestHandlers.Add(56, new WhisperMessageEvent());
			this.RequestHandlers.Add(317, new StartTypingMessageEvent());
			this.RequestHandlers.Add(318, new CancelTypingMessageEvent());
		}
		public void RoomsEngine()
		{
			this.RequestHandlers.Add(480, new SetClothingChangeDataMessageEvent());
			this.RequestHandlers.Add(215, new GetFurnitureAliasesMessageEvent());
			this.RequestHandlers.Add(390, new GetRoomEntryDataMessageEvent());
			this.RequestHandlers.Add(67, new PickupObjectMessageEvent());
			this.RequestHandlers.Add(73, new MoveObjectMessageEvent());
			this.RequestHandlers.Add(91, new MoveWallItemMessageEvent());
			this.RequestHandlers.Add(90, new PlaceObjectMessageEvent());
			this.RequestHandlers.Add(83, new GetItemDataMessageEvent());
			this.RequestHandlers.Add(84, new SetItemDataMessageEvent());
			this.RequestHandlers.Add(85, new RemoveItemMessageEvent());
			this.RequestHandlers.Add(75, new MoveAvatarMessageEvent());
			this.RequestHandlers.Add(74, new SetObjectDataMessageEvent());
			this.RequestHandlers.Add(66, new ApplyRoomEffect());
			this.RequestHandlers.Add(182, new GetInterstitialMessageEvent());
		}
		public void RoomsFurniture()
		{
			this.RequestHandlers.Add(392, new UseFurnitureMessageEvent());
			this.RequestHandlers.Add(393, new UseFurnitureMessageEvent());
			this.RequestHandlers.Add(232, new UseFurnitureMessageEvent());
			this.RequestHandlers.Add(314, new UseFurnitureMessageEvent());
			this.RequestHandlers.Add(247, new UseFurnitureMessageEvent());
			this.RequestHandlers.Add(76, new UseFurnitureMessageEvent());
			this.RequestHandlers.Add(183, new CreditFurniRedeemMessageEvent());
			this.RequestHandlers.Add(78, new PresentOpenMessageEvent());
			this.RequestHandlers.Add(77, new DiceOffMessageEvent());
			this.RequestHandlers.Add(341, new RoomDimmerGetPresetsMessageEvent());
			this.RequestHandlers.Add(342, new RoomDimmerSavePresetMessageEvent());
			this.RequestHandlers.Add(343, new RoomDimmerChangeStateMessageEvent());
			this.RequestHandlers.Add(3254, new PlacePostItMessageEvent());
		}
		public void RoomsPets()
		{
			this.RequestHandlers.Add(3001, new GetPetInfoMessageEvent());
			this.RequestHandlers.Add(3002, new PlacePetMessageEvent());
			this.RequestHandlers.Add(3003, new RemovePetFromFlatMessageEvent());
			this.RequestHandlers.Add(3004, new GetPetCommandsMessageEvent());
			this.RequestHandlers.Add(3005, new RespectPetMessageEvent());
		}
        public void RoomsPools()
        {
            this.RequestHandlers.Add(234, new ShowRoomPoll());
            this.RequestHandlers.Add(236, new GetRoomPollAnswers());
            this.RequestHandlers.Add(112, new AnswerInfobusPoll());
        }
		public void RoomsSession()
		{
			this.RequestHandlers.Add(53, new QuitMessageEvent());
			this.RequestHandlers.Add(391, new OpenFlatConnectionMessageEvent());
			this.RequestHandlers.Add(2, new OpenConnectionMessageEvent());
			this.RequestHandlers.Add(59, new GoToFlatMessageEvent());
		}
		public void RoomsSettings()
		{
			this.RequestHandlers.Add(400, new GetRoomSettingsMessageEvent());
			this.RequestHandlers.Add(401, new SaveRoomSettingsMessageEvent());
			this.RequestHandlers.Add(23, new DeleteRoomMessageEvent());
		}
		public void Catalog()
		{
			this.RequestHandlers.Add(101, new GetCatalogIndexEvent());
			this.RequestHandlers.Add(102, new GetCatalogPageEvent());
			this.RequestHandlers.Add(3031, new GetClubOffersMessageEvent());
			this.RequestHandlers.Add(473, new GetGiftWrappingConfigurationEvent());
			this.RequestHandlers.Add(3038, new GetHabboBasicMembershipExtendOfferEvent());
			this.RequestHandlers.Add(3035, new GetHabboClubExtendOfferMessageEvent());
			this.RequestHandlers.Add(3030, new GetIsOfferGiftableEvent());
			this.RequestHandlers.Add(3007, new GetSellablePetBreedsEvent());
			this.RequestHandlers.Add(3034, new MarkCatalogNewAdditionsPageOpenedEvent());
			this.RequestHandlers.Add(3037, new PurchaseBasicMembershipExtensionEvent());
			this.RequestHandlers.Add(472, new PurchaseFromCatalogAsGiftEvent());
			this.RequestHandlers.Add(100, new PurchaseFromCatalogEvent());
			this.RequestHandlers.Add(3036, new PurchaseVipMembershipExtensionEvent());
			this.RequestHandlers.Add(129, new RedeemVoucherMessageEvent());
			this.RequestHandlers.Add(475, new SelectClubGiftEvent());
		}
		public void Marketplace()
		{
			this.RequestHandlers.Add(3013, new BuyMarketplaceTokensMessageEvent());
			this.RequestHandlers.Add(3014, new BuyOfferMessageEvent());
			this.RequestHandlers.Add(3015, new CancelOfferMessageEvent());
			this.RequestHandlers.Add(3012, new GetMarketplaceCanMakeOfferEvent());
			this.RequestHandlers.Add(3020, new GetMarketplaceItemStatsEvent());
			this.RequestHandlers.Add(3018, new GetOffersMessageEvent());
			this.RequestHandlers.Add(3019, new GetOwnOffersMessageEvent());
			this.RequestHandlers.Add(3010, new MakeOfferMessageEvent());
			this.RequestHandlers.Add(3016, new RedeemOfferCreditsMessageEvent());
			this.RequestHandlers.Add(3011, new GetMarketplaceConfigurationMessageEvent());
		}
		public void Recycler()
		{
			this.RequestHandlers.Add(412, new GetRecyclerPrizesMessageEvent());
			this.RequestHandlers.Add(413, new GetRecyclerStatusMessageEvent());
			this.RequestHandlers.Add(414, new RecycleItemsMessageEvent());
		}
		public void Quest()
		{
			this.RequestHandlers.Add(3102, new AcceptQuestMessageEvent());
			this.RequestHandlers.Add(3101, new GetQuestsMessageEvent());
			this.RequestHandlers.Add(3107, new OpenQuestTrackerMessageEvent());
			this.RequestHandlers.Add(3106, new RejectQuestMessageEvent());
		}
		public void InventoryPurse()
		{
			this.RequestHandlers.Add(8, new GetCreditsInfoEvent());
		}
		public void InventoryFurni()
		{
			this.RequestHandlers.Add(3000, new GetPetInventoryEvent());
			this.RequestHandlers.Add(404, new RequestFurniInventoryEvent());
		}
		public void InventoryBadges()
		{
			this.RequestHandlers.Add(3032, new GetBadgePointLimitsEvent());
			this.RequestHandlers.Add(157, new GetBadgesEvent());
			this.RequestHandlers.Add(158, new SetActivatedBadgesEvent());
		}
		public void InventoryTrading()
		{
			this.RequestHandlers.Add(68, new UnacceptTradingEvent());
			this.RequestHandlers.Add(69, new AcceptTradingEvent());
			this.RequestHandlers.Add(71, new OpenTradingEvent());
			this.RequestHandlers.Add(72, new AddItemToTradeEvent());
			this.RequestHandlers.Add(402, new ConfirmAcceptTradingEvent());
			this.RequestHandlers.Add(403, new ConfirmDeclineTradingEvent());
			this.RequestHandlers.Add(70, new ConfirmDeclineTradingEvent());
			this.RequestHandlers.Add(405, new RemoveItemFromTradeEvent());
		}
		public void InventoryAvatarFX()
		{
			this.RequestHandlers.Add(372, new AvatarEffectSelectedEvent());
			this.RequestHandlers.Add(373, new AvatarEffectActivatedEvent());
		}
		public void InventoryAchievements()
		{
			this.RequestHandlers.Add(370, new GetAchievementsEvent());
		}
		public void Avatar()
		{
			this.RequestHandlers.Add(484, new ChangeMottoMessageEvent());
			this.RequestHandlers.Add(375, new GetWardrobeMessageEvent());
			this.RequestHandlers.Add(376, new SaveWardrobeOutfitMessageEvent());
		}
		public void Register()
		{
			this.RequestHandlers.Add(44, new UpdateFigureDataMessageEvent());
		}
		public void Users()
		{
			this.RequestHandlers.Add(26, new ScrGetUserInfoMessageEvent());
			this.RequestHandlers.Add(42, new ApproveNameMessageEvent());
			this.RequestHandlers.Add(263, new GetUserTagsMessageEvent());
			this.RequestHandlers.Add(159, new GetSelectedBadgesMessageEvent());
			this.RequestHandlers.Add(230, new GetHabboGroupBadgesMessageEvent());
			this.RequestHandlers.Add(231, new GetHabboGroupDetailsMessageEvent());
			this.RequestHandlers.Add(3260, new LoadUserGroupsEvent());
			this.RequestHandlers.Add(319, new IgnoreUserMessageEvent());
			this.RequestHandlers.Add(322, new UnignoreUserMessageEvent());
			this.RequestHandlers.Add(371, new RespectUserMessageEvent());
			this.RequestHandlers.Add(3257, new JoinGuildEvent());
			this.RequestHandlers.Add(3258, new GetGuildFavorite());
			this.RequestHandlers.Add(3259, new RemoveGuildFavorite());
		}
		public void Help()
		{
			this.RequestHandlers.Add(453, new CallForHelpMessageEvent());
			this.RequestHandlers.Add(452, new CloseIssuesMessageEvent());
			this.RequestHandlers.Add(238, new DeletePendingCallsForHelpMessageEvent());
			this.RequestHandlers.Add(457, new GetCfhChatlogMessageEvent());
			this.RequestHandlers.Add(416, new GetClientFaqsMessageEvent());
			this.RequestHandlers.Add(417, new GetFaqCategoriesMessageEvent());
			this.RequestHandlers.Add(420, new GetFaqCategoryMessageEvent());
			this.RequestHandlers.Add(418, new GetFaqTextMessageEvent());
			this.RequestHandlers.Add(459, new GetModeratorRoomInfoMessageEvent());
			this.RequestHandlers.Add(454, new GetModeratorUserInfoMessageEvent());
			this.RequestHandlers.Add(456, new GetRoomChatlogMessageEvent());
			this.RequestHandlers.Add(458, new GetRoomVisitsMessageEvent());
			this.RequestHandlers.Add(455, new GetUserChatlogMessageEvent());
			this.RequestHandlers.Add(461, new ModAlertMessageEvent());
			this.RequestHandlers.Add(464, new ModBanMessageEvent());
			this.RequestHandlers.Add(460, new ModerateRoomMessageEvent());
			this.RequestHandlers.Add(200, new ModeratorActionMessageEvent());
			this.RequestHandlers.Add(463, new ModKickMessageEvent());
			this.RequestHandlers.Add(462, new ModMessageMessageEvent());
			this.RequestHandlers.Add(450, new PickIssuesMessageEvent());
			this.RequestHandlers.Add(451, new ReleaseIssuesMessageEvent());
			this.RequestHandlers.Add(419, new SearchFaqsMessageEvent());
		}
		public void Sound()
		{
			this.RequestHandlers.Add(228, new GetSoundSettingsEvent());
			this.RequestHandlers.Add(229, new SetSoundSettingsEvent());
			this.RequestHandlers.Add(245, new GetSoundMachinePlayListMessageEvent());
			this.RequestHandlers.Add(221, new GetSongInfoMessageEvent());
			this.RequestHandlers.Add(249, new GetNowPlayingMessageEvent());
			this.RequestHandlers.Add(258, new GetJukeboxPlayListMessageEvent());
			this.RequestHandlers.Add(259, new GetUserSongDisksMessageEvent());
		}
        public void Jukebox()
        {
            this.RequestHandlers.Add(255, new AddNewJukeboxCD());
            this.RequestHandlers.Add(256, new RemoveCDToJukebox());
        }
		public void Wired()
		{
			this.RequestHandlers.Add(3050, new UpdateTriggerMessageEvent());
			this.RequestHandlers.Add(3051, new UpdateActionMessageEvent());
			this.RequestHandlers.Add(3052, new UpdateConditionMessageEvent());
            this.RequestHandlers.Add(3054, new ApplyFurniToSetConditions());
		}

        public void FriendStream()
        {
            this.RequestHandlers.Add(500, new GetEventStreamComposer());
            this.RequestHandlers.Add(501, new SetEventStreamingAllowedComposer());
            this.RequestHandlers.Add(502, new EventStreamingLikeButton());
        }

		public void Clear()
		{
			this.RequestHandlers.Clear();
		}
	}
}
