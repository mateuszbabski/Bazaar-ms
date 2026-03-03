IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Customers' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Customers
    (
        Id              UNIQUEIDENTIFIER    NOT NULL    DEFAULT NEWID(),
        Name            NVARCHAR(100)       NOT NULL,
        LastName        NVARCHAR(100)       NOT NULL,
        Email           NVARCHAR(255)       NOT NULL,
        PasswordHash    NVARCHAR(512)       NOT NULL,
        Phone           NVARCHAR(20)        NOT NULL,
        Address         NVARCHAR(255)       NOT NULL,
        City            NVARCHAR(100)       NOT NULL,
        PostalCode      NVARCHAR(20)        NOT NULL,
        Country         NVARCHAR(100)       NOT NULL,
        Role            NVARCHAR(50)        NOT NULL    DEFAULT 'Customer',
        CONSTRAINT PK_Customers PRIMARY KEY (Id),
        CONSTRAINT UQ_Customers_Email UNIQUE (Email)
    );
END