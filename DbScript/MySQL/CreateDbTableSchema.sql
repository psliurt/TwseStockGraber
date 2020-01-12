CREATE DATABASE  IF NOT EXISTS `tw_stock_history_warehouse` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `tw_stock_history_warehouse`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: tw_stock_history_warehouse
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bfi82u_daily`
--

DROP TABLE IF EXISTS `bfi82u_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfi82u_daily` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `unit_name` varchar(128) NOT NULL,
  `buy_money` decimal(20,3) DEFAULT NULL,
  `sell_money` decimal(20,3) DEFAULT NULL,
  `money_diff` decimal(20,3) DEFAULT NULL COMMENT '買賣差額',
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime DEFAULT NULL,
  `update_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`unit_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額統計表-日報表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfi82u_month`
--

DROP TABLE IF EXISTS `bfi82u_month`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfi82u_month` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_year` int(11) NOT NULL,
  `data_month` int(11) NOT NULL,
  `last_update` datetime NOT NULL,
  `unit_name` varchar(128) NOT NULL,
  `buy_money` decimal(20,3) DEFAULT NULL,
  `sell_money` decimal(20,3) DEFAULT NULL,
  `money_diff` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_year`,`data_month`,`last_update`,`unit_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額統計表-月報表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfi82u_week`
--

DROP TABLE IF EXISTS `bfi82u_week`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfi82u_week` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `week_start` datetime NOT NULL,
  `week_end` datetime NOT NULL,
  `last_update` datetime NOT NULL,
  `unit_name` varchar(128) NOT NULL,
  `buy_money` decimal(20,3) DEFAULT NULL,
  `sell_money` decimal(20,3) DEFAULT NULL,
  `money_diff` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime DEFAULT NULL,
  `update_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`week_start`,`week_end`,`unit_name`,`last_update`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額統計表-周報表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfi84u2`
--

DROP TABLE IF EXISTS `bfi84u2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfi84u2` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `stop_start` datetime DEFAULT NULL,
  `stop_end` datetime DEFAULT NULL,
  `reason` varchar(32) NOT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`stock_no`,`reason`,`stop_start`,`stop_end`) /*!80000 INVISIBLE */
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='停券歷史查詢';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfiamu`
--

DROP TABLE IF EXISTS `bfiamu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfiamu` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(64) NOT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_trade_cnt` int(11) DEFAULT NULL,
  `up_down_price` decimal(10,2) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime DEFAULT NULL,
  `update_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfiauu_daily`
--

DROP TABLE IF EXISTS `bfiauu_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfiauu_daily` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `type_class` varchar(16) NOT NULL COMMENT '類別',
  `trade_type` varchar(16) DEFAULT NULL COMMENT '交易別',
  `settle_type` varchar(16) DEFAULT NULL COMMENT '交割期別',
  `deal_cnt` bigint(20) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_stock_rate` decimal(20,3) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_money_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`type_class`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易日成交量值統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfiauu_monthly`
--

DROP TABLE IF EXISTS `bfiauu_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfiauu_monthly` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `deal_year` int(11) NOT NULL,
  `deal_month` int(11) NOT NULL,
  `trade_type` varchar(16) NOT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_stock_rate` decimal(20,3) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_money_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`deal_year`,`deal_month`,`trade_type`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易月成交量值統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfiauu_single`
--

DROP TABLE IF EXISTS `bfiauu_single`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfiauu_single` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `trade_type` varchar(16) DEFAULT NULL,
  `settle_period` varchar(32) DEFAULT NULL,
  `deal_price` decimal(20,3) DEFAULT NULL,
  `deal_qty` bigint(20) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易日成交資訊-單一證券';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfiauu_stock`
--

DROP TABLE IF EXISTS `bfiauu_stock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfiauu_stock` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `year` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `trade_date` datetime NOT NULL,
  `trade_type` varchar(32) DEFAULT NULL,
  `settle_period` varchar(32) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `high_price` decimal(10,3) DEFAULT NULL,
  `low_price` decimal(10,3) DEFAULT NULL,
  `weight_price` decimal(13,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`year`,`stock_no`,`trade_date`)
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股單一證券鉅額交易日成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bfiauu_yearly`
--

DROP TABLE IF EXISTS `bfiauu_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bfiauu_yearly` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `year` int(11) NOT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_stock_rate` decimal(20,3) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_money_rate` decimal(20,3) DEFAULT NULL,
  `note` varchar(256) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`year`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易年成交量值統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bft41u`
--

DROP TABLE IF EXISTS `bft41u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bft41u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_qty` bigint(20) DEFAULT NULL,
  `deal_trade_cnt` bigint(20) DEFAULT NULL,
  `deal_amount` decimal(20,3) DEFAULT NULL,
  `deal_price` decimal(10,0) DEFAULT NULL,
  `last_buy_qty` bigint(20) DEFAULT NULL,
  `last_sell_qty` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=9065 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='盤後定價交易';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bwibbu_daily`
--

DROP TABLE IF EXISTS `bwibbu_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `bwibbu_daily` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yield_rate` decimal(20,3) DEFAULT NULL,
  `dividend_yearly` int(11) DEFAULT NULL,
  `pe_ratio` decimal(20,3) DEFAULT NULL,
  `pb_ratio` decimal(20,3) DEFAULT NULL,
  `report_year` int(11) DEFAULT NULL,
  `report_season` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=933 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日本益比、殖利率及股價淨值比(依日期查詢)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `c_holiday`
--

DROP TABLE IF EXISTS `c_holiday`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `c_holiday` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `holiday_date` datetime NOT NULL,
  `holiday_name` varchar(32) DEFAULT NULL,
  `is_holiday` bit(1) NOT NULL,
  `holiday_category` varchar(64) DEFAULT NULL,
  `holiday_desc` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=959 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='國定假日表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrade_hedge_daily`
--

DROP TABLE IF EXISTS `d_3itrade_hedge_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrade_hedge_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` int(11) DEFAULT NULL,
  `foreign_sell_out` int(11) DEFAULT NULL,
  `foreign_diff` int(11) DEFAULT NULL,
  `foreign_self_buy_in` int(11) DEFAULT NULL,
  `foreign_self_sell_out` int(11) DEFAULT NULL,
  `foreign_self_diff` int(11) DEFAULT NULL,
  `foreign_all_buy_in` int(11) DEFAULT NULL,
  `foreign_all_sell_out` int(11) DEFAULT NULL,
  `foreign_all_diff` int(11) DEFAULT NULL,
  `invest_buy_in` int(11) DEFAULT NULL,
  `invest_sell_out` int(11) DEFAULT NULL,
  `invest_diff` int(11) DEFAULT NULL,
  `dealer_self_buy_in` int(11) DEFAULT NULL,
  `dealer_self_sell_out` int(11) DEFAULT NULL,
  `dealer_self_diff` int(11) DEFAULT NULL,
  `dealer_risk_buy_in` int(11) DEFAULT NULL,
  `dealer_risk_sell_out` int(11) DEFAULT NULL,
  `dealer_risk_diff` int(11) DEFAULT NULL,
  `dealer_all_buy_in` int(11) DEFAULT NULL,
  `dealer_all_sell_out` int(11) DEFAULT NULL,
  `dealer_all_diff` int(11) DEFAULT NULL,
  `total_diff` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=21365 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人日交易明細資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrade_hedge_monthly`
--

DROP TABLE IF EXISTS `d_3itrade_hedge_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrade_hedge_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` int(11) DEFAULT NULL,
  `foreign_sell_out` int(11) DEFAULT NULL,
  `foreign_diff` int(11) DEFAULT NULL,
  `foreign_self_buy_in` int(11) DEFAULT NULL,
  `foreign_self_sell_out` int(11) DEFAULT NULL,
  `foreign_self_diff` int(11) DEFAULT NULL,
  `foreign_all_buy_in` int(11) DEFAULT NULL,
  `foreign_all_sell_out` int(11) DEFAULT NULL,
  `foreign_all_diff` int(11) DEFAULT NULL,
  `invest_buy_in` int(11) DEFAULT NULL,
  `invest_sell_out` int(11) DEFAULT NULL,
  `invest_diff` int(11) DEFAULT NULL,
  `dealer_self_buy_in` int(11) DEFAULT NULL,
  `dealer_self_sell_out` int(11) DEFAULT NULL,
  `dealer_self_diff` int(11) DEFAULT NULL,
  `dealer_risk_buy_in` int(11) DEFAULT NULL,
  `dealer_risk_sell_out` int(11) DEFAULT NULL,
  `dealer_risk_diff` int(11) DEFAULT NULL,
  `dealer_all_buy_in` int(11) DEFAULT NULL,
  `dealer_all_sell_out` int(11) DEFAULT NULL,
  `dealer_all_diff` int(11) DEFAULT NULL,
  `total_diff` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人日交易明細資訊(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrade_hedge_weekly`
