CREATE DATABASE ElectronicShopDb;
GO
USE ElectronicShopDb;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20) UNIQUE,
    Address NVARCHAR(MAX),
    Role NVARCHAR(10) CHECK (Role IN ('Admin', 'User')) DEFAULT 'User',
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) UNIQUE NOT NULL,
    Description NVARCHAR(MAX),
);

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) UNIQUE NOT NULL,
    CategoryId INT NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) DEFAULT 0,
    Stock INT DEFAULT 0,
    ImageUrl NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
);

ALTER TABLE Products 
ADD CONSTRAINT FK_Products_Category FOREIGN KEY (CategoryId) REFERENCES Categories(Id);

CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    TotalPrice DECIMAL(10,2),
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Processing', 'Shipped', 'Cancelled')) DEFAULT 'Pending',
    OrderDate DATETIME DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(20) CHECK (PaymentMethod IN ('COD', 'Bank Transfer', 'Credit Card')) DEFAULT 'COD'
);

ALTER TABLE Orders 
ADD CONSTRAINT FK_Orders_User FOREIGN KEY (UserId) REFERENCES Users(Id);

CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) DEFAULT 0
);

ALTER TABLE OrderDetails 
ADD CONSTRAINT FK_Order_Details_Order FOREIGN KEY (OrderId) REFERENCES Orders(Id);

ALTER TABLE OrderDetails 
ADD CONSTRAINT FK_Order_Details_Product FOREIGN KEY (ProductId) REFERENCES Products(Id);