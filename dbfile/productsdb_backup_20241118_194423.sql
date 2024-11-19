-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2024-11-18 19:44:23
-- --------------------------------------
-- Server version 10.4.27-MariaDB mariadb.org binary distribution


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of paymentmethods
-- 

DROP TABLE IF EXISTS `paymentmethods`;
CREATE TABLE IF NOT EXISTS `paymentmethods` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table paymentmethods
-- 

/*!40000 ALTER TABLE `paymentmethods` DISABLE KEYS */;
INSERT INTO `paymentmethods`(`Id`,`Name`) VALUES(1,'Cash'),(2,'Credit Card'),(3,'Debit Card');
/*!40000 ALTER TABLE `paymentmethods` ENABLE KEYS */;

-- 
-- Definition of products
-- 

DROP TABLE IF EXISTS `products`;
CREATE TABLE IF NOT EXISTS `products` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `currency` int(11) NOT NULL,
  `Stock` int(11) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table products
-- 

/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products`(`Id`,`Name`,`Description`,`Price`,`currency`,`Stock`,`CreatedAt`) VALUES(1,'iPhone 14','Latest Apple iPhone with A15 Bionic chip',999.99,1,15,'2024-09-01 10:00:00'),(2,'Samsung Galaxy S23','Newest Samsung Galaxy model with Snapdragon processor',899.99,1,98,'2024-09-01 10:05:00'),(3,'Google Pixel 7','Google Pixel with the latest Android version',799.99,1,32,'2024-09-01 10:10:00'),(4,'OnePlus 10 Pro','High-end OnePlus phone with fast charging',699.99,1,100,'2024-09-01 10:15:00'),(5,'Xiaomi Mi 11','Xiaomi phone with excellent camera quality',499.99,1,96,'2024-09-01 10:20:00'),(6,'Sony Xperia 1 IV','Sony Xperia phone with 4K display',1199.99,1,100,'2024-09-01 10:25:00'),(7,'Huawei P50 Pro','Huawei phone with powerful camera setup',899.99,1,93,'2024-09-01 10:30:00'),(8,'Motorola Edge 30','Motorola smartphone with curved display',649.99,1,98,'2024-09-01 10:35:00'),(9,'Nokia G50','Affordable Nokia smartphone with 5G connectivity',299.99,1,88,'2024-09-01 10:40:00'),(10,'Asus ROG Phone 6','Gaming smartphone with cooling system',1099.99,1,100,'2024-09-01 10:45:00'),(11,'Oppo Find X5','Oppo phone with AMOLED display',749.99,1,96,'2024-09-01 10:50:00'),(12,'Vivo X80','Vivo phone with advanced photography features',829.99,1,100,'2024-09-01 10:55:00'),(15,'product example','this is an example product',4000.00,1,8,'2024-11-18 19:25:50');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;

-- 
-- Definition of roles
-- 

DROP TABLE IF EXISTS `roles`;
CREATE TABLE IF NOT EXISTS `roles` (
  `RoleId` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(255) NOT NULL,
  PRIMARY KEY (`RoleId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table roles
-- 

