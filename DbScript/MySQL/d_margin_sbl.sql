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
) ENGINE=InnoDB AUTO_INCREMENT=3502 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='融券借券賣出餘額';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-12-24 11:55:16
