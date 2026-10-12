CREATE TABLE Category
(
    Id           UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name         NVARCHAR(100)    NOT NULL,
    Description  NVARCHAR(1000)   NOT NULL,

    CONSTRAINT UQ_Category_Name UNIQUE (Name)
);