--

DROP TABLE IF EXISTS `d_3itrade_hedge_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrade_hedge_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` int(11) DEFAULT NULL,
  `foreign_sell_out` int(11) DEFAULT NULL,
  `foreign_diff` int(11) DEFAULT NULL,
  `foreign_self_buy_in` int(11) DEFAULT NULL,
  `foreign_self_sell_out` int(11) DEFAULT NULL,
  `foreign_self_diff` int(11) DEFAULT NULL,
  `foreign_all_buy_in` int(11) DEFAULT NULL,
  `foreign_all_sell_out` int(11) DEFAULT NULL,
  `foreign_all_diff` int(11) DEFAULT NULL,
  `invest_buy_in` int(11) DEFAULT NULL,
  `invest_sell_out` int(11) DEFAULT NULL,
  `invest_diff` int(11) DEFAULT NULL,
  `dealer_self_buy_in` int(11) DEFAULT NULL,
  `dealer_self_sell_out` int(11) DEFAULT NULL,
  `dealer_self_diff` int(11) DEFAULT NULL,
  `dealer_risk_buy_in` int(11) DEFAULT NULL,
  `dealer_risk_sell_out` int(11) DEFAULT NULL,
  `dealer_risk_diff` int(11) DEFAULT NULL,
  `dealer_all_buy_in` int(11) DEFAULT NULL,
  `dealer_all_sell_out` int(11) DEFAULT NULL,
  `dealer_all_diff` int(11) DEFAULT NULL,
  `total_diff` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=101664 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人日交易明細資訊(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrade_hedge_yearly`
--

DROP TABLE IF EXISTS `d_3itrade_hedge_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrade_hedge_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` int(11) DEFAULT NULL,
  `foreign_sell_out` int(11) DEFAULT NULL,
  `foreign_diff` int(11) DEFAULT NULL,
  `foreign_self_buy_in` int(11) DEFAULT NULL,
  `foreign_self_sell_out` int(11) DEFAULT NULL,
  `foreign_self_diff` int(11) DEFAULT NULL,
  `foreign_all_buy_in` int(11) DEFAULT NULL,
  `foreign_all_sell_out` int(11) DEFAULT NULL,
  `foreign_all_diff` int(11) DEFAULT NULL,
  `invest_buy_in` int(11) DEFAULT NULL,
  `invest_sell_out` int(11) DEFAULT NULL,
  `invest_diff` int(11) DEFAULT NULL,
  `dealer_self_buy_in` int(11) DEFAULT NULL,
  `dealer_self_sell_out` int(11) DEFAULT NULL,
  `dealer_self_diff` int(11) DEFAULT NULL,
  `dealer_risk_buy_in` int(11) DEFAULT NULL,
  `dealer_risk_sell_out` int(11) DEFAULT NULL,
  `dealer_risk_diff` int(11) DEFAULT NULL,
  `dealer_all_buy_in` int(11) DEFAULT NULL,
  `dealer_all_sell_out` int(11) DEFAULT NULL,
  `dealer_all_diff` int(11) DEFAULT NULL,
  `total_diff` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人日交易明細資訊(年)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrdsum_daily`
--

DROP TABLE IF EXISTS `d_3itrdsum_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrdsum_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `item_name` varchar(64) NOT NULL,
  `buy_in_money` decimal(20,3) DEFAULT NULL,
  `sell_out_money` decimal(20,3) DEFAULT NULL,
  `diff_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=1939 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額彙總表(日)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrdsum_monthly`
--

DROP TABLE IF EXISTS `d_3itrdsum_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrdsum_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `item_name` varchar(64) NOT NULL,
  `buy_in_money` decimal(20,3) DEFAULT NULL,
  `sell_out_money` decimal(20,3) DEFAULT NULL,
  `diff_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=273 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額彙總表(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrdsum_weekly`
--

DROP TABLE IF EXISTS `d_3itrdsum_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrdsum_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `item_name` varchar(64) NOT NULL,
  `buy_in_money` decimal(20,3) DEFAULT NULL,
  `sell_out_money` decimal(20,3) DEFAULT NULL,
  `diff_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=1151 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額彙總表(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_3itrdsum_yearly`
--

DROP TABLE IF EXISTS `d_3itrdsum_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_3itrdsum_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `item_name` varchar(64) NOT NULL,
  `buy_in_money` decimal(20,3) DEFAULT NULL,
  `sell_out_money` decimal(20,3) DEFAULT NULL,
  `diff_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣金額彙總表(年)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_amt_rank_daily`
--

DROP TABLE IF EXISTS `d_amt_rank_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_amt_rank_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_value` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=2643 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股成交值排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_amt_rank_monthly`
--

DROP TABLE IF EXISTS `d_amt_rank_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_amt_rank_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_value` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7571 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股成交值排行(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_amt_rank_yearly`
--

DROP TABLE IF EXISTS `d_amt_rank_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_amt_rank_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_value` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=9440 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股成交值排行(年)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_avg_amt_daily`
--

DROP TABLE IF EXISTS `d_avg_amt_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_avg_amt_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `avg_value` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=3093 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日均值排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_block_day`
--

DROP TABLE IF EXISTS `d_block_day`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_block_day` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `trade_item` varchar(32) NOT NULL,
  `trade_period` varchar(32) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_price` decimal(20,3) DEFAULT NULL,
  `deal_stock_count` bigint(20) DEFAULT NULL,
  `deal_value` decimal(20,3) DEFAULT NULL,
  `deal_time` datetime DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易日成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_block_mth_monthly`
--

DROP TABLE IF EXISTS `d_block_mth_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_block_mth_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `trade_type` varchar(32) NOT NULL,
  `deal_cnt` int(11) DEFAULT NULL,
  `deal_stock_cnt` decimal(20,3) DEFAULT NULL,
  `deal_percent` decimal(20,3) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_money_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=629 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易月成交量值統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_block_yr_yearly`
--

DROP TABLE IF EXISTS `d_block_yr_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_block_yr_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `trade_type` varchar(32) NOT NULL,
  `deal_cnt` int(11) DEFAULT NULL,
  `deal_stock_cnt` decimal(20,3) DEFAULT NULL,
  `deal_percent` decimal(20,3) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_money_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易年成交量值統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_ceil_ord`
--

DROP TABLE IF EXISTS `d_ceil_ord`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_ceil_ord` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `up_down_price` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `ceil_floor_deal_cnt` bigint(20) DEFAULT NULL,
  `ceil_floor_ask_cnt` bigint(20) DEFAULT NULL,
  `ceil_floor_no_deal_cnt` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=16359 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='漲跌停未成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_dealtr_hedge_daily`
--

DROP TABLE IF EXISTS `d_dealtr_hedge_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_dealtr_hedge_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_sell` smallint(6) NOT NULL,
  `day_order` int(11) DEFAULT NULL,
  `self_buy_in` int(11) DEFAULT NULL,
  `self_sell_out` int(11) DEFAULT NULL,
  `self_diff` int(11) DEFAULT NULL,
  `risk_buy_in` int(11) DEFAULT NULL,
  `risk_sell_out` int(11) DEFAULT NULL,
  `risk_diff` int(11) DEFAULT NULL,
  `total_diff` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7637 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='自營商買賣超彙總表(日)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_forgtr_daily`
--

DROP TABLE IF EXISTS `d_forgtr_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_forgtr_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `buy_sell_type` smallint(6) NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_in` int(11) DEFAULT NULL,
  `sell_out` int(11) DEFAULT NULL,
  `diff` int(11) DEFAULT NULL,
  `self_buy_in` int(11) DEFAULT NULL,
  `self_sell_out` int(11) DEFAULT NULL,
  `self_diff` int(11) DEFAULT NULL,
  `total_buy_in` int(11) DEFAULT NULL,
  `total_sell_out` int(11) DEFAULT NULL,
  `total_diff` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=1214 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資買賣超彙總表(日)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_forgtr_monthly`
--

DROP TABLE IF EXISTS `d_forgtr_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_forgtr_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `buy_sell_type` smallint(6) NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_in` int(11) DEFAULT NULL,
  `sell_out` int(11) DEFAULT NULL,
  `diff` int(11) DEFAULT NULL,
  `self_buy_in` int(11) DEFAULT NULL,
  `self_sell_out` int(11) DEFAULT NULL,
  `self_diff` int(11) DEFAULT NULL,
  `total_buy_in` int(11) DEFAULT NULL,
  `total_sell_out` int(11) DEFAULT NULL,
  `total_diff` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=40839 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資買賣超彙總表(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_forgtr_weekly`
--

DROP TABLE IF EXISTS `d_forgtr_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_forgtr_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `buy_sell_type` smallint(6) NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_in` int(11) DEFAULT NULL,
  `sell_out` int(11) DEFAULT NULL,
  `diff` int(11) DEFAULT NULL,
  `self_buy_in` int(11) DEFAULT NULL,
  `self_sell_out` int(11) DEFAULT NULL,
  `self_diff` int(11) DEFAULT NULL,
  `total_buy_in` int(11) DEFAULT NULL,
  `total_sell_out` int(11) DEFAULT NULL,
  `total_diff` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=106759 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資買賣超彙總表(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_forgtr_yearly`
--

DROP TABLE IF EXISTS `d_forgtr_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_forgtr_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `buy_sell_type` smallint(6) NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_in` int(11) DEFAULT NULL,
  `sell_out` int(11) DEFAULT NULL,
  `diff` int(11) DEFAULT NULL,
  `self_buy_in` int(11) DEFAULT NULL,
  `self_sell_out` int(11) DEFAULT NULL,
  `self_diff` int(11) DEFAULT NULL,
  `total_buy_in` int(11) DEFAULT NULL,
  `total_sell_out` int(11) DEFAULT NULL,
  `total_diff` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=5761 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資買賣超彙總表(年)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_index_summary`
--

DROP TABLE IF EXISTS `d_index_summary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_index_summary` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_cls` smallint(6) NOT NULL COMMENT '1:指數 2:報酬指數',
  `index_name` varchar(64) NOT NULL,
  `index_price` decimal(20,3) DEFAULT NULL,
  `up_down_price` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=529 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃股價指數收盤行情';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_lend`
--

DROP TABLE IF EXISTS `d_lend`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_lend` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `stock_agent` varchar(64) DEFAULT NULL,
  `lend_cnt` int(11) DEFAULT NULL,
  `lend_max_price` decimal(20,3) DEFAULT NULL,
  `lend_success_cnt` int(11) DEFAULT NULL,
  `lend_floor` decimal(20,5) DEFAULT NULL,
  `lend_ceil` decimal(20,5) DEFAULT NULL,
  `lend_fail_cnt` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=315 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='標借';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_margin_bal`
--

DROP TABLE IF EXISTS `d_margin_bal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_margin_bal` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yesterday_lend_balance` int(11) DEFAULT NULL,
  `lend_buy` int(11) DEFAULT NULL COMMENT '資買',
  `lend_sell` int(11) DEFAULT NULL COMMENT '資賣',
  `lend_back` int(11) DEFAULT NULL,
  `lend_balance` int(11) DEFAULT NULL,
  `lend_margin` int(11) DEFAULT NULL,
  `lend_percent` decimal(20,3) DEFAULT NULL,
  `lend_limit` decimal(20,3) DEFAULT NULL,
  `yesterday_borrow_balance` int(11) DEFAULT NULL,
  `borrow_sell` int(11) DEFAULT NULL COMMENT '券賣',
  `borrow_buy` int(11) DEFAULT NULL COMMENT '券買',
  `borrow_back` int(11) DEFAULT NULL COMMENT '券償',
  `borrow_balance` int(11) DEFAULT NULL,
  `borrow_margin` int(11) DEFAULT NULL,
  `borrow_percent` decimal(20,3) DEFAULT NULL,
  `borrow_limit` decimal(20,3) DEFAULT NULL,
  `offset` int(11) DEFAULT NULL COMMENT '資券相抵(張)',
  `note` varchar(128) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=27940 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃股票融資融券餘額';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_margin_rank_daily`
--

DROP TABLE IF EXISTS `d_margin_rank_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_margin_rank_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `mg_type` varchar(12) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yesterday_balance` decimal(20,3) DEFAULT NULL,
  `today_balance` decimal(20,3) DEFAULT NULL,
  `total_used` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=181 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券增減排行表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_margin_rank_monthly`
--

DROP TABLE IF EXISTS `d_margin_rank_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_margin_rank_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `mg_type` varchar(12) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yesterday_balance` decimal(20,3) DEFAULT NULL,
  `today_balance` decimal(20,3) DEFAULT NULL,
  `total_used` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=321 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券增減排行表(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_margin_rank_weekly`
--

DROP TABLE IF EXISTS `d_margin_rank_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_margin_rank_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `mg_type` varchar(12) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yesterday_balance` decimal(20,3) DEFAULT NULL,
  `today_balance` decimal(20,3) DEFAULT NULL,
  `total_used` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=241 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券增減排行表(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_margin_sbl`
--

DROP TABLE IF EXISTS `d_margin_sbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_margin_sbl` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `margin_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `margin_sell` decimal(20,3) DEFAULT NULL,
  `margin_buy` decimal(20,3) DEFAULT NULL,
  `margin_back` decimal(20,3) DEFAULT NULL,
  `margin_today_balance` decimal(20,3) DEFAULT NULL,
  `margin_limit` decimal(20,3) DEFAULT NULL,
  `lend_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `lend_sell` decimal(20,3) DEFAULT NULL,
  `lend_back` decimal(20,3) DEFAULT NULL,
  `lend_adjust` decimal(20,3) DEFAULT NULL,
  `lend_today_balance` decimal(20,3) DEFAULT NULL,
  `lend_next_remain_limit` decimal(20,3) DEFAULT NULL,
  `note` varchar(64) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=16376 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融券借券賣出餘額';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_market_highlight`
--

DROP TABLE IF EXISTS `d_market_highlight`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_market_highlight` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `list_stock_count` int(11) DEFAULT NULL,
  `total_capital` decimal(20,3) DEFAULT NULL,
  `total_value` decimal(20,3) DEFAULT NULL,
  `today_trade_value` decimal(20,3) DEFAULT NULL,
  `today_trade_stock_count` decimal(20,3) DEFAULT NULL,
  `close_index_price` decimal(20,3) DEFAULT NULL,
  `up_count` int(11) DEFAULT NULL,
  `down_count` int(11) DEFAULT NULL,
  `no_change_count` int(11) DEFAULT NULL,
  `index_up_down_price` decimal(20,3) DEFAULT NULL,
  `up_limit_count` int(11) DEFAULT NULL,
  `down_limit_count` int(11) DEFAULT NULL,
  `no_trade_count` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃股票市場現況';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_market_statistics_daily`
--

DROP TABLE IF EXISTS `d_market_statistics_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_market_statistics_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `item_name` varchar(64) NOT NULL,
  `trade_money` decimal(20,3) DEFAULT NULL,
  `trade_stock_count` int(11) DEFAULT NULL,
  `deal_count` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=122 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃證券成交統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_mgratio`
--

DROP TABLE IF EXISTS `d_mgratio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_mgratio` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `margin_avg_money` bigint(20) DEFAULT NULL,
  `margin_market_percent` decimal(20,3) DEFAULT NULL,
  `lend_avg_money` bigint(20) DEFAULT NULL,
  `lend_market_percent` decimal(20,3) DEFAULT NULL,
  `avg_money` bigint(20) DEFAULT NULL,
  `market_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=196 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='信用交易餘額概況表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_mgused_daily`
--

DROP TABLE IF EXISTS `d_mgused_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_mgused_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `mg_type` varchar(12) NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `used_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=581 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券使用率報表(日)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_mgused_weekly`
--

DROP TABLE IF EXISTS `d_mgused_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_mgused_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `mg_type` varchar(12) NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `used_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=101 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券使用率報表(週)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_mkt`
--

DROP TABLE IF EXISTS `d_mkt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_mkt` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `issue_stock_count` bigint(20) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `market_value` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=12665 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股市值排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_odd_daily`
--

DROP TABLE IF EXISTS `d_odd_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_odd_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `trade_type` varchar(32) NOT NULL,
  `deal_cnt` int(11) DEFAULT NULL,
  `deal_stock_cnt` decimal(20,3) DEFAULT NULL,
  `deal_percent` decimal(20,3) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_money_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=301 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='鉅額交易日成交量值統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_pera`
--

DROP TABLE IF EXISTS `d_pera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_pera` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `surplus` decimal(20,3) DEFAULT NULL,
  `ep_ratio` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=14726 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股本益比排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_peratio_pera`
--

DROP TABLE IF EXISTS `d_peratio_pera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_peratio_pera` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `pe_ratio` decimal(20,3) DEFAULT NULL,
  `dividend` decimal(10,8) DEFAULT NULL,
  `dividend_year` int(11) DEFAULT NULL,
  `yield` decimal(20,3) DEFAULT NULL,
  `net_value_ratio` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=5242 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股本益比殖利率及股價淨值比(依日期查詢)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_qfii`
--

DROP TABLE IF EXISTS `d_qfii`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_qfii` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `issue_stock_count` bigint(20) DEFAULT NULL,
  `remain_invest_count` bigint(20) DEFAULT NULL,
  `hold_count` bigint(20) DEFAULT NULL,
  `remain_invest_percent` decimal(20,3) DEFAULT NULL,
  `hold_percent` decimal(20,3) DEFAULT NULL,
  `law_hold_limit` decimal(20,3) DEFAULT NULL,
  `data_note` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=451 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='僑外資及陸資持股比例排行表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_qfiisect`
--

DROP TABLE IF EXISTS `d_qfiisect`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_qfiisect` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `sector_item` varchar(64) NOT NULL,
  `compony_count` int(11) DEFAULT NULL,
  `all_issue_stock_count` bigint(20) DEFAULT NULL,
  `hold_stock_count` bigint(20) DEFAULT NULL,
  `hold_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=169 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='各類股僑外資及陸資持股比例表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_brk`
--

DROP TABLE IF EXISTS `d_rt_brk`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_brk` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) DEFAULT NULL,
  `stock_name` varchar(64) NOT NULL,
  `dealer_name` varchar(32) DEFAULT NULL,
  `dealer_order` int(11) NOT NULL,
  `buy_count` int(11) DEFAULT NULL,
  `sell_count` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=233936 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='熱門股證券商進出排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_declined_daily`
--

DROP TABLE IF EXISTS `d_rt_declined_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_declined_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yesterday_close` decimal(20,3) DEFAULT NULL,
  `today_close` decimal(20,3) DEFAULT NULL,
  `down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=1777 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股跌幅排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_declined_monthly`
--

DROP TABLE IF EXISTS `d_rt_declined_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_declined_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=50955 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股跌幅排行(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_declined_weekly`
--

DROP TABLE IF EXISTS `d_rt_declined_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_declined_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股跌幅排行(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_rally_daily`
--

DROP TABLE IF EXISTS `d_rt_rally_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_rally_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `yesterday_close` decimal(20,3) DEFAULT NULL,
  `today_close` decimal(20,3) DEFAULT NULL,
  `up_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=814 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股漲幅排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_rally_monthly`
--

DROP TABLE IF EXISTS `d_rt_rally_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_rally_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `up_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=45603 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股漲幅排行(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_rt_rally_weekly`
--

DROP TABLE IF EXISTS `d_rt_rally_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_rt_rally_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `up_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=13595 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股漲幅排行(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_sectr`
--

DROP TABLE IF EXISTS `d_sectr`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_sectr` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `sector_name` varchar(32) NOT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_percent` decimal(20,3) DEFAULT NULL,
  `deal_stock_count` bigint(20) DEFAULT NULL,
  `deal_stock_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=361 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='類股成交價量比重';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_shtsell`
--

DROP TABLE IF EXISTS `d_shtsell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_shtsell` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `margin_count` int(11) DEFAULT NULL,
  `margin_money` decimal(20,3) DEFAULT NULL,
  `lend_count` int(11) DEFAULT NULL,
  `lend_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=3446 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='當日融券賣出與借券賣出成交量值';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_sitctr_daily`
--

DROP TABLE IF EXISTS `d_sitctr_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_sitctr_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `buy_sell_type` smallint(6) NOT NULL,
  `rank` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_in` int(11) DEFAULT NULL,
  `sell_out` int(11) DEFAULT NULL,
  `total_diff` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='投信買賣超彙總表(日)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_stk_avg_daily`
--

DROP TABLE IF EXISTS `d_stk_avg_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_stk_avg_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `avg_count` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=2616 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日均量排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_stk_avg_monthly`
--

DROP TABLE IF EXISTS `d_stk_avg_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_stk_avg_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `avg_count` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=6455 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日均量排行(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_stk_avg_yearly`
--

DROP TABLE IF EXISTS `d_stk_avg_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_stk_avg_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `avg_count` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=9437 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日均量排行(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_stk_quote`
--

DROP TABLE IF EXISTS `d_stk_quote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_stk_quote` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `close_p` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `open_p` decimal(20,3) DEFAULT NULL,
  `high_p` decimal(20,3) DEFAULT NULL,
  `low_p` decimal(20,3) DEFAULT NULL,
  `avg_p` decimal(20,3) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `last_buy_price` decimal(20,3) DEFAULT NULL,
  `last_sell_price` decimal(20,3) DEFAULT NULL,
  `issue_stock_cnt` bigint(20) DEFAULT NULL,
  `next_ref_price` decimal(20,3) DEFAULT NULL,
  `next_max_price` decimal(20,3) DEFAULT NULL,
  `next_min_price` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=129474 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃股票行情';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_stk_wn1430`
--

DROP TABLE IF EXISTS `d_stk_wn1430`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_stk_wn1430` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(8) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `up_down_price` decimal(20,3) DEFAULT NULL,
  `open_price` decimal(20,3) DEFAULT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `deal_stock_count` int(11) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_trade_count` int(11) DEFAULT NULL,
  `last_buy_price` decimal(20,3) DEFAULT NULL,
  `last_sell_price` decimal(20,3) DEFAULT NULL,
  `issue_stock_count` bigint(20) DEFAULT NULL,
  `next_up_limit` decimal(20,3) DEFAULT NULL,
  `next_down_limit` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=93125 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃股票每日收盤行情(不含定價)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_stk_wn1430_summary`
--

DROP TABLE IF EXISTS `d_stk_wn1430_summary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_stk_wn1430_summary` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `sum_item` varchar(16) NOT NULL,
  `all_market` decimal(20,3) DEFAULT NULL,
  `stock` decimal(20,3) DEFAULT NULL,
  `fund` decimal(20,3) DEFAULT NULL,
  `buy_warrant` decimal(20,3) DEFAULT NULL,
  `sale_warrant` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=166 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='上櫃股票每日收盤行情(不含定價)-委託及成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_trn_daily`
--

DROP TABLE IF EXISTS `d_trn_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_trn_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_stock_count` bigint(20) DEFAULT NULL,
  `issue_stock_count` bigint(20) DEFAULT NULL,
  `turnover_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=41422 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股週轉率排行';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_trn_monthly`
--

DROP TABLE IF EXISTS `d_trn_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_trn_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_stock_count` bigint(20) DEFAULT NULL,
  `issue_stock_count` bigint(20) DEFAULT NULL,
  `turnover_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=99774 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股週轉率排行(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_trn_yearly`
--

DROP TABLE IF EXISTS `d_trn_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_trn_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_stock_count` bigint(20) DEFAULT NULL,
  `issue_stock_count` bigint(20) DEFAULT NULL,
  `turnover_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=9440 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股週轉率排行(年)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_vol_rank_daily`
--

DROP TABLE IF EXISTS `d_vol_rank_daily`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_vol_rank_daily` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_sheet_count` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=3172 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT=' 個股成交量排行(日)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_vol_rank_monthly`
--

DROP TABLE IF EXISTS `d_vol_rank_monthly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_vol_rank_monthly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_sheet_count` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=99774 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股成交量排行(月)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_vol_rank_weekly`
--

DROP TABLE IF EXISTS `d_vol_rank_weekly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_vol_rank_weekly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_sheet_count` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=189651 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股成交量排行(周)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_vol_rank_yearly`
--

DROP TABLE IF EXISTS `d_vol_rank_yearly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_vol_rank_yearly` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `rank_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_sheet_count` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=9440 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股成交量排行(年)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `d_wkq`
--

DROP TABLE IF EXISTS `d_wkq`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `d_wkq` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `ref_price` decimal(20,3) DEFAULT NULL,
  `high_max_price` decimal(20,3) DEFAULT NULL,
  `low_min_price` decimal(20,3) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `open_price` decimal(20,3) DEFAULT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `deal_sheet_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `pe_ratio` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=67216 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='證券行情週報表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fmnptk`
--

DROP TABLE IF EXISTS `fmnptk`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `fmnptk` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stock_no` varchar(16) NOT NULL,
  `year` int(11) NOT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `high_date` datetime DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `low_date` datetime DEFAULT NULL,
  `close_avg` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`stock_no`,`year`)
) ENGINE=InnoDB AUTO_INCREMENT=5972 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股年成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fmnptk_stat`
--

DROP TABLE IF EXISTS `fmnptk_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `fmnptk_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stock_no` varchar(16) NOT NULL,
  `last_high` decimal(20,3) DEFAULT NULL,
  `last_high_date` datetime DEFAULT NULL,
  `last_low` decimal(20,3) DEFAULT NULL,
  `last_low_date` datetime DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=318 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股年成交資訊-統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fmsrfk`
--

DROP TABLE IF EXISTS `fmsrfk`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `fmsrfk` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `year` int(11) NOT NULL,
  `month` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `weight_avg` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `turnover_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`year`,`month`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=1122 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股月成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fmtqik`
--

DROP TABLE IF EXISTS `fmtqik`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `fmtqik` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `deal_stock_num` bigint(20) unsigned DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_trade_num` bigint(20) unsigned DEFAULT NULL,
  `issue_weight` decimal(20,3) DEFAULT NULL COMMENT '發行量加權股價指數',
  `up_down_point` decimal(20,3) DEFAULT NULL COMMENT '漲跌點數',
  `title` varchar(64) DEFAULT NULL,
  `create_at` datetime DEFAULT NULL,
  `update_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日市場成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_5mins`
--

DROP TABLE IF EXISTS `mi_5mins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_5mins` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_time` datetime NOT NULL,
  `ask_buy_in_cnt` int(11) DEFAULT NULL,
  `ask_buy_in_qty` bigint(20) DEFAULT NULL,
  `ask_sell_out_cnt` int(11) DEFAULT NULL,
  `ask_sell_out_qty` bigint(20) DEFAULT NULL,
  `deal_cnt` int(11) DEFAULT NULL,
  `deal_qty` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime DEFAULT NULL,
  `update_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_time`)
) ENGINE=InnoDB AUTO_INCREMENT=3242 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index`
--

DROP TABLE IF EXISTS `mi_index`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index` (
  `uid` bigint(20) unsigned NOT NULL,
  `data_date` datetime NOT NULL COMMENT '資料日期',
  `stock_no` varchar(16) NOT NULL COMMENT '證券代號',
  `stock_name` varchar(64) DEFAULT NULL COMMENT '證券名稱',
  `deal_stock_num` bigint(20) unsigned DEFAULT NULL COMMENT '成交股數',
  `deal_trade_num` bigint(20) unsigned DEFAULT NULL COMMENT '成交筆數',
  `deal_money` decimal(10,0) DEFAULT NULL COMMENT '成交金額',
  `open_price` decimal(10,0) DEFAULT NULL,
  `high_price` decimal(10,0) DEFAULT NULL,
  `low_price` decimal(10,0) DEFAULT NULL,
  `close_price` decimal(10,0) DEFAULT NULL,
  `up_down` tinyint(4) DEFAULT NULL,
  `up_down_price` decimal(10,0) DEFAULT NULL,
  `last_show_buy_price` decimal(10,0) DEFAULT NULL COMMENT '最後揭示買價',
  `last_show_buy_qty` bigint(20) DEFAULT NULL COMMENT '最後揭示買量',
  `last_show_sell_price` decimal(10,0) DEFAULT NULL COMMENT '最後揭示賣價',
  `last_show_sell_qty` bigint(20) DEFAULT NULL COMMENT '最後揭示賣量',
  `eps` decimal(10,0) DEFAULT NULL COMMENT '本益比',
  `title` varchar(256) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_all`
--

DROP TABLE IF EXISTS `mi_index_all`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_all` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL COMMENT '資料日期',
  `select_type` varchar(32) NOT NULL,
  `stock_no` varchar(16) NOT NULL COMMENT '證券代號',
  `stock_name` varchar(64) DEFAULT NULL COMMENT '證券名稱',
  `deal_stock_num` bigint(20) unsigned DEFAULT NULL COMMENT '成交股數',
  `deal_trade_num` bigint(20) unsigned DEFAULT NULL COMMENT '成交筆數',
  `deal_money` decimal(20,3) DEFAULT NULL COMMENT '成交金額',
  `open_price` decimal(20,3) DEFAULT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `up_down` tinyint(4) DEFAULT NULL,
  `up_down_price` decimal(20,3) DEFAULT NULL,
  `last_show_buy_price` decimal(20,3) DEFAULT NULL COMMENT '最後揭示買價',
  `last_show_buy_qty` bigint(20) unsigned DEFAULT NULL COMMENT '最後揭示買量',
  `last_show_sell_price` decimal(20,3) DEFAULT NULL COMMENT '最後揭示賣價',
  `last_show_sell_qty` bigint(20) unsigned DEFAULT NULL COMMENT '最後揭示賣量',
  `eps` decimal(20,3) DEFAULT NULL COMMENT '本益比',
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=365891 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_deal_stat`
--

DROP TABLE IF EXISTS `mi_index_deal_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_deal_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `deal_item` varchar(16) NOT NULL,
  `all_market` bigint(20) DEFAULT NULL,
  `stock_market` bigint(20) DEFAULT NULL,
  `fund_market` bigint(20) DEFAULT NULL,
  `warrant_percent` decimal(20,3) DEFAULT NULL,
  `warrant_market` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`deal_item`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-委託及成交統計資訊-成交統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_etf`
--

DROP TABLE IF EXISTS `mi_index_etf`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_etf` (
  `uid` bigint(20) unsigned NOT NULL,
  `data_date` datetime NOT NULL COMMENT '資料日期',
  `stock_no` varchar(16) NOT NULL COMMENT '證券代號',
  `stock_name` varchar(64) DEFAULT NULL COMMENT '證券名稱',
  `deal_stock_num` bigint(20) unsigned DEFAULT NULL COMMENT '成交股數',
  `deal_trade_num` bigint(20) unsigned DEFAULT NULL COMMENT '成交筆數',
  `deal_money` decimal(10,0) DEFAULT NULL COMMENT '成交金額',
  `open_price` decimal(10,0) DEFAULT NULL,
  `high_price` decimal(10,0) DEFAULT NULL,
  `low_price` decimal(10,0) DEFAULT NULL,
  `close_price` decimal(10,0) DEFAULT NULL,
  `up_down` tinyint(4) DEFAULT NULL,
  `up_down_price` decimal(10,0) DEFAULT NULL,
  `last_show_buy_price` decimal(10,0) DEFAULT NULL COMMENT '最後揭示買價',
  `last_show_buy_qty` bigint(20) unsigned DEFAULT NULL COMMENT '最後揭示買量',
  `last_show_sell_price` decimal(10,0) DEFAULT NULL COMMENT '最後揭示賣價',
  `last_show_sell_qty` bigint(20) unsigned DEFAULT NULL COMMENT '最後揭示賣量',
  `eps` decimal(10,0) DEFAULT NULL COMMENT '本益比',
  `cat` varchar(32) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_intrust_stat`
--

DROP TABLE IF EXISTS `mi_index_intrust_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_intrust_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `intrust_item` varchar(16) NOT NULL,
  `all_market` bigint(20) DEFAULT NULL,
  `stock_market` bigint(20) DEFAULT NULL,
  `fund_market` bigint(20) DEFAULT NULL,
  `warrant_percent` decimal(20,3) DEFAULT NULL,
  `warrant_market` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`intrust_item`)
) ENGINE=InnoDB AUTO_INCREMENT=73 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-委託及成交統計資訊-委託統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_market_stat`
--

DROP TABLE IF EXISTS `mi_index_market_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_market_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `deal_stat_item` varchar(32) NOT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`deal_stat_item`)
) ENGINE=InnoDB AUTO_INCREMENT=331 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-大盤統計資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_price_idx_cross`
--

DROP TABLE IF EXISTS `mi_index_price_idx_cross`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_price_idx_cross` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(32) NOT NULL,
  `close_index_val` decimal(20,3) DEFAULT NULL,
  `up_down_point` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=83 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-收盤指數資訊-價格指數-跨市場';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_price_idx_twcomp`
--

DROP TABLE IF EXISTS `mi_index_price_idx_twcomp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_price_idx_twcomp` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(32) NOT NULL,
  `close_index_val` decimal(20,3) DEFAULT NULL,
  `up_down_point` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=451 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-收盤指數資訊-價格指數-臺灣指數公司';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_price_idx_twse`
--

DROP TABLE IF EXISTS `mi_index_price_idx_twse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_price_idx_twse` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(32) NOT NULL,
  `close_index_val` decimal(20,3) DEFAULT NULL,
  `up_down_point` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=1144 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-收盤指數資訊-價格指數-台灣證券交易所';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_return_idx_cross`
--

DROP TABLE IF EXISTS `mi_index_return_idx_cross`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_return_idx_cross` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(32) NOT NULL,
  `close_index_val` decimal(20,3) DEFAULT NULL,
  `up_down_point` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=99 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-收盤指數資訊-報酬指數-跨市場';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_return_idx_twcomp`
--

DROP TABLE IF EXISTS `mi_index_return_idx_twcomp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_return_idx_twcomp` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(32) NOT NULL,
  `close_index_val` decimal(20,3) DEFAULT NULL,
  `up_down_point` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=533 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-收盤指數資訊-報酬指數-臺灣指數公司';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_return_idx_twse`
--

DROP TABLE IF EXISTS `mi_index_return_idx_twse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_return_idx_twse` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `index_name` varchar(32) NOT NULL,
  `close_index_val` decimal(20,3) DEFAULT NULL,
  `up_down_point` decimal(20,3) DEFAULT NULL,
  `up_down_percent` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`index_name`)
) ENGINE=InnoDB AUTO_INCREMENT=946 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-收盤指數資訊-報酬指數-台灣證券交易所';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_top20`
--

