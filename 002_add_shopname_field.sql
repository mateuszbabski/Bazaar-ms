IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Customers' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    ALTER TABLE Customers 
        ADD ShopName NVARCHAR(255) NULL;
END