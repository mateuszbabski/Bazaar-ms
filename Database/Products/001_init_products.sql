IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Bazaar_Products')
BEGIN
    CREATE DATABASE Bazaar_Products;
END
GO

USE Bazaar_Products;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Products' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Products
    (
        Id                  UNIQUEIDENTIFIER    NOT NULL    DEFAULT NEWID(),
        CustomerId          UNIQUEIDENTIFIER    NOT NULL,
        Name                NVARCHAR(200)       NOT NULL,
        Description         NVARCHAR(2000)      NOT NULL,
        Category            NVARCHAR(100)       NOT NULL,
        Price               DECIMAL(18, 2)      NOT NULL,
        Currency            NVARCHAR(10)        NOT NULL,
        Unit                NVARCHAR(50)        NOT NULL,
        WeightPerUnitInKg   DECIMAL(18, 4)      NOT NULL,
        IsAvailable         BIT                 NOT NULL    DEFAULT 1,
        CONSTRAINT PK_Products PRIMARY KEY (Id)
    );

    CREATE INDEX IX_Products_CustomerId ON Products (CustomerId);
END