DROP TABLE IF EXISTS `mi_index_top20`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_top20` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `day_order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_stock_num` bigint(20) DEFAULT NULL,
  `deal_trade_num` bigint(20) DEFAULT NULL,
  `open_price` decimal(10,3) DEFAULT NULL,
  `high_price` decimal(10,3) DEFAULT NULL,
  `low_price` decimal(10,3) DEFAULT NULL,
  `close_price` decimal(10,3) DEFAULT NULL,
  `up_down` tinyint(4) DEFAULT NULL,
  `up_down_price` decimal(10,3) DEFAULT NULL,
  `last_show_buy_price` decimal(10,3) DEFAULT NULL,
  `last_show_sell_price` decimal(10,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`day_order`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日成交量前二十名證券';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_index_up_down_stat`
--

DROP TABLE IF EXISTS `mi_index_up_down_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_index_up_down_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `class_item` varchar(32) NOT NULL,
  `all_market` int(11) DEFAULT NULL,
  `market_at_limit` int(11) DEFAULT NULL,
  `all_stock` int(11) DEFAULT NULL,
  `stock_at_limit` int(11) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`class_item`)
) ENGINE=InnoDB AUTO_INCREMENT=111 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日收盤行情-大盤統計資訊-漲跌證券數合計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_margin`
--

