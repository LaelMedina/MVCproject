-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 31, 2024 at 06:17 PM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.0.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `productsdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `paymentmethods`
--

CREATE TABLE `paymentmethods` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `paymentmethods`
--

INSERT INTO `paymentmethods` (`Id`, `Name`) VALUES
(1, 'Cash'),
(2, 'Credit Card'),
(3, 'Debit Card');

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `currency` int(11) NOT NULL,
  `Stock` int(11) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`Id`, `Name`, `Description`, `Price`, `currency`, `Stock`, `CreatedAt`) VALUES
(1, 'iPhone 14', 'Latest Apple iPhone with A15 Bionic chip', '999.99', 1, 75, '2024-09-01 10:00:00'),
(2, 'Samsung Galaxy S23', 'Newest Samsung Galaxy model with Snapdragon processor', '899.99', 1, 56, '2024-09-01 10:05:00'),
(3, 'Google Pixel 7', 'Google Pixel with the latest Android version', '799.99', 1, 80, '2024-09-01 10:10:00'),
(4, 'OnePlus 10 Pro', 'High-end OnePlus phone with fast charging', '699.99', 1, 100, '2024-09-01 10:15:00'),
(5, 'Xiaomi Mi 11', 'Xiaomi phone with excellent camera quality', '499.99', 1, 100, '2024-09-01 10:20:00'),
(6, 'Sony Xperia 1 IV', 'Sony Xperia phone with 4K display', '1199.99', 1, 100, '2024-09-01 10:25:00'),
(7, 'Huawei P50 Pro', 'Huawei phone with powerful camera setup', '899.99', 1, 98, '2024-09-01 10:30:00'),
(8, 'Motorola Edge 30', 'Motorola smartphone with curved display', '649.99', 1, 98, '2024-09-01 10:35:00'),
(9, 'Nokia G50', 'Affordable Nokia smartphone with 5G connectivity', '299.99', 1, 88, '2024-09-01 10:40:00'),
(10, 'Asus ROG Phone 6', 'Gaming smartphone with cooling system', '1099.99', 1, 100, '2024-09-01 10:45:00'),
(11, 'Oppo Find X5', 'Oppo phone with AMOLED display', '749.99', 1, 98, '2024-09-01 10:50:00'),
(12, 'Vivo X80', 'Vivo phone with advanced photography features', '829.99', 1, 100, '2024-09-01 10:55:00'),
(13, 'Realme GT 2', 'Realme smartphone with flagship specs', '599.99', 1, 99, '2024-09-01 11:00:00'),
(14, 'ZTE Axon 30', 'ZTE phone with under-display camera.', '499.99', 1, 89, '2024-09-01 11:05:00');

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `RoleId` int(11) NOT NULL,
  `RoleName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`RoleId`, `RoleName`) VALUES
(1, 'admin'),
(2, 'user');

-- --------------------------------------------------------

--
-- Table structure for table `sales`
--

CREATE TABLE `sales` (
  `Id` int(11) NOT NULL,
  `ClientName` varchar(255) NOT NULL,
  `TotalSale` decimal(10,2) NOT NULL,
  `Currency` varchar(50) NOT NULL DEFAULT '',
  `PaymentMethod` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sales`
--

INSERT INTO `sales` (`Id`, `ClientName`, `TotalSale`, `Currency`, `PaymentMethod`, `CreatedAt`) VALUES
(2, 'Lael Medina', '3999.96', 'USD', 'Debit Card', '2024-10-27 10:41:01'),
(10, 'Eren Jeager', '999.99', 'EUR', 'Cash', '2024-10-27 23:32:50'),
(11, 'Mikasa Ackerman', '899.99', 'USD', 'Credit Card', '2024-10-27 23:46:17'),
(12, 'Armin Arlert', '9299.89', 'NIO', 'Cash', '2024-10-27 23:48:16'),
(13, 'none2', '1499.98', 'NIO', 'Debit Card', '2024-10-28 02:53:06'),
(14, 'Lael Medina Mayorga', '2699.97', 'USD', 'Debit Card', '2024-10-31 17:19:03'),
(15, 'Lael Medina Mayorga', '2699.97', 'USD', 'Debit Card', '2024-10-31 17:28:02'),
(16, 'Lael Medina Mayorga', '7799.91', 'USD', 'Debit Card', '2024-10-31 17:57:57'),
(17, 'Lael Medina Mayorga', '1099.98', 'NIO', 'Cash', '2024-10-31 18:14:28'),
(18, 'admin', '2199.97', 'EUR', 'Credit Card', '2024-10-31 18:16:04');

-- --------------------------------------------------------

--
-- Table structure for table `sale_details`
--

CREATE TABLE `sale_details` (
  `Id` int(11) NOT NULL,
  `SaleId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `ProductName` varchar(250) DEFAULT NULL,
  `Price` decimal(10,2) DEFAULT NULL,
  `Units` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sale_details`
--

INSERT INTO `sale_details` (`Id`, `SaleId`, `ProductId`, `ProductName`, `Price`, `Units`) VALUES
(6, 10, 1, 'iPhone 14', '0.00', 0),
(7, 11, 2, 'Samsung Galaxy S23', '899.99', 1),
(8, 12, 3, 'Google Pixel 7', '799.99', 7),
(9, 12, 2, 'Samsung Galaxy S23', '899.99', 3),
(10, 12, 1, 'iPhone 14', '999.99', 1),
(37, 13, 1, 'iPhone 14', '999.99', 1),
(38, 13, 2, 'Samsung Galaxy S23', '899.99', 1),
(39, 13, 3, 'Google Pixel 7', '799.99', 1),
(40, 13, 9, 'Nokia G50', '299.99', 4),
(41, 13, 14, 'ZTE Axon 30', '499.99', 5),
(42, 13, 11, 'Oppo Find X5', '749.99', 2),
(43, 2, 1, 'iPhone 14', '999.99', 4),
(49, 14, 2, 'Samsung Galaxy S23', '899.99', 1),
(50, 14, 1, 'iPhone 14', '999.99', 1),
(51, 14, 3, 'Google Pixel 7', '799.99', 1),
(52, 15, 1, 'iPhone 14', '999.99', 1),
(53, 15, 2, 'Samsung Galaxy S23', '899.99', 1),
(54, 15, 3, 'Google Pixel 7', '799.99', 1),
(55, 16, 1, 'iPhone 14', '999.99', 1),
(56, 16, 2, 'Samsung Galaxy S23', '899.99', 2),
(57, 16, 3, 'Google Pixel 7', '799.99', 4),
(58, 16, 7, 'Huawei P50 Pro', '899.99', 2),
(59, 17, 14, 'ZTE Axon 30', '499.99', 1),
(60, 17, 13, 'Realme GT 2', '599.99', 1),
(61, 18, 2, 'Samsung Galaxy S23', '899.99', 1),
(62, 18, 8, 'Motorola Edge 30', '649.99', 2);

-- --------------------------------------------------------

--
-- Table structure for table `tb_currency`
--

CREATE TABLE `tb_currency` (
  `id` int(11) NOT NULL,
  `currency` varchar(250) NOT NULL DEFAULT '',
  `acronym` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tb_currency`
--

INSERT INTO `tb_currency` (`id`, `currency`, `acronym`) VALUES
(0, 'Cordobas', 'NIO'),
(1, 'Dolares', 'USD'),
(2, 'Euros', 'EUR');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `UserId` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `RoleId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserId`, `Username`, `PasswordHash`, `RoleId`) VALUES
(1, 'admin', '$2a$11$VVSzZcc6qCSHbi751xm/seYt9.9cBLuFYJsjbnqXyWXAm1FrLy1gm', 1),
(2, 'user', '$2a$11$ThRRjwhJHwYQjMVI1TtIZe.Qn/ZkoSOTqQzxqhkexVlY1m632XYqy', 2),
(4, 'user2', '123', 2),
(8, 'admin3', '$2a$11$GtQPNFHzaWFI6Q3FsxePVuIJAdAp3RCanLLyB3vtukgjnilsTeUlW', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `paymentmethods`
--
ALTER TABLE `paymentmethods`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`RoleId`) USING BTREE;

--
-- Indexes for table `sales`
--
ALTER TABLE `sales`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `sale_details`
--
ALTER TABLE `sale_details`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `sale_details_ibfk_2` (`ProductId`),
  ADD KEY `sale_details_ibfk_1` (`SaleId`);

--
-- Indexes for table `tb_currency`
--
ALTER TABLE `tb_currency`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `paymentmethods`
--
ALTER TABLE `paymentmethods`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `roles`
--
ALTER TABLE `roles`
  MODIFY `RoleId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `sales`
--
ALTER TABLE `sales`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT for table `sale_details`
--
ALTER TABLE `sale_details`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=63;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `sale_details`
--
ALTER TABLE `sale_details`
  ADD CONSTRAINT `sale_details_ibfk_1` FOREIGN KEY (`SaleId`) REFERENCES `sales` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `sale_details_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
