/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : darkgod

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2021-12-06 23:28:08
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `acct` varchar(255) NOT NULL,
  `pass` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `power` int(11) NOT NULL,
  `coin` int(11) NOT NULL,
  `diamond` int(11) NOT NULL,
  `crystal` int(11) NOT NULL,
  `hp` int(11) NOT NULL,
  `ad` int(11) NOT NULL,
  `ap` int(11) NOT NULL,
  `addef` int(11) NOT NULL,
  `apdef` int(11) NOT NULL,
  `dodge` int(11) NOT NULL,
  `pierce` int(11) NOT NULL,
  `critical` int(11) NOT NULL,
  `guideid` int(11) NOT NULL,
  `strong` varchar(255) NOT NULL,
  `time` bigint(11) NOT NULL,
  `task` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('9', '2', '111', '晏婉', '1', '0', '150', '49850', '0', '495', '2020', '300', '290', '85', '61', '7', '5', '2', '1001', '1#0#0#0#0#0#', '1638800602124', '1|0|0#2|0|0#3|0|0#4|0|0#5|0|0#6|0|0#');
INSERT INTO `account` VALUES ('10', '54488', '111', '项盈', '3', '800', '150', '51270', '0', '495', '2020', '300', '290', '85', '61', '7', '5', '2', '1003', '1#0#0#0#0#0#', '1638804156216', '1|1|1#2|0|0#3|1|0#4|0|0#5|0|0#6|0|0#');