DROP TABLE IF EXISTS `mi_margin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_margin` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(32) DEFAULT NULL,
  `finance_buy_in` decimal(20,3) DEFAULT NULL,
  `finance_sell_out` decimal(20,3) DEFAULT NULL,
  `finance_cash_back` decimal(20,3) DEFAULT NULL,
  `finance_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `finance_today_balance` decimal(20,3) DEFAULT NULL,
  `finance_ceiling` decimal(20,3) DEFAULT NULL,
  `margin_buy_in` decimal(20,3) DEFAULT NULL,
  `margin_sell_out` decimal(20,3) DEFAULT NULL,
  `margin_cash_back` decimal(20,3) DEFAULT NULL,
  `margin_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `margin_today_balance` decimal(20,3) DEFAULT NULL,
  `margin_ceiling` decimal(20,3) DEFAULT NULL,
  `offset` bigint(20) DEFAULT NULL,
  `note` varchar(128) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=22265 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券餘額-彙總';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_margin_stat`
--

DROP TABLE IF EXISTS `mi_margin_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_margin_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stat_item` varchar(32) NOT NULL,
  `buy_in` decimal(20,3) DEFAULT NULL,
  `sell_out` decimal(20,3) DEFAULT NULL,
  `return_back` decimal(20,3) DEFAULT NULL,
  `yesterday_balance` decimal(20,3) DEFAULT NULL,
  `today_balance` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stat_item`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融資融券餘額-信用交易統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_qfiis`