/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles`(`RoleId`,`RoleName`) VALUES(1,'admin'),(2,'user');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;

-- 
-- Definition of sale_details
-- 

DROP TABLE IF EXISTS `sale_details`;
CREATE TABLE IF NOT EXISTS `sale_details` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SaleId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `ProductName` varchar(250) DEFAULT NULL,
  `Price` decimal(10,2) DEFAULT NULL,
  `Units` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `sale_details_ibfk_2` (`ProductId`),
  KEY `sale_details_ibfk_1` (`SaleId`),
  CONSTRAINT `sale_details_ibfk_1` FOREIGN KEY (`SaleId`) REFERENCES `sales` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sale_details_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=219 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table sale_details
-- 

/*!40000 ALTER TABLE `sale_details` DISABLE KEYS */;
INSERT INTO `sale_details`(`Id`,`SaleId`,`ProductId`,`ProductName`,`Price`,`Units`) VALUES(6,10,1,'iPhone 14',0.00,0),(7,11,2,'Samsung Galaxy S23',899.99,1),(8,12,3,'Google Pixel 7',799.99,7),(9,12,2,'Samsung Galaxy S23',899.99,3),(10,12,1,'iPhone 14',999.99,1),(37,13,1,'iPhone 14',999.99,1),(38,13,2,'Samsung Galaxy S23',899.99,1),(39,13,3,'Google Pixel 7',799.99,1),(40,13,9,'Nokia G50',299.99,4),(42,13,11,'Oppo Find X5',749.99,2),(43,2,1,'iPhone 14',999.99,4),(49,14,2,'Samsung Galaxy S23',899.99,1),(50,14,1,'iPhone 14',999.99,1),(51,14,3,'Google Pixel 7',799.99,1),(52,15,1,'iPhone 14',999.99,1),(53,15,2,'Samsung Galaxy S23',899.99,1),(54,15,3,'Google Pixel 7',799.99,1),(55,16,1,'iPhone 14',999.99,1),(56,16,2,'Samsung Galaxy S23',899.99,2),(57,16,3,'Google Pixel 7',799.99,4),(58,16,7,'Huawei P50 Pro',899.99,2),(61,18,2,'Samsung Galaxy S23',899.99,1),(62,18,8,'Motorola Edge 30',649.99,2),(63,19,1,'iPhone 14',999.99,1),(64,19,2,'Samsung Galaxy S23',899.99,1),(65,19,3,'Google Pixel 7',799.99,1),(66,20,1,'iPhone 14',999.99,1),(67,20,2,'Samsung Galaxy S23',899.99,2),(68,20,3,'Google Pixel 7',799.99,3),(69,21,2,'Samsung Galaxy S23',899.99,3),(70,22,1,'iPhone 14',999.99,3),(71,22,2,'Samsung Galaxy S23',899.99,3),(72,23,1,'iPhone 14',999.99,1),(73,24,1,'iPhone 14',999.99,1),(74,25,3,'Google Pixel 7',799.99,1),(161,26,2,'Samsung Galaxy S23',899.99,2),(162,26,1,'iPhone 14',999.99,1),(168,27,1,'iPhone 14',999.99,3),(183,28,1,'iPhone 14',999.99,1),(184,28,2,'Samsung Galaxy S23',899.99,1),(185,28,7,'Huawei P50 Pro',899.99,1),(186,29,1,'iPhone 14',999.99,2),(187,30,1,'iPhone 14',999.99,1),(188,30,2,'Samsung Galaxy S23',899.99,1),(189,30,3,'Google Pixel 7',799.99,1),(190,30,7,'Huawei P50 Pro',899.99,1),(192,31,1,'iPhone 14',999.99,1),(193,31,2,'Samsung Galaxy S23',899.99,1),(194,31,3,'Google Pixel 7',799.99,1),(197,32,2,'Samsung Galaxy S23',899.99,9),(200,33,2,'Samsung Galaxy S23',899.99,1),(206,34,1,'iPhone 14',999.99,1),(216,35,15,'product example',4000.00,1),(217,36,2,'Samsung Galaxy S23',899.99,1),(218,36,1,'iPhone 14',999.99,1);
/*!40000 ALTER TABLE `sale_details` ENABLE KEYS */;

-- 
-- Definition of sales
-- 

DROP TABLE IF EXISTS `sales`;
CREATE TABLE IF NOT EXISTS `sales` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientName` varchar(255) NOT NULL,
  `SellerId` int(11) NOT NULL,
  `SellerName` varchar(255) NOT NULL DEFAULT '',
  `TotalSale` decimal(10,2) NOT NULL,
  `Currency` varchar(50) NOT NULL DEFAULT '',
  `PaymentMethod` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table sales
-- 

