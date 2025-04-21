-- Chèn 2 user vào bảng Users
INSERT INTO Users (Name, Email, Password, Phone, Address, Role, CreatedAt)  
VALUES  
('Nguyen Le Phuong Thao', 'lethao13111991@gmail.com', 'hashed_password_1', '0944020338', 'Hanoi, Vietnam', 'User', GETDATE()),  
('Nguyen Van A', 'nguyenvana@example.com', 'hashed_password_1', '0987654321', 'Hanoi, Vietnam', 'User', GETDATE()),  
('Tran Thi B', 'tranthib@example.com', 'hashed_password_2', '0912345678', 'Ho Chi Minh, Vietnam', 'Admin', GETDATE());

-- Chèn 5 danh mục linh kiện điện tử vào bảng Categories
INSERT INTO Categories (Name, Description)  
VALUES  
('CPU', N'Bộ vi xử lý trung tâm'),  
('RAM', N'Bộ nhớ truy cập ngẫu nhiên'),  
('SSD', N'Ổ cứng thể rắn'),  
('Mainboard', N'Bo mạch chủ'),  
('VGA', N'Card đồ họa');

-- Chèn 7 sản phẩm cho danh mục CPU
INSERT INTO Products (Name, CategoryId, Description, Price, Stock, ImageUrl, CreatedAt)  
VALUES  
('Intel Core i9-13900K', 1, N'CPU Intel Core i9 thế hệ 13', 15000000, 10, 'image1.jpg', GETDATE()),  
('Intel Core i7-12700K', 1, N'CPU Intel Core i7 thế hệ 12', 10000000, 15, 'image2.jpg', GETDATE()),  
('AMD Ryzen 9 7900X', 1, N'CPU AMD Ryzen 9 thế hệ mới', 14000000, 12, 'image3.jpg', GETDATE()),  
('AMD Ryzen 7 7700X', 1, N'CPU AMD Ryzen 7 mạnh mẽ', 9000000, 20, 'image4.jpg', GETDATE()),  
('Intel Core i5-12600K', 1, N'CPU Intel Core i5 thế hệ 12', 7000000, 18, 'image5.jpg', GETDATE()),  
('AMD Ryzen 5 7600X', 1, N'CPU AMD Ryzen 5 tiết kiệm điện', 6500000, 22, 'image6.jpg', GETDATE()),  
('Intel Core i3-12100F', 1, N'CPU Intel Core i3 giá rẻ', 3000000, 25, 'image7.jpg', GETDATE());

-- Chèn 7 sản phẩm cho danh mục RAM
INSERT INTO Products (Name, CategoryId, Description, Price, Stock, ImageUrl, CreatedAt)  
VALUES  
('Corsair Vengeance 16GB DDR5', 2, N'RAM DDR5 tốc độ cao', 2500000, 30, 'ram1.jpg', GETDATE()),  
('G.Skill Trident Z 32GB DDR4', 2, N'RAM gaming mạnh mẽ', 3200000, 25, 'ram2.jpg', GETDATE()),  
('Kingston Fury Beast 16GB DDR4', 2, N'RAM Kingston hiệu năng cao', 1800000, 40, 'ram3.jpg', GETDATE()),  
('Crucial Ballistix 16GB DDR4', 2, N'RAM DDR4 cho gaming', 2000000, 35, 'ram4.jpg', GETDATE()),  
('ADATA XPG Spectrix D60G 16GB DDR4', 2, N'RAM RGB đẹp mắt', 2200000, 28, 'ram5.jpg', GETDATE()),  
('Patriot Viper Steel 32GB DDR4', 2, N'RAM dung lượng lớn', 4000000, 15, 'ram6.jpg', GETDATE()),  
('Samsung 8GB DDR4', 2, N'RAM phổ thông giá rẻ', 800000, 50, 'ram7.jpg', GETDATE());

