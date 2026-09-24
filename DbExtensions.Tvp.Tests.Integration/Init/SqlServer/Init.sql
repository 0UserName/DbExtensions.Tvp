-- -------------------------------------------------
-- 1. Create Schema
-- -------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM sys.schemas
    WHERE name = N'dbo'
)
BEGIN
    EXEC(N'CREATE SCHEMA dbo AUTHORIZATION dbo');
END;
GO


-- -------------------------------------------------
-- 2. Create Table-Valued Parameter Type
-- -------------------------------------------------

CREATE TYPE dbo.ExternalMetadataTableValued AS TABLE
(
    Property5 NVARCHAR(MAX) NOT NULL,
    Property4 NVARCHAR(MAX) NOT NULL,
    Property3 INT NOT NULL,
    Property2 INT NOT NULL,
    Property1 INT NULL,
    Property0 INT NULL
);
GO


-- -------------------------------------------------
-- 3. Create Target Table
-- -------------------------------------------------

CREATE TABLE dbo.table_valued_test
(
    Property5 NVARCHAR(MAX) NOT NULL,
    Property4 NVARCHAR(MAX) NOT NULL,
    Property3 INT NOT NULL,
    Property2 INT NOT NULL,
    Property1 INT NULL,
    Property0 INT NULL
);
GO


-- -------------------------------------------------
-- 4. Create Procedure Accepting TVP
-- -------------------------------------------------

CREATE PROCEDURE dbo.table_valued_insert @i_p dbo.ExternalMetadataTableValued READONLY, @o_p BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.table_valued_test
    (
        Property5,
        Property4,
        Property3,
        Property2,
        Property1,
        Property0
    )
    SELECT
        Property5,
        Property4,
        Property3,
        Property2,
        Property1,
        Property0
    FROM @i_p;

    SET @o_p = @@ROWCOUNT;
END;