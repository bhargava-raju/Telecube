CREATE TABLE Customer
(
    Id             UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    EntraObjectId  NVARCHAR(64)     NOT NULL,
    Email          NVARCHAR(256)    NOT NULL,
    DisplayName    NVARCHAR(256)    NOT NULL,

    CONSTRAINT UQ_Customer_EntraObjectId UNIQUE (EntraObjectId)
);