--

DROP TABLE IF EXISTS `mi_qfiis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_qfiis` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `world_stock_no` varchar(24) DEFAULT NULL,
  `issue_cnt` bigint(20) DEFAULT NULL,
  `investable_cnt` bigint(20) DEFAULT NULL,
  `hold_cnt` bigint(20) DEFAULT NULL,
  `investable_rate` decimal(8,2) DEFAULT NULL,
  `hold_rate` decimal(8,2) DEFAULT NULL,
  `law_max_rate` decimal(8,2) DEFAULT NULL,
  `law_china_max_rate` decimal(8,2) DEFAULT NULL,
  `change_reason` varchar(256) DEFAULT NULL,
  `last_report_change_date` datetime DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=5494 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資投資持股統計';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_qfiis_cat`
--

DROP TABLE IF EXISTS `mi_qfiis_cat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_qfiis_cat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `industry_cat` varchar(64) NOT NULL,
  `hold_comp_cnt` int(11) DEFAULT NULL,
  `total_issue_cnt` bigint(20) DEFAULT NULL,
  `hold_stock_cnt` bigint(20) DEFAULT NULL,
  `hold_stock_rate` decimal(8,2) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`industry_cat`)
) ENGINE=InnoDB AUTO_INCREMENT=3777 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資投資類股彙總持股比率表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mi_qfiis_sort_20`
--

