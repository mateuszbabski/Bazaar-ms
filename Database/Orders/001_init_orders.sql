IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Bazaar_Orders')
BEGIN
    CREATE DATABASE Bazaar_Orders;
END
GO

USE Bazaar_Orders;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Orders' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Orders
    (
        Id              UNIQUEIDENTIFIER    NOT NULL    DEFAULT NEWID(),
        CustomerId      UNIQUEIDENTIFIER    NOT NULL,
        TotalPrice      DECIMAL(18, 2)      NOT NULL,
        Currency        NVARCHAR(10)        NOT NULL,
        TotalWeightInKg DECIMAL(18, 4)      NOT NULL,
        Status          NVARCHAR(20)        NOT NULL    DEFAULT 'Pending',
        CreatedAt       DATETIME2           NOT NULL    DEFAULT GETUTCDATE(),
        CONSTRAINT PK_Orders PRIMARY KEY (Id)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'OrderItems' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE OrderItems
    (
        Id              UNIQUEIDENTIFIER    NOT NULL    DEFAULT NEWID(),
        OrderId         UNIQUEIDENTIFIER    NOT NULL,
        ProductId       UNIQUEIDENTIFIER    NOT NULL,
        ShopId          UNIQUEIDENTIFIER    NOT NULL,
        Name            NVARCHAR(255)       NOT NULL,
        Quantity        INT                 NOT NULL,
        Price           DECIMAL(18, 2)      NOT NULL,
        Currency        NVARCHAR(10)        NOT NULL,
        WeightInKg      DECIMAL(18, 4)      NOT NULL,
        CONSTRAINT PK_OrderItems PRIMARY KEY (Id),
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
    );
END
