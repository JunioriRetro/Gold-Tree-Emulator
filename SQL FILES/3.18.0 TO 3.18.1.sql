CREATE TABLE IF NOT EXISTS `items_jukebox_songs` (
  `itemid` int(10) unsigned NOT NULL,
  `jukeboxid` int(10) unsigned NOT NULL,
  `songid` int(11) NOT NULL,
  `baseitem` int(11) NOT NULL,
  PRIMARY KEY (`itemid`,`jukeboxid`),
  KEY `itemid` (`itemid`),
  KEY `jukeboxid` (`jukeboxid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;