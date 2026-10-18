-- ================================================================
-- File   : database_setup.sql
-- Mô tả  : Khởi tạo database SOA cho project SOA-JWT
--          Bao gồm: tạo database, tạo bảng, chèn dữ liệu mẫu
-- Chạy   : SQL Server Management Studio hoặc Azure Data Studio
-- ================================================================

-- ----------------------------------------------------------------
-- 1. Tạo database (nếu chưa tồn tại)
-- ----------------------------------------------------------------
IF NOT EXISTS (
    SELECT name FROM sys.databases WHERE name = 'SOA'
)
BEGIN
    CREATE DATABASE SOA;
END
GO

USE SOA;
GO

-- ----------------------------------------------------------------
-- 2. Bảng Users (Bài thực hành 2 — JWT Authentication)
-- ----------------------------------------------------------------
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'Users'
)
BEGIN
    CREATE TABLE Users (
        IdUser   INT            IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(50)   NOT NULL UNIQUE,
        Password NVARCHAR(100)  NOT NULL,
        Token    NVARCHAR(MAX)  NULL
    );

    -- Dữ liệu mẫu
    INSERT INTO Users (UserName, Password, Token)
    VALUES
        ('admin',   '123456', ''),
        ('user01',  '123456', ''),
        ('user02',  '123456', '');
END
GO

-- ----------------------------------------------------------------
-- 3. Bảng Products (Bài thực hành 3 — Product Management)
-- ----------------------------------------------------------------
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'Products'
)
BEGIN
    CREATE TABLE Products (
        Id          INT             IDENTITY(1,1) PRIMARY KEY,
        Name        NVARCHAR(255)   NOT NULL,
        Description NVARCHAR(MAX)   NULL,
        Price       DECIMAL(10, 2)  NOT NULL DEFAULT 0,
        Quantity    INT             NOT NULL DEFAULT 0,
        CreatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt   DATETIME2       NOT NULL DEFAULT GETUTCDATE()
    );

    -- Dữ liệu mẫu
    INSERT INTO Products (Name, Description, Price, Quantity, CreatedAt, UpdatedAt)
    VALUES
        (N'Laptop Dell XPS 15',     N'Laptop cao cấp màn hình 15.6 inch, Intel Core i7',  25000000, 10, GETUTCDATE(), GETUTCDATE()),
        (N'MacBook Pro M3',         N'Laptop Apple chip M3, 16GB RAM, 512GB SSD',          45000000,  5, GETUTCDATE(), GETUTCDATE()),
        (N'iPhone 15 Pro',          N'Điện thoại Apple, chip A17 Pro, 256GB',              27000000, 20, GETUTCDATE(), GETUTCDATE()),
        (N'Samsung Galaxy S24',     N'Điện thoại Android cao cấp, 12GB RAM',               22000000, 15, GETUTCDATE(), GETUTCDATE()),
        (N'Tai nghe Sony WH-1000',  N'Tai nghe chống ồn chủ động, Bluetooth 5.0',           6500000, 30, GETUTCDATE(), GETUTCDATE());
END
GO

-- ----------------------------------------------------------------
-- 4. Kiểm tra kết quả
-- ----------------------------------------------------------------
SELECT * FROM Users;

SELECT * FROM Products;
