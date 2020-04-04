CREATE TABLE IF NOT EXISTS `friend_stream_likes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `friend_stream_id` int(11) NOT NULL,
  `userid` int(11) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=latin1;
