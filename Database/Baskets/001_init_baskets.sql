IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Baskets' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE Baskets
    (
        Id                  UNIQUEIDENTIFIER    NOT NULL    DEFAULT NEWID(),
        CustomerId          UNIQUEIDENTIFIER    NOT NULL,
        TotalPrice          DECIMAL(18,2)       NOT NULL    DEFAULT 0,
        Currency            NVARCHAR(10)        NOT NULL    DEFAULT 'PLN',
        TotalWeightInKg     DECIMAL(18,4)       NOT NULL    DEFAULT 0,
        CONSTRAINT PK_Baskets PRIMARY KEY (Id)
    );

    CREATE UNIQUE INDEX IX_Baskets_CustomerId ON Baskets (CustomerId);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BasketItems' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE BasketItems
    (
        Id              UNIQUEIDENTIFIER    NOT NULL    DEFAULT NEWID(),
        ProductId       UNIQUEIDENTIFIER    NOT NULL,
        ShopId          UNIQUEIDENTIFIER    NOT NULL,
        BasketId        UNIQUEIDENTIFIER    NOT NULL,
        Name            NVARCHAR(200)       NOT NULL,
        Quantity        INT                 NOT NULL    DEFAULT 1,
        Price           DECIMAL(18,2)       NOT NULL,
        Currency        NVARCHAR(10)        NOT NULL,
        BasePrice       DECIMAL(18,2)       NOT NULL,
        BaseCurrency    NVARCHAR(10)        NOT NULL,
        WeightInKg      DECIMAL(18,4)       NOT NULL    DEFAULT 0,
        CONSTRAINT PK_BasketItems PRIMARY KEY (Id),
        CONSTRAINT FK_BasketItems_Baskets FOREIGN KEY (BasketId)
            REFERENCES Baskets (Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_BasketItems_BasketId ON BasketItems (BasketId);
    CREATE INDEX IX_BasketItems_ProductId ON BasketItems (ProductId);
END
