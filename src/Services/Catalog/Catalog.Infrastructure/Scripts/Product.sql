-- Run Category.sql first: the foreign key below needs the Category table.
CREATE TABLE Product
(
    Id           UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CategoryId   UNIQUEIDENTIFIER NOT NULL,
    Sku          NVARCHAR(50)     NOT NULL,
    Name         NVARCHAR(200)    NOT NULL,
    Description  NVARCHAR(2000)   NOT NULL,
    Price        DECIMAL(18, 2)   NOT NULL,

    CONSTRAINT UQ_Product_Sku UNIQUE (Sku),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES Category (Id),
    CONSTRAINT CK_Product_Price CHECK (Price >= 0)
);
