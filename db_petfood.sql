-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 14, 2023 at 07:52 AM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_petfood`
--

-- --------------------------------------------------------

--
-- Table structure for table `order_detail`
--

CREATE TABLE `order_detail` (
  `order_code` varchar(10) NOT NULL,
  `product_code` varchar(10) NOT NULL,
  `qty` int(11) NOT NULL,
  `subtotal` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Triggers `order_detail`
--
DELIMITER $$
CREATE TRIGGER `trgInsertTransDetail` AFTER INSERT ON `order_detail` FOR EACH ROW BEGIN
SET @order_code = NEW.order_code;
SET @sub = NEW.subtotal;
UPDATE order_header
SET order_total = order_total + @sub
WHERE order_code = @order_code;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trgStockUpdate` AFTER INSERT ON `order_detail` FOR EACH ROW BEGIN
SET @qty = NEW.qty;
SET @product_code = NEW.product_code;
SET @product_stock = (SELECT product_stock FROM product WHERE product_code =
@product_code);
SET @StockNow = @product_stock - @qty;
IF @StockNow < 0 THEN
SIGNAL sqlstate '45001' set message_text = "No way ! You\r\n\r\ncannot do this !";
ELSE
UPDATE product
SET product_stock = @StockNow
WHERE product_code = @product_code;
END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `order_header`
--

CREATE TABLE `order_header` (
  `order_code` varchar(10) NOT NULL,
  `order_date` date NOT NULL,
  `order_total` decimal(10,0) NOT NULL,
  `user_code` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Triggers `order_header`
--
DELIMITER $$
CREATE TRIGGER `auto_increment_code_order` BEFORE INSERT ON `order_header` FOR EACH ROW BEGIN
    IF NEW.order_code IS NULL OR NEW.order_code = '' THEN
        SET @max_code = (SELECT MAX(SUBSTRING(order_code, 4)+0) FROM order_header);
        SET NEW.order_code = CONCAT('ORC', LPAD(IFNULL(@max_code+1, 1), 3, '0'));
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

CREATE TABLE `product` (
  `product_code` varchar(10) NOT NULL,
  `product_name` varchar(20) NOT NULL,
  `product_category` varchar(10) NOT NULL,
  `product_stock` int(11) NOT NULL,
  `product_price` decimal(10,0) NOT NULL,
  `user_code` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `product`
--

INSERT INTO `product` (`product_code`, `product_name`, `product_category`, `product_stock`, `product_price`, `user_code`) VALUES
('PRC001', 'Whiskas Cat ', 'Cat Food', 195, 30000, 'us012'),
('PRC002', 'Birbo Cat', 'Cat Food', 100, 20000, 'us013'),
('PRC003', 'Cat Choize', 'Cat Food', 100, 20000, 'us013'),
('PRC004', 'Grain Cat', 'Cat Food', 200, 25000, 'us012'),
('PRC005', 'Dog Choize', 'Dog Food', 200, 12000, 'us013'),
('PRC006', 'Prins Dog', 'Dog Food', 200, 50000, 'us012'),
('PRC007', 'Proplan Dog', 'Dog Food', 150, 60000, 'us013'),
('PRC008', 'Takari Fish', 'Fish Food', 250, 15000, 'us012'),
('PRC009', 'Sakura Fish', 'Fish Food', 150, 30000, 'us013'),
('PRC010', 'Hi-Red Fish', 'Fish Food', 200, 20000, 'us012'),
('PRC011', 'Sankoi Fish', 'Fish Food', 200, 25000, 'us012'),
('PRC012', 'Asahi Fish', 'Fish Food', 200, 50000, 'us013');

--
-- Triggers `product`
--
DELIMITER $$
CREATE TRIGGER `auto_increment_code_product` BEFORE INSERT ON `product` FOR EACH ROW BEGIN
    IF NEW.product_code IS NULL OR NEW.product_code = '' THEN
        SET @max_code = (SELECT MAX(SUBSTRING(product_code, 4)+0) FROM product);
        SET NEW.product_code = CONCAT('PRC', LPAD(IFNULL(@max_code+1, 1), 3, '0'));
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_code` varchar(10) NOT NULL,
  `user_name` varchar(20) NOT NULL,
  `user_password` varchar(10) NOT NULL,
  `user_dob` date NOT NULL,
  `user_phone` bigint(20) NOT NULL,
  `user_gender` enum('Laki - Laki','Perempuan') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`user_code`, `user_name`, `user_password`, `user_dob`, `user_phone`, `user_gender`) VALUES
('us012', 'kianix', '213456', '2002-02-04', 81287594870, 'Laki - Laki'),
('us013', 'gegewp', 'gegewp123', '2003-03-05', 81318563435, 'Laki - Laki');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `order_detail`
--
ALTER TABLE `order_detail`
  ADD PRIMARY KEY (`order_code`,`product_code`),
  ADD KEY `fkprod` (`product_code`);

--
-- Indexes for table `order_header`
--
ALTER TABLE `order_header`
  ADD PRIMARY KEY (`order_code`),
  ADD KEY `fkoh` (`user_code`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`product_code`),
  ADD KEY `fkpr` (`user_code`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_code`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `order_detail`
--
ALTER TABLE `order_detail`
  ADD CONSTRAINT `fkorder` FOREIGN KEY (`order_code`) REFERENCES `order_header` (`order_code`),
  ADD CONSTRAINT `fkprod` FOREIGN KEY (`product_code`) REFERENCES `product` (`product_code`);

--
-- Constraints for table `order_header`
--
ALTER TABLE `order_header`
  ADD CONSTRAINT `fkoh` FOREIGN KEY (`user_code`) REFERENCES `users` (`user_code`);

--
-- Constraints for table `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `fkpr` FOREIGN KEY (`user_code`) REFERENCES `users` (`user_code`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
