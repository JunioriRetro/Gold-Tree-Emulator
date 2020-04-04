SET FOREIGN_KEY_CHECKS=0;

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