DROP TABLE IF EXISTS `mi_qfiis_sort_20`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `mi_qfiis_sort_20` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `order` int(11) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `issue_cnt` bigint(20) DEFAULT NULL,
  `investable_cnt` bigint(20) DEFAULT NULL,
  `hold_cnt` bigint(20) DEFAULT NULL,
  `investable_rate` decimal(8,2) DEFAULT NULL,
  `hold_rate` decimal(8,2) DEFAULT NULL,
  `law_max_rate` decimal(8,2) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`order`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=141 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資持股比率前二十名彙總表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `p_filter_stg`
--

DROP TABLE IF EXISTS `p_filter_stg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `p_filter_stg` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stg_code` varchar(32) NOT NULL,
  `stg_name` varchar(32) NOT NULL,
  `stg_class_name` varchar(128) DEFAULT NULL,
  `stg_p1` varchar(32) DEFAULT NULL,
  `stg_p2` varchar(32) DEFAULT NULL,
  `stg_p3` varchar(32) DEFAULT NULL,
  `stg_p4` varchar(32) DEFAULT NULL,
  `stg_p5` varchar(32) DEFAULT NULL,
  `stg_p6` varchar(32) DEFAULT NULL,
  `stg_p7` varchar(32) DEFAULT NULL,
  `stg_p8` varchar(32) DEFAULT NULL,
  `stg_p9` varchar(32) DEFAULT NULL,
  `stg_p10` varchar(32) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='盤後選股策略設定檔';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `p_filter_stg_describe`
