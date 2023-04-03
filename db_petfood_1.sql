-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 03 Apr 2023 pada 04.04
-- Versi server: 10.4.25-MariaDB
-- Versi PHP: 8.1.10

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
-- Struktur dari tabel `order_detail`
--

CREATE TABLE `order_detail` (
  `order_code` varchar(10) NOT NULL,
  `product_code` varchar(10) NOT NULL,
  `qty` int(11) NOT NULL,
  `subtotal` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `order_detail`
--

INSERT INTO `order_detail` (`order_code`, `product_code`, `qty`, `subtotal`) VALUES
('ORC001', 'PRF001', 1, 100000),
('ORC001', 'PRF002', 2, 40000),
('ORC002', 'PRF001', 2, 100000),
('ORC003', 'PRF001', 2, 100000),
('ORC004', 'PRF001', 2, 10000);

--
-- Trigger `order_detail`
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
-- Struktur dari tabel `order_header`
--

CREATE TABLE `order_header` (
  `order_code` varchar(10) NOT NULL,
  `order_date` date NOT NULL,
  `order_total` decimal(10,0) NOT NULL,
  `user_code` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `order_header`
--

INSERT INTO `order_header` (`order_code`, `order_date`, `order_total`, `user_code`) VALUES
('ORC001', '2023-03-27', 140000, 'us013'),
('ORC002', '2023-03-30', 100000, 'us013'),
('ORC003', '2023-03-30', 100000, 'us013'),
('ORC004', '2023-03-30', 10000, 'us013'),
('ORC005', '2023-04-03', 100000, 'us013');

--
-- Trigger `order_header`
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
-- Struktur dari tabel `product`
--

CREATE TABLE `product` (
  `product_code` varchar(10) NOT NULL,
  `product_name` varchar(20) NOT NULL,
  `product_category` varchar(10) NOT NULL,
  `product_stock` int(11) NOT NULL,
  `product_price` decimal(10,0) NOT NULL,
  `user_code` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `product`
--

INSERT INTO `product` (`product_code`, `product_name`, `product_category`, `product_stock`, `product_price`, `user_code`) VALUES
('PRC003', 'Cat Choize', 'Cat Food', 100, 40000, 'us013'),
('PRF001', 'Whiskas Cat ', 'Cat Food', 44, 30000, 'us013'),
('PRF002', 'Birbo Cat', 'Cat Food', 48, 20000, 'us013');

--
-- Trigger `product`
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
-- Struktur dari tabel `users`
--

CREATE TABLE `users` (
  `user_code` varchar(10) NOT NULL,
  `user_name` varchar(20) NOT NULL,
  `user_password` varchar(10) NOT NULL,
  `user_dob` date NOT NULL,
  `user_phone` bigint(20) NOT NULL,
  `user_gender` enum('Laki - Laki','Perempuan') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `users`
--

INSERT INTO `users` (`user_code`, `user_name`, `user_password`, `user_dob`, `user_phone`, `user_gender`) VALUES
('us012', 'Naufal Febryansyah', '213456', '2002-02-04', 81287594870, 'Laki - Laki'),
('us013', 'gegewp', 'gegewp123', '2003-03-05', 81318563435, 'Laki - Laki');

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `order_detail`
--
ALTER TABLE `order_detail`
  ADD PRIMARY KEY (`order_code`,`product_code`),
  ADD KEY `fkprod` (`product_code`);

--
-- Indeks untuk tabel `order_header`
--
ALTER TABLE `order_header`
  ADD PRIMARY KEY (`order_code`),
  ADD KEY `fkoh` (`user_code`);

--
-- Indeks untuk tabel `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`product_code`),
  ADD KEY `fkpr` (`user_code`);

--
-- Indeks untuk tabel `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_code`);

--
-- Ketidakleluasaan untuk tabel pelimpahan (Dumped Tables)
--

--
-- Ketidakleluasaan untuk tabel `order_detail`
--
ALTER TABLE `order_detail`
  ADD CONSTRAINT `fkorder` FOREIGN KEY (`order_code`) REFERENCES `order_header` (`order_code`),
  ADD CONSTRAINT `fkprod` FOREIGN KEY (`product_code`) REFERENCES `product` (`product_code`);

--
-- Ketidakleluasaan untuk tabel `order_header`
--
ALTER TABLE `order_header`
  ADD CONSTRAINT `fkoh` FOREIGN KEY (`USER_Code`) REFERENCES `users` (`USER_Code`);

--
-- Ketidakleluasaan untuk tabel `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `fkpr` FOREIGN KEY (`USER_Code`) REFERENCES `users` (`USER_Code`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
