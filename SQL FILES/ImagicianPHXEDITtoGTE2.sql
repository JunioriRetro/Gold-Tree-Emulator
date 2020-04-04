INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_vipha_name', 'vipha');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_vipha_title', 'VIP Alert');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_viphal_name', 'viphal');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_viphal_title', 'VIP Alert');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_spush_name', 'spush');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_roomeffect_name', 'roomeffect');

ALTER TABLE `items` ADD `fw_count` INT(10) NOT NULL DEFAULT '0';

ALTER TABLE `users` ADD `seckey` VARCHAR(999) NOT NULL DEFAULT '0';
ALTER TABLE `users` ADD `last_loggedin` VARCHAR(50) NOT NULL DEFAULT '0';
ALTER TABLE `users` ADD `vipha_last` DOUBLE NOT NULL DEFAULT '0';
ALTER TABLE `users` ADD `viphal_last` DOUBLE NOT NULL DEFAULT '0';
ALTER TABLE  `users` ADD  `friend_stream_enabled` ENUM(  '0',  '1' ) NOT NULL DEFAULT  '0';

ALTER TABLE `user_stats` ADD `fireworks` INT(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `RegularVisitor` INT(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `FootballGoalScorer` INT(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `FootballGoalHost` INT(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `TilesLocked` INT(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `staff_picks` INT(10) NOT NULL DEFAULT '0';
ALTER TABLE  `user_stats` ADD  `lev_custom1` INT( 10 ) NOT NULL DEFAULT  '0' AFTER  `lev_explore` ;

ALTER TABLE `furniture` CHANGE `interaction_type` `interaction_type` ENUM('default','gate','postit','roomeffect','dimmer','trophy','bed','scoreboard','vendingmachine','alert','onewaygate','loveshuffler','habbowheel','dice','bottle','teleport','rentals','pet','roller','water','ball','bb_red_gate','bb_green_gate','bb_yellow_gate','bb_puck','bb_blue_gate','bb_patch','bb_teleport','blue_score','green_score','red_score','yellow_score','fbgate','tagpole','counter','red_goal','blue_goal','yellow_goal','green_goal','wired','wf_trg_onsay','wf_act_saymsg','wf_trg_enterroom','wf_act_moveuser','wf_act_togglefurni','wf_trg_furnistate','wf_trg_onfurni','pressure_pad','wf_trg_offfurni','wf_trg_gameend','wf_trg_gamestart','wf_trg_timer','wf_act_givepoints','wf_trg_attime','wf_trg_atscore','wf_act_moverotate','rollerskate','stickiepole','wf_xtra_random','wf_cnd_trggrer_on_frn','wf_cnd_furnis_hv_avtrs','wf_act_matchfurni','wf_cnd_has_furni_on','puzzlebox','switch','wf_act_give_phx','wf_cnd_phx','firework','wf_act_kick_user','hopper','jukebox','musicdisc','freeze_tile','freeze_counter','freeze_ice_block','freeze_blue_gate','freeze_red_gate','freeze_green_gate','freeze_yellow_gate','freeze_exit','freeze_blue_score','freeze_red_score','freeze_green_score','freeze_yellow_score','wf_act_reset_timers','wf_cnd_match_snapshot','wf_cnd_time_more_than','wf_cnd_time_less_than') CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT 'default';
ALTER TABLE `furniture` ADD `HeightOverride` ENUM('1', '0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_ranks` ADD `wired_give_wiredactived` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `wired_cnd_wiredactived` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `wired_unlimitedselects` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_push` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_pull` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_flagme` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_mimic` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_moonwalk` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_follow` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_vipha` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `acc_moveotheruserstodoor` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_viphal` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_spush` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_roomeffect` ENUM('1', '0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_users` ADD `wired_give_wiredactived` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `wired_cnd_wiredactived` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `wired_unlimitedselects` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_push` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_pull` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_flagme` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_mimic` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_moonwalk` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_follow` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_vipha` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `acc_moveotheruserstodoor` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_viphal` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_spush` ENUM('1', '0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_roomeffect` ENUM('1', '0') NOT NULL DEFAULT '0';

ALTER TABLE `server_settings` ADD `StaffPicksCategoryID` int(11) NOT NULL DEFAULT '8';
ALTER TABLE `server_settings` ADD `vipha_interval` DOUBLE NOT NULL DEFAULT '1800';
ALTER TABLE `server_settings` ADD `viphal_interval` DOUBLE NOT NULL DEFAULT '3600';
ALTER TABLE `server_settings` ADD `DisableOtherUsersToMovingOtherUsersToDoor` ENUM('1', '0') NOT NULL DEFAULT '0';

ALTER TABLE `catalog_items` ADD `song_id` INT(11) NOT NULL;
ALTER TABLE `catalog_items` ADD `BadgeID` VARCHAR(100) NOT NULL DEFAULT '';

INSERT INTO `navigator_publics` (`id` ,`ordernum` ,`bannertype` ,`caption` ,`image` ,`image_type` ,`room_id` ,`category` ,`category_parent_id`) VALUES (NULL ,  '0',  '0',  'Recommended by staff',  'officialrooms_hq/staffpickfolder.gif',  'external',  '0',  '1',  '0');

CREATE TABLE IF NOT EXISTS `user_wiredactived` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `wiredid` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

DROP TABLE IF EXISTS `achievements`;

CREATE TABLE IF NOT EXISTS `achievements` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `type` varchar(100) NOT NULL DEFAULT '',
  `levels` int(11) NOT NULL DEFAULT '1',
  `dynamic_badgelevel` enum('0','1') NOT NULL DEFAULT '1',
  `badge` varchar(100) NOT NULL,
  `pixels_base` int(5) NOT NULL DEFAULT '50',
  `score_base` int(5) NOT NULL DEFAULT '10',
  `pixels_multiplier` double NOT NULL DEFAULT '1.25',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=cp1251 AUTO_INCREMENT=22 ;

INSERT INTO `achievements` (`id`, `type`, `levels`, `dynamic_badgelevel`, `badge`, `pixels_base`, `score_base`, `pixels_multiplier`) VALUES
(1, 'identity', 1, '0', 'ACH_AvatarLooks1', 50, 10, 1),
(2, 'identity', 1, '0', 'ACH_Motto1', 50, 10, 1),
(3, 'identity', 1, '0', 'ACH_Student1', 50, 10, 1),
(4, 'social', 20, '1', 'ACH_RespectGiven', 50, 10, 1),
(5, 'identity', 1, '0', 'ACH_Name1', 50, 10, 1),
(6, 'social', 10, '1', 'ACH_RespectEarned', 50, 10, 1),
(7, 'identity', 1, '0', 'ACH_AvatarTags1', 50, 10, 1),
(8, 'explore', 20, '1', 'ACH_RoomEntry', 50, 10, 1),
(9, 'identity', 1, '0', 'ACH_HappyHour1', 50, 10, 1),
(10, 'social', 15, '1', 'ACH_GiftGiver', 50, 10, 1),
(11, 'social', 10, '1', 'ACH_GiftReceiver', 50, 10, 1),
(13, 'social', 10, '1', 'ACH_FireworksCharger', 50, 10, 1),
(14, 'pets', 10, '1', 'ACH_PetLover', 50, 10, 1),
(15, 'identity', 20, '1', 'ACH_AllTimeHotelPresence', 50, 10, 1),
(16, 'identity', 5, '1', 'ACH_VipHC', 50, 10, 1),
(17, 'identity', 20, '1', 'ACH_RegistrationDuration', 50, 10, 1),
(18, 'identity', 20, '1', 'ACH_Login', 50, 10, 1),
(19, 'games', 5, '1', 'ACH_FootballGoalScored', 50, 10, 1),
(20, 'games', 5, '1', 'ACH_FootballGoalScoredInRoom', 50, 10, 1),
(21, 'games', 20, '1', 'ACH_BattleBallTilesLocked', 50, 10, 1),
(22, 'room_builder', 10, '1', 'ACH_Spr', 50, 10, 1);

DROP TABLE IF EXISTS `room_poll_answers`;
CREATE TABLE `room_poll_answers` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `question_id` int(11) NOT NULL DEFAULT '0',
  `answer` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `room_poll_questions`;
CREATE TABLE `room_poll_questions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `poll_id` int(11) NOT NULL DEFAULT '0',
  `title` text NOT NULL,
  `type` enum('textbox','checkbox','radio') NOT NULL DEFAULT 'textbox',
  `min_selects` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `room_poll_results`;
CREATE TABLE `room_poll_results` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `poll_id` int(11) NOT NULL,
  `question_id` int(11) NOT NULL,
  `answer_text` text NOT NULL,
  `user_id` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `room_polls`;
CREATE TABLE `room_polls` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `room_id` int(11) NOT NULL DEFAULT '0',
  `title` text NOT NULL,
  `thanks` text NOT NULL,
  `details` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `infobus_answers`;
CREATE TABLE `infobus_answers` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `question_id` int(11) NOT NULL DEFAULT '0',
  `answer_text` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `infobus_questions`;
CREATE TABLE `infobus_questions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `question` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `infobus_results`;
CREATE TABLE `infobus_results` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `question_id` int(11) NOT NULL,
  `answer_id` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `items_rooms_songs` (
  `itemid` int(10) unsigned NOT NULL,
  `roomid` int(10) unsigned NOT NULL,
  `songid` int(11) NOT NULL,
  `baseitem` int(11) NOT NULL,
  PRIMARY KEY (`itemid`,`roomid`),
  KEY `itemid` (`itemid`),
  KEY `roomid` (`roomid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `friend_stream` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `type` int(1) NOT NULL,
  `userid` int(11) unsigned NOT NULL,
  `gender` enum('M','F') NOT NULL DEFAULT 'M',
  `look` varchar(255) NOT NULL DEFAULT 'hr-115-42.hd-190-1.ch-215-62.lg-285-91.sh-290-62	',
  `time` double NOT NULL DEFAULT '0',
  `data` varchar(255) NOT NULL NULL DEFAULT '',
  `data_extra` varchar(255) NOT NULL NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

CREATE TABLE IF NOT EXISTS `items_jukebox_songs` (
  `itemid` int(10) unsigned NOT NULL,
  `jukeboxid` int(10) unsigned NOT NULL,
  `songid` int(11) NOT NULL,
  `baseitem` int(11) NOT NULL,
  PRIMARY KEY (`itemid`,`jukeboxid`),
  KEY `itemid` (`itemid`),
  KEY `jukeboxid` (`jukeboxid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `items_extra_data` (
  `item_id` int(10) unsigned NOT NULL,
  `extra_data` text NOT NULL,
  PRIMARY KEY (`item_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `items_firework` (
  `item_id` int(10) unsigned NOT NULL,
  `fw_count` int(10) NOT NULL,
  PRIMARY KEY (`item_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `friend_stream_likes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `friend_stream_id` int(11) NOT NULL,
  `userid` int(11) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=latin1;

TRUNCATE user_achievements;