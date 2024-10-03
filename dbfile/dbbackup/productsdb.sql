-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 03, 2024 at 02:39 PM
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
  `Stock` int(11) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`Id`, `Name`, `Description`, `Price`, `Stock`, `CreatedAt`) VALUES
(1, 'iPhone 14', 'Latest Apple iPhone with A15 Bionic chip', '999.99', 95, '2024-09-01 10:00:00'),
(2, 'Samsung Galaxy S23', 'Newest Samsung Galaxy model with Snapdragon processor', '899.99', 92, '2024-09-01 10:05:00'),
(3, 'Google Pixel 7', 'Google Pixel with the latest Android version', '799.99', 97, '2024-09-01 10:10:00'),
(4, 'OnePlus 10 Pro', 'High-end OnePlus phone with fast charging', '699.99', 100, '2024-09-01 10:15:00'),
(5, 'Xiaomi Mi 11', 'Xiaomi phone with excellent camera quality', '499.99', 100, '2024-09-01 10:20:00'),
(6, 'Sony Xperia 1 IV', 'Sony Xperia phone with 4K display', '1199.99', 100, '2024-09-01 10:25:00'),
(7, 'Huawei P50 Pro', 'Huawei phone with powerful camera setup', '899.99', 100, '2024-09-01 10:30:00'),
(8, 'Motorola Edge 30', 'Motorola smartphone with curved display', '649.99', 100, '2024-09-01 10:35:00'),
(9, 'Nokia G50', 'Affordable Nokia smartphone with 5G connectivity', '299.99', 100, '2024-09-01 10:40:00'),
(10, 'Asus ROG Phone 6', 'Gaming smartphone with cooling system', '1099.99', 100, '2024-09-01 10:45:00'),
(11, 'Oppo Find X5', 'Oppo phone with AMOLED display', '749.99', 100, '2024-09-01 10:50:00'),
(12, 'Vivo X80', 'Vivo phone with advanced photography features', '829.99', 100, '2024-09-01 10:55:00'),
(13, 'Realme GT 2', 'Realme smartphone with flagship specs', '599.99', 100, '2024-09-01 11:00:00'),
(14, 'ZTE Axon 30', 'ZTE phone with under-display camera', '499.99', 100, '2024-09-01 11:05:00'),
(15, 'Lenovo Legion Duel 2', 'Gaming phone with dual cooling fans', '999.99', 100, '2024-09-01 11:10:00'),
(16, 'Alcatel 1S', 'Budget Alcatel Phone With Base Features', '99.99', 100, '2024-09-01 11:15:00');

-- --------------------------------------------------------

--
-- Table structure for table `rol`
--

CREATE TABLE `rol` (
  `RolId` int(11) NOT NULL,
  `RolName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `rol`
--

INSERT INTO `rol` (`RolId`, `RolName`) VALUES
(1, 'admin'),
(2, 'user');

-- --------------------------------------------------------

--
-- Table structure for table `sales`
--

CREATE TABLE `sales` (
  `Id` int(11) NOT NULL,
  `ClientName` varchar(255) NOT NULL,
  `SaleContent` text NOT NULL,
  `ProductSoldId` int(11) NOT NULL,
  `TotalUnits` int(11) NOT NULL,
  `TotalSale` decimal(10,2) NOT NULL,
  `PaymentMethod` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sales`
--

INSERT INTO `sales` (`Id`, `ClientName`, `SaleContent`, `ProductSoldId`, `TotalUnits`, `TotalSale`, `PaymentMethod`, `CreatedAt`) VALUES
(1, 'Lael Medina', 'iPhone 14', 0, 1, '999.99', 'Debit Card', '2024-09-08 17:07:23'),
(2, 'Lael Medina', 'Samsung Galaxy S23', 0, 1, '899.99', 'Debit Card', '2024-09-08 17:10:24'),
(3, 'Lael Medina', 'Google Pixel 7', 3, 1, '799.99', 'Debit Card', '2024-09-08 18:42:07'),
(5, 'Lael Medina', 'Samsung Galaxy S23', 2, 1, '899.99', 'Debit Card', '2024-09-08 18:50:46'),
(7, 'Dorian Figueroa', 'iPhone 15 pro max', 20, 2, '3000.00', 'Debit Card', '2024-09-09 09:54:22'),
(8, 'Lael Medina', 'Samsung Galaxy S23', 2, 3, '2699.97', 'Debit Card', '2024-10-02 13:12:02'),
(9, 'Lael Medina', 'Samsung Galaxy S23', 2, 3, '2699.97', 'Debit Card', '2024-10-02 13:26:20');

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
(2, 'user', '$2a$11$ThRRjwhJHwYQjMVI1TtIZe.Qn/ZkoSOTqQzxqhkexVlY1m632XYqy', 2);

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
-- Indexes for table `rol`
--
ALTER TABLE `rol`
  ADD PRIMARY KEY (`RolId`);

--
-- Indexes for table `sales`
--
ALTER TABLE `sales`
  ADD PRIMARY KEY (`Id`);

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
-- AUTO_INCREMENT for table `rol`
--
ALTER TABLE `rol`
  MODIFY `RolId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `sales`
--
ALTER TABLE `sales`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
