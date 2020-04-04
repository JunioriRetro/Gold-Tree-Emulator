ALTER TABLE  `users` ADD  `friend_stream_enabled` ENUM(  '0',  '1' ) NOT NULL DEFAULT  '0';

CREATE TABLE IF NOT EXISTS `friend_stream` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `type` int(1) NOT NULL,
  `userid` int(11) unsigned NOT NULL,
  `gender` enum('M','F') NOT NULL DEFAULT 'M',
  `look` varchar(255) NOT NULL DEFAULT 'hr-115-42.hd-190-1.ch-215-62.lg-285-91.sh-290-62	',
  `time` double NOT NULL DEFAULT '0',
  `data` varchar(255) NOT NULL DEFAULT '',
  `data_extra` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;