/*!40000 ALTER TABLE `sales` DISABLE KEYS */;
INSERT INTO `sales`(`Id`,`ClientName`,`SellerId`,`SellerName`,`TotalSale`,`Currency`,`PaymentMethod`,`CreatedAt`) VALUES(2,'Lael Medina',1,'Eren Jeager',3999.96,'USD','Debit Card','2024-10-27 10:41:01'),(10,'Eren Jeager',5,'Mikasa Ackerman',999.99,'EUR','Cash','2024-10-27 23:32:50'),(11,'Mikasa Ackerman',6,'Armin Arlert',899.99,'USD','Credit Card','2024-10-27 23:46:17'),(12,'Armin Arlert',6,'Armin Arlert',9299.89,'NIO','Cash','2024-10-27 23:48:16'),(13,'none2',1,'Eren Jeager',1499.98,'NIO','Debit Card','2024-10-28 02:53:06'),(14,'Lael Medina Mayorga',1,'Eren Jeager',2699.97,'USD','Debit Card','2024-10-31 17:19:03'),(15,'Lael Medina Mayorga',1,'Eren Jeager',2699.97,'USD','Debit Card','2024-10-31 17:28:02'),(16,'Lael Medina Mayorga',1,'Eren Jeager',7799.91,'USD','Debit Card','2024-10-31 17:57:57'),(17,'Lael Medina Mayorga',1,'Eren Jeager',1099.98,'NIO','Cash','2024-10-31 18:14:28'),(18,'admin',5,'Mikasa Ackerman',2199.97,'EUR','Credit Card','2024-10-31 18:16:04'),(19,'Eren Jeager',6,'Armin Arlert',2699.97,'USD','Debit Card','2024-10-31 18:26:50'),(20,'Lael Medina Mayorga',6,'Armin Arlert',5199.94,'USD','Debit Card','2024-10-31 18:45:03'),(21,'Eren Jeager',5,'Mikasa Ackerman',2699.97,'NIO','Cash','2024-11-05 15:08:14'),(22,'Lael Medina',1,'Eren Jeager',5699.94,'USD','Credit Card','2024-11-05 15:08:59'),(23,'Lael Medina Mayorga',1,'Eren Jeager',999.99,'EUR','Cash','2024-11-07 03:36:35'),(24,'Eren Jeager',5,'Mikasa Ackerman',999.99,'USD','Credit Card','2024-11-12 21:55:44'),(25,'admin',5,'Mikasa Ackerman',799.99,'USD','Debit Card','2024-11-13 18:31:35'),(26,'Lael Medina Mayorga',5,'Mikasa Ackerman',2799.97,'USD','Debit Card','2024-11-14 03:45:40'),(27,'admin',5,'Mikasa Ackerman',2999.97,'EUR','Cash','2024-11-14 05:35:12'),(28,'Lael Medina Mayorga',5,'Mikasa Ackerman',2799.97,'NIO','Debit Card','2024-11-14 06:47:54'),(29,'Lael Medina Mayorga',1,'Eren Jeager',1999.98,'USD','Credit Card','2024-11-14 06:50:16'),(30,'Lael Medina Mayorga',5,'Mikasa Ackerman',4199.95,'USD','Debit Card','2024-11-14 06:53:56'),(31,'Test Client',18,'Test Seller',3097.97,'NIO','Debit Card','2024-11-14 16:17:09'),(32,'Test Client',18,'Test Seller',8099.91,'USD','Credit Card','2024-11-18 15:20:35'),(33,'admin',5,'Mikasa Ackerman',899.99,'USD','Credit Card','2024-11-18 17:08:48'),(34,'Lael Medina Mayorga',5,'Mikasa Ackerman',999.99,'NIO','Cash','2024-11-18 18:18:10'),(35,'example client',20,'example seller',4000.00,'NIO','Credit Card','2024-11-18 19:31:21'),(36,'example client',20,'example seller',1899.98,'USD','Debit Card','2024-11-18 19:32:50');
/*!40000 ALTER TABLE `sales` ENABLE KEYS */;

-- 
-- Definition of tb_currency
-- 

DROP TABLE IF EXISTS `tb_currency`;
CREATE TABLE IF NOT EXISTS `tb_currency` (
  `id` int(11) NOT NULL,
  `currency` varchar(250) NOT NULL DEFAULT '',
  `acronym` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table tb_currency
-- 

/*!40000 ALTER TABLE `tb_currency` DISABLE KEYS */;
INSERT INTO `tb_currency`(`id`,`currency`,`acronym`) VALUES(0,'Cordobas','NIO'),(1,'Dolares','USD'),(2,'Euros','EUR');
/*!40000 ALTER TABLE `tb_currency` ENABLE KEYS */;

-- 
-- Definition of tb_seller
-- 

DROP TABLE IF EXISTS `tb_seller`;
CREATE TABLE IF NOT EXISTS `tb_seller` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(250) DEFAULT '',
  `Identity` varchar(250) DEFAULT '',
  `CreatedOn` date DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table tb_seller
-- 

/*!40000 ALTER TABLE `tb_seller` DISABLE KEYS */;
INSERT INTO `tb_seller`(`Id`,`Name`,`Identity`,`CreatedOn`) VALUES(1,'Eren Jeager','281-140203-1001C','2024-11-12 00:00:00'),(20,'example seller','281-140203-1001C','2024-11-18 00:00:00');
/*!40000 ALTER TABLE `tb_seller` ENABLE KEYS */;

-- 
-- Definition of users
-- 

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 
-- Dumping data for table users
-- 

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users`(`UserId`,`Username`,`PasswordHash`,`RoleId`) VALUES(1,'admin','$2a$11$VVSzZcc6qCSHbi751xm/seYt9.9cBLuFYJsjbnqXyWXAm1FrLy1gm',1),(19,'user','$2a$11$yBmj7RU/fmJWwqBlYv7VluMEcB6x1ngBPwsk4RUG0pLwOcKbj7w6.',2),(20,'admin2','$2a$11$8jAjdcuKEueVrbNBxhpX0OHqjtPPs0MvGoMoGREwrWx7PFR2xC/bi',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2024-11-18 19:44:23
-- Total time: 0:0:0:0:335 (d:h:m:s:ms)