--

DROP TABLE IF EXISTS `p_filter_stg_describe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `p_filter_stg_describe` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stg_code` varchar(32) NOT NULL,
  `stg_param_note` varchar(512) DEFAULT NULL,
  `stg_note` varchar(512) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='盤後選股策略設定檔文字說明';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `stock_day`
--

DROP TABLE IF EXISTS `stock_day`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `stock_day` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `open_price` decimal(20,3) DEFAULT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `high_low_diff` decimal(20,3) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=1645 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日成交資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `stock_day_avg`
--

DROP TABLE IF EXISTS `stock_day_avg`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `stock_day_avg` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=402 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='個股日收盤價及月平均價';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `stock_first`
--

DROP TABLE IF EXISTS `stock_first`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `stock_first` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `deal_stock_cnt` bigint(20) DEFAULT NULL,
  `deal_cnt` bigint(20) DEFAULT NULL,
  `deal_money` decimal(20,3) DEFAULT NULL,
  `open_price` decimal(20,3) DEFAULT NULL,
  `high_price` decimal(20,3) DEFAULT NULL,
  `low_price` decimal(20,3) DEFAULT NULL,
  `close_price` decimal(20,3) DEFAULT NULL,
  `up_down_price` decimal(20,3) DEFAULT NULL,
  `last_buy_in_price` decimal(20,3) DEFAULT NULL,
  `last_buy_ask_qty` bigint(20) DEFAULT NULL,
  `last_sell_out_price` decimal(20,3) DEFAULT NULL,
  `last_sell_ask_qty` bigint(20) DEFAULT NULL,
  `pe_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=76 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日第一上市外國股票成交量值';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `stock_item`
--

DROP TABLE IF EXISTS `stock_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `stock_item` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stock_type` int(11) NOT NULL DEFAULT '0' COMMENT '10:一般股票\n11:ETF\n12:受益證券\n13:附認股權特別股\n14:認股權憑證\n15:存託憑證\n16:封閉式基金\n21:認購權證\n22:認售權證\n23:牛證\n24:熊證\n30:附認股權公司債\n31:可轉換公司債\n40:其他',
  `category` varchar(12) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `memo` varchar(512) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`stock_no`,`category`,`stock_type`)
) ENGINE=InnoDB AUTO_INCREMENT=69027 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `t86`
--

DROP TABLE IF EXISTS `t86`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `t86` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` bigint(20) DEFAULT NULL,
  `foreign_sell_out` bigint(20) DEFAULT NULL,
  `foreign_diff` bigint(20) DEFAULT NULL,
  `foreign_dealer_buy_in` bigint(20) DEFAULT NULL,
  `foreign_dealer_sell_out` bigint(20) DEFAULT NULL,
  `foreign_dealer_diff` bigint(20) DEFAULT NULL,
  `trust_buy_in` bigint(20) DEFAULT NULL,
  `trust_sell_out` bigint(20) DEFAULT NULL,
  `trust_diff` bigint(20) DEFAULT NULL,
  `dealer_diff` bigint(20) DEFAULT NULL,
  `dealer_self_buy_in` bigint(20) DEFAULT NULL,
  `dealer_self_sell_out` bigint(20) DEFAULT NULL,
  `dealer_self_diff` bigint(20) DEFAULT NULL,
  `dealer_risk_buy_in` bigint(20) DEFAULT NULL,
  `dealer_risk_sell_out` bigint(20) DEFAULT NULL,
  `dealer_risk_diff` bigint(20) DEFAULT NULL,
  `capital3_total_diff` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=232866 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt38u`
--

DROP TABLE IF EXISTS `twt38u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt38u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_cnt` bigint(20) DEFAULT NULL,
  `sell_cnt` bigint(20) DEFAULT NULL,
  `cnt_diff` bigint(20) DEFAULT NULL,
  `self_buy_cnt` bigint(20) DEFAULT NULL,
  `self_sell_cnt` bigint(20) DEFAULT NULL,
  `self_cnt_diff` bigint(20) DEFAULT NULL,
  `total_buy_cnt` bigint(20) DEFAULT NULL,
  `total_sell_cnt` bigint(20) DEFAULT NULL,
  `total_cnt_diff` bigint(20) DEFAULT NULL,
  `is_big_trade` bit(1) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=17012 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='外資及陸資買賣超彙總表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt43u`
--

DROP TABLE IF EXISTS `twt43u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt43u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `self_buy_cnt` bigint(20) DEFAULT NULL,
  `self_sell_cnt` bigint(20) DEFAULT NULL,
  `self_cnt_diff` bigint(20) DEFAULT NULL,
  `risk_buy_cnt` bigint(20) DEFAULT NULL,
  `risk_sell_cnt` bigint(20) DEFAULT NULL,
  `risk_cnt_diff` bigint(20) DEFAULT NULL,
  `total_buy_cnt` bigint(20) DEFAULT NULL,
  `total_sell_cnt` bigint(20) DEFAULT NULL,
  `total_cnt_diff` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='自營商買賣超彙總表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt44u`
--

DROP TABLE IF EXISTS `twt44u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt44u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `buy_cnt` bigint(20) DEFAULT NULL,
  `sell_cnt` bigint(20) DEFAULT NULL,
  `cnt_diff` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime DEFAULT NULL,
  `update_at` datetime DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt47u`
--

