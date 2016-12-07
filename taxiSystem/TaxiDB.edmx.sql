
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/06/2016 20:14:04
-- Generated from EDMX file: C:\Computer\studijos\2k\TOP\trecioji\taxiSystem\taxiSystem\TaxiDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TaxiDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CarNumber]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Trip] DROP CONSTRAINT [FK_CarNumber];
GO
IF OBJECT_ID(N'[dbo].[FK_CarOwner]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Car] DROP CONSTRAINT [FK_CarOwner];
GO
IF OBJECT_ID(N'[dbo].[FK_PassengerNumber]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Trip] DROP CONSTRAINT [FK_PassengerNumber];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Car]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Car];
GO
IF OBJECT_ID(N'[dbo].[Driver]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Driver];
GO
IF OBJECT_ID(N'[dbo].[Passenger]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Passenger];
GO
IF OBJECT_ID(N'[dbo].[Trip]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Trip];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cars'
CREATE TABLE [dbo].[Cars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Year] int  NULL,
    [Model] nvarchar(50)  NULL,
    [Size] int  NULL,
    [DriverNr] int  NOT NULL
);
GO

-- Creating table 'Drivers'
CREATE TABLE [dbo].[Drivers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Availability] bit  NOT NULL,
    [Price] float  NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Lastname] nvarchar(50)  NOT NULL,
    [CoordX] int  NOT NULL,
    [CordY] int  NOT NULL,
    [ActiveCar] int  NULL,
    [Username] nvarchar(50)  NULL,
    [Password] nvarchar(50)  NULL
);
GO

-- Creating table 'Passengers'
CREATE TABLE [dbo].[Passengers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NULL,
    [Password] nvarchar(50)  NULL
);
GO

-- Creating table 'Trips'
CREATE TABLE [dbo].[Trips] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Range] float  NULL,
    [Price] float  NULL,
    [StartX] int  NULL,
    [StartY] int  NULL,
    [EndX] int  NULL,
    [EndY] int  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [CarNr] int  NOT NULL,
    [PassengerNr] int  NOT NULL,
    [Status] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Cars'
ALTER TABLE [dbo].[Cars]
ADD CONSTRAINT [PK_Cars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Drivers'
ALTER TABLE [dbo].[Drivers]
ADD CONSTRAINT [PK_Drivers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Passengers'
ALTER TABLE [dbo].[Passengers]
ADD CONSTRAINT [PK_Passengers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Trips'
ALTER TABLE [dbo].[Trips]
ADD CONSTRAINT [PK_Trips]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CarNr] in table 'Trips'
ALTER TABLE [dbo].[Trips]
ADD CONSTRAINT [FK_CarNumber]
    FOREIGN KEY ([CarNr])
    REFERENCES [dbo].[Cars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarNumber'
CREATE INDEX [IX_FK_CarNumber]
ON [dbo].[Trips]
    ([CarNr]);
GO

-- Creating foreign key on [DriverNr] in table 'Cars'
ALTER TABLE [dbo].[Cars]
ADD CONSTRAINT [FK_CarOwner]
    FOREIGN KEY ([DriverNr])
    REFERENCES [dbo].[Drivers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarOwner'
CREATE INDEX [IX_FK_CarOwner]
ON [dbo].[Cars]
    ([DriverNr]);
GO

-- Creating foreign key on [PassengerNr] in table 'Trips'
ALTER TABLE [dbo].[Trips]
ADD CONSTRAINT [FK_PassengerNumber]
    FOREIGN KEY ([PassengerNr])
    REFERENCES [dbo].[Passengers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PassengerNumber'
CREATE INDEX [IX_FK_PassengerNumber]
ON [dbo].[Trips]
    ([PassengerNr]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------