-- Chèn 7 sản phẩm cho danh mục SSD
INSERT INTO Products (Name, CategoryId, Description, Price, Stock, ImageUrl, CreatedAt)  
VALUES  
('Samsung 970 EVO Plus 1TB', 3, N'SSD NVMe tốc độ cao', 3500000, 20, 'ssd1.jpg', GETDATE()),  
('WD Black SN850 1TB', 3, N'SSD gaming PCIe 4.0', 4000000, 18, 'ssd2.jpg', GETDATE()),  
('Crucial P5 Plus 1TB', 3, N'SSD NVMe PCIe 4.0', 3200000, 22, 'ssd3.jpg', GETDATE()),  
('ADATA XPG SX8200 Pro 1TB', 3, N'SSD gaming giá rẻ', 2800000, 30, 'ssd4.jpg', GETDATE()),  
('Kingston NV2 500GB', 3, N'SSD phổ thông tốc độ cao', 1500000, 35, 'ssd5.jpg', GETDATE()),  
('WD Blue SN570 1TB', 3, N'SSD NVMe phổ thông', 2500000, 25, 'ssd6.jpg', GETDATE()),  
('Samsung 980 Pro 2TB', 3, N'SSD PCIe 4.0 cao cấp', 7000000, 10, 'ssd7.jpg', GETDATE());

-- Chèn 7 sản phẩm cho danh mục Mainboard
INSERT INTO Products (Name, CategoryId, Description, Price, Stock, ImageUrl, CreatedAt)  
VALUES  
('ASUS ROG Strix Z690-E', 4, N'Mainboard cao cấp cho Intel', 8000000, 12, 'mb1.jpg', GETDATE()),  
('MSI MEG X570 Godlike', 4, N'Mainboard cao cấp cho AMD', 10000000, 8, 'mb2.jpg', GETDATE()),  
('Gigabyte B550 AORUS PRO', 4, N'Mainboard tầm trung cho Ryzen', 5000000, 15, 'mb3.jpg', GETDATE()),  
('ASUS TUF Gaming B660M', 4, N'Mainboard gaming cho Intel', 4000000, 20, 'mb4.jpg', GETDATE()),  
('ASRock B450 Steel Legend', 4, N'Mainboard giá rẻ cho Ryzen', 3000000, 25, 'mb5.jpg', GETDATE()),  
('MSI MAG B550M Mortar', 4, N'Mainboard Micro-ATX cho Ryzen', 4500000, 18, 'mb6.jpg', GETDATE()),  
('Gigabyte Z790 AORUS Elite', 4, N'Mainboard cao cấp Intel', 8500000, 10, 'mb7.jpg', GETDATE());

-- Chèn 7 sản phẩm cho danh mục VGA (Card đồ họa)
INSERT INTO Products (Name, CategoryId, Description, Price, Stock, ImageUrl, CreatedAt)  
VALUES  
('NVIDIA RTX 4090', 5, N'Card đồ họa mạnh nhất hiện tại', 50000000, 5, 'vga1.jpg', GETDATE()),  
('AMD Radeon RX 7900 XTX', 5, N'Card đồ họa cao cấp AMD', 40000000, 7, 'vga2.jpg', GETDATE()),  
('NVIDIA RTX 4080', 5, N'Card đồ họa mạnh mẽ', 35000000, 10, 'vga3.jpg', GETDATE()),  
('AMD Radeon RX 7800 XT', 5, N'Card đồ họa gaming tầm trung', 20000000, 15, 'vga4.jpg', GETDATE()),  
('NVIDIA RTX 4070 Ti', 5, N'Card đồ họa hiệu năng cao', 25000000, 12, 'vga5.jpg', GETDATE()),  
('AMD Radeon RX 7600', 5, N'Card đồ họa phổ thông', 12000000, 20, 'vga6.jpg', GETDATE()),  
('NVIDIA RTX 4060', 5, N'Card đồ họa tầm trung', 15000000, 18, 'vga7.jpg', GETDATE());

-- Thêm 5 đơn hàng vào bảng Orders
INSERT INTO Orders (UserId, TotalPrice, Status, OrderDate, PaymentMethod)  
VALUES  
(1, 15000000, N'Pending', GETDATE(), N'COD'),  
(2, 10000000, N'Processing', GETDATE(), N'Bank Transfer'),  
(3, 20000000, N'Shipped', GETDATE(), N'Credit Card'),  
(2, 5000000, N'Cancelled', GETDATE(), N'COD'),  
(3, 25000000, N'Pending', GETDATE(), N'Bank Transfer');

-- Thêm 5 chi tiết đơn hàng vào bảng Order_Details
INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)  
VALUES  
(1, 1, 1, 15000000),  
(1, 8, 2, 5000000),  
(2, 15, 1, 20000000),  
(2, 22, 3, 1500000),  
(2, 30, 1, 25000000);