DROP TABLE IF EXISTS `twt47u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt47u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_year` int(11) NOT NULL,
  `data_month` int(11) NOT NULL,
  `last_update` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` bigint(20) DEFAULT NULL,
  `foreign_sell_out` bigint(20) DEFAULT NULL,
  `foreign_diff` bigint(20) DEFAULT NULL,
  `foreign_dealer_buy_in` bigint(20) DEFAULT NULL,
  `foreign_dealer_sell_out` bigint(20) DEFAULT NULL,
  `foreign_dealer_diff` bigint(20) DEFAULT NULL,
  `trust_buy_in` bigint(20) DEFAULT NULL,
  `trust_sell_out` bigint(20) DEFAULT NULL,
  `trust_diff` bigint(20) DEFAULT NULL,
  `dealer_diff` bigint(20) DEFAULT NULL,
  `dealer_self_buy_in` bigint(20) DEFAULT NULL,
  `dealer_self_sell_out` bigint(20) DEFAULT NULL,
  `dealer_self_diff` bigint(20) DEFAULT NULL,
  `dealer_risk_buy_in` bigint(20) DEFAULT NULL,
  `dealer_risk_sell_out` bigint(20) DEFAULT NULL,
  `dealer_risk_diff` bigint(20) DEFAULT NULL,
  `capital3_total_diff` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_year`,`data_month`,`select_type`,`stock_no`,`last_update`)
) ENGINE=InnoDB AUTO_INCREMENT=11904 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='三大法人買賣超月報';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt54u`
--

DROP TABLE IF EXISTS `twt54u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt54u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `week_start` datetime NOT NULL,
  `week_end` datetime NOT NULL,
  `last_update` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `foreign_buy_in` bigint(20) DEFAULT NULL,
  `foreign_sell_out` bigint(20) DEFAULT NULL,
  `foreign_diff` bigint(20) DEFAULT NULL,
  `foreign_dealer_buy_in` bigint(20) DEFAULT NULL,
  `foreign_dealer_sell_out` bigint(20) DEFAULT NULL,
  `foreign_dealer_diff` bigint(20) DEFAULT NULL,
  `trust_buy_in` bigint(20) DEFAULT NULL,
  `trust_sell_out` bigint(20) DEFAULT NULL,
  `trust_diff` bigint(20) DEFAULT NULL,
  `dealer_diff` bigint(20) DEFAULT NULL,
  `dealer_self_buy_in` bigint(20) DEFAULT NULL,
  `dealer_self_sell_out` bigint(20) DEFAULT NULL,
  `dealer_self_diff` bigint(20) DEFAULT NULL,
  `dealer_risk_buy_in` bigint(20) DEFAULT NULL,
  `dealer_risk_sell_out` bigint(20) DEFAULT NULL,
  `dealer_risk_diff` bigint(20) DEFAULT NULL,
  `capital3_total_diff` bigint(20) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`week_start`,`week_end`,`select_type`,`stock_no`,`last_update`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt84u`
--

DROP TABLE IF EXISTS `twt84u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt84u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `select_type` varchar(16) NOT NULL,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `today_high_limit` decimal(20,3) DEFAULT NULL,
  `today_open_base` decimal(20,3) DEFAULT NULL,
  `today_low_limit` decimal(20,3) DEFAULT NULL,
  `yesterday_open_base` decimal(20,3) DEFAULT NULL,
  `yesterday_close` decimal(20,3) DEFAULT NULL,
  `yesterday_buy_in` decimal(20,3) DEFAULT NULL,
  `yesterday_sell_out` decimal(20,3) DEFAULT NULL,
  `recent_deal_date` datetime DEFAULT NULL,
  `can_odd_lot` varchar(16) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`select_type`,`data_date`,`stock_no`) /*!80000 INVISIBLE */
) ENGINE=InnoDB AUTO_INCREMENT=15519 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='股價升降幅度';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt92u`
--

DROP TABLE IF EXISTS `twt92u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt92u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(32) DEFAULT NULL,
  `stop_margin_sell` bit(1) DEFAULT NULL,
  `stop_lend_sell` bit(1) DEFAULT NULL,
  `yesterday_down_limit_stop_all` bit(1) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='平盤下得融(借)券賣出之證券名單';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twt93u`
--

DROP TABLE IF EXISTS `twt93u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twt93u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(32) DEFAULT NULL,
  `margin_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `margin_buy_in` decimal(20,3) DEFAULT NULL,
  `margin_sell_out` decimal(20,3) DEFAULT NULL,
  `margin_current_ticket` bigint(20) DEFAULT NULL,
  `margin_today_balance` decimal(20,3) DEFAULT NULL,
  `margin_ceiling` decimal(20,3) DEFAULT NULL,
  `lend_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `lend_sell_out` decimal(20,3) DEFAULT NULL,
  `lend_return_back` decimal(20,3) DEFAULT NULL,
  `lend_adjust` decimal(20,3) DEFAULT NULL,
  `lend_balance` decimal(20,3) DEFAULT NULL,
  `lend_next_ceiling` decimal(20,3) DEFAULT NULL,
  `note` varchar(256) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=5313 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融券借券賣出餘額';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twta1u`
--

DROP TABLE IF EXISTS `twta1u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twta1u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(32) DEFAULT NULL,
  `market_type` varchar(16) DEFAULT NULL,
  `finance_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `finance_buy_in` decimal(20,3) DEFAULT NULL,
  `finance_sell_out` decimal(20,3) DEFAULT NULL,
  `finance_cash_back` decimal(20,3) DEFAULT NULL,
  `finance_today_balance` decimal(20,3) DEFAULT NULL,
  `finance_ceiling` decimal(20,3) DEFAULT NULL,
  `dealer_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `dealer_buy_in` decimal(20,3) DEFAULT NULL,
  `dealer_sell_out` decimal(20,3) DEFAULT NULL,
  `dealer_cash_back` decimal(20,3) DEFAULT NULL,
  `dealer_change` decimal(20,3) DEFAULT NULL,
  `dealer_today_balance` decimal(20,3) DEFAULT NULL,
  `dealer_ceiling` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_buy_in` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_sell_out` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_cash_back` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_change` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_today_balance` decimal(20,3) DEFAULT NULL,
  `dealer_unlimit_ceiling` decimal(20,3) DEFAULT NULL,
  `margin_loan_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `margin_loan_buy_in` decimal(20,3) DEFAULT NULL,
  `margin_loan_sell_out` decimal(20,3) DEFAULT NULL,
  `margin_loan_cash_back` decimal(20,3) DEFAULT NULL,
  `margin_loan_change` decimal(20,3) DEFAULT NULL,
  `margin_loan_today_balance` decimal(20,3) DEFAULT NULL,
  `margin_loan_ceiling` decimal(20,3) DEFAULT NULL,
  `margin_deliver_yesterday_balance` decimal(20,3) DEFAULT NULL,
  `margin_deliver_buy_in` decimal(20,3) DEFAULT NULL,
  `margin_deliver_sell_out` decimal(20,3) DEFAULT NULL,
  `margin_deliver_cash_back` decimal(20,3) DEFAULT NULL,
  `margin_deliver_change` decimal(20,3) DEFAULT NULL,
  `margin_deliver_today_balance` decimal(20,3) DEFAULT NULL,
  `margin_deliver_ceiling` decimal(20,3) DEFAULT NULL,
  `note` varchar(256) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`,`stock_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='借貸款項擔保品管制餘額';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twtasu`
--

DROP TABLE IF EXISTS `twtasu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twtasu` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) DEFAULT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `borrow_sell_deal_cnt` bigint(20) DEFAULT NULL COMMENT '融券賣出成交數量',
  `borrow_sell_deal_money` decimal(20,3) DEFAULT NULL COMMENT '融券賣出成交金額',
  `lending_sell_deal_cnt` bigint(20) DEFAULT NULL COMMENT '借券賣出成交數量',
  `lending_sell_deal_money` decimal(20,3) DEFAULT NULL COMMENT '借券賣出成交金額',
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=6485 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='當日融券賣出與借券賣出成交量值';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twtb4u`
--

DROP TABLE IF EXISTS `twtb4u`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twtb4u` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `select_type` varchar(16) NOT NULL,
  `data_date` datetime NOT NULL,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(64) DEFAULT NULL,
  `mark` varchar(4) DEFAULT NULL,
  `daytrade_deal_stock_cnt` bigint(20) DEFAULT NULL,
  `daytrade_buy_in_money` decimal(20,3) DEFAULT NULL,
  `daytrade_sell_out_money` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`select_type`,`data_date`,`stock_no`)
) ENGINE=InnoDB AUTO_INCREMENT=976 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日當日沖銷交易標的';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twtb4u_stat`
--

DROP TABLE IF EXISTS `twtb4u_stat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twtb4u_stat` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `data_date` datetime NOT NULL,
  `select_type` varchar(16) NOT NULL,
  `all_deal_stock_cnt` bigint(20) DEFAULT NULL,
  `all_deal_stock_rate` decimal(20,3) DEFAULT NULL,
  `all_buy_in_money` decimal(20,3) DEFAULT NULL,
  `all_buy_in_money_rate` decimal(20,3) DEFAULT NULL,
  `all_sell_out_money` decimal(20,3) DEFAULT NULL,
  `all_sell_out_money_rate` decimal(20,3) DEFAULT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`data_date`,`select_type`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='每日當日沖銷交易標的-當日沖銷交易統計資訊';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `twtbau2`
--

DROP TABLE IF EXISTS `twtbau2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `twtbau2` (
  `uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `stock_no` varchar(16) NOT NULL,
  `stock_name` varchar(32) DEFAULT NULL,
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `reason` varchar(32) NOT NULL,
  `title` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `IX_QRY` (`stock_no`,`start_date`,`end_date`,`reason`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='暫停先賣後買當日沖銷交易歷史查詢';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `work_record`
--

DROP TABLE IF EXISTS `work_record`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `work_record` (
  `Uid` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `graber_name` varchar(64) NOT NULL,
  `graber_freq` int(11) NOT NULL,
  `work_data_date` datetime NOT NULL,
  `is_complete` bit(1) DEFAULT NULL,
  `start_time` datetime DEFAULT NULL,
  `end_time` datetime DEFAULT NULL,
  `note` varchar(256) DEFAULT NULL,
  `create_at` datetime NOT NULL,
  `update_at` datetime NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='工作紀錄表';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-12 22:54:41
