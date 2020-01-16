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
-- Dumping data for table `p_filter_stg`
--

LOCK TABLES `p_filter_stg` WRITE;
/*!40000 ALTER TABLE `p_filter_stg` DISABLE KEYS */;
INSERT INTO `p_filter_stg` VALUES (1,'7e34ba46e2c38276480e','盤前選股1號','FilterNo1','-1000','2','2000','500','','','','','','','2019-12-10 11:34:26','2019-12-10 18:23:56'),(2,'7e34ba4733a4e3350a4d','盤後選股2號','FilterNo2','-1000','-2','2000','0','0','','','','','','2019-12-10 11:39:31','2019-12-10 23:37:10'),(3,'7e34c88bee5ea18f2f07','做多第一級','FilterLongStep1','3','','','','','','','','','','2019-12-24 22:39:02','2020-01-07 22:11:04'),(4,'7e34c88c6c3c71322c50','做多第二級','FilterLongStep2','3','','','','','','','','','','2019-12-24 22:46:51','2020-01-07 22:11:28'),(5,'7e34c88c6c3c80525c26','做多第三級','FilterLongStep3','3','','','','','','','','','','2019-12-24 22:47:51','2020-01-07 22:11:40'),(6,'7e406a009e65c1cd0755','做空第一級','FilterShortStep1','3','','','','','','','','','','2020-01-06 00:10:00','2020-01-07 22:11:50'),(7,'7e406a009e65d3615e56','做空第二級','FilterShortStep2','3','','','','','','','','','','2020-01-06 00:10:25','2020-01-07 22:12:00'),(8,'7e406a009e65e0592e1c','做空第三級','FilterShortStep3','3','','','','','','','','','','2020-01-06 00:10:59','2020-01-07 22:12:09'),(9,'7e40733fd80ea1663c0b','小黑空軍1號','FilterBlackShort1','2','','','','','','','','','','2020-01-15 10:21:49','2020-01-15 10:21:49');
/*!40000 ALTER TABLE `p_filter_stg` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `p_filter_stg_describe`
--

LOCK TABLES `p_filter_stg_describe` WRITE;
/*!40000 ALTER TABLE `p_filter_stg_describe` DISABLE KEYS */;
INSERT INTO `p_filter_stg_describe` VALUES (1,'7e34ba46e2c38276480e','1. 外資賣超\r\n2. 漲幅\r\n3. 成交量(張)\r\n4. 融資','股票大漲，外資賣超，融資增加','2019-12-10 11:34:27','2019-12-10 18:23:56'),(2,'7e34ba4733a4e3350a4d','1.外資賣超\r\n2.漲跌幅\r\n3.成交量\r\n4.自營商賣超\r\n5.融資','股票大跌，外資賣超，自營賣超，融資增加','2019-12-10 11:39:31','2019-12-10 23:37:10'),(3,'7e34c88bee5ea18f2f07','1. 連續N天','連續三天 融資減少 外資買超','2019-12-24 22:39:02','2020-01-07 22:11:04'),(4,'7e34c88c6c3c71322c50','1. 連續N天','連續三天 融資減少 外資買超  借券減少','2019-12-24 22:46:51','2020-01-07 22:11:28'),(5,'7e34c88c6c3c80525c26','1. 連續N天','連續三天 融資減少 外資買超  借券減少  融券增加','2019-12-24 22:47:51','2020-01-07 22:11:40'),(6,'7e406a009e65c1cd0755','1.連續N天','連續三天 融資增加 外資賣超','2020-01-06 00:10:00','2020-01-07 22:11:50'),(7,'7e406a009e65d3615e56','1.連續N天','連續三天 融資增加 外資賣超  借券增加','2020-01-06 00:10:25','2020-01-07 22:12:00'),(8,'7e406a009e65e0592e1c','1.連續N天','連續三天 融資增加 外資賣超  借券增加  融券增加','2020-01-06 00:10:59','2020-01-07 22:12:09'),(9,'7e40733fd80ea1663c0b','1. 連續N天','連續2天，融資比率高於50%，外資賣超連續2天，且第二天高於第一天','2020-01-15 10:21:49','2020-01-15 10:21:49');
/*!40000 ALTER TABLE `p_filter_stg_describe` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-16 23:05:55
