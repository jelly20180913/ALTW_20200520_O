
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/07/2019 16:51:23
-- Generated from EDMX file: C:\Users\L180067\Documents\Visual Studio 2015\Projects\Git20190315\WebApi\WebApi\Models\EDI.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EDI];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Login]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Login];
GO
IF OBJECT_ID(N'[dbo].[Pos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pos];
GO
IF OBJECT_ID(N'[dbo].[PosColumnMap]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PosColumnMap];
GO
IF OBJECT_ID(N'[dbo].[PosData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PosData];
GO
IF OBJECT_ID(N'[dbo].[PosOrderMapping]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PosOrderMapping];
GO
IF OBJECT_ID(N'[dbo].[ReportFile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReportFile];
GO
IF OBJECT_ID(N'[dbo].[UploadLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UploadLog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UploadLog'
CREATE TABLE [dbo].[UploadLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FK_LoginId] int  NOT NULL,
    [FileName] nvarchar(50)  NULL,
    [UpdateTime] varchar(20)  NULL,
    [TableName] varchar(20)  NULL,
    [Success] bit  NULL,
    [ServerFileName] nvarchar(250)  NULL
);
GO

-- Creating table 'Login'
CREATE TABLE [dbo].[Login] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Account] varchar(20)  NULL,
    [Password] varchar(20)  NULL,
    [Country] nvarchar(20)  NULL,
    [CustomerName] nvarchar(50)  NULL,
    [IndexPage] nvarchar(50)  NULL
);
GO

-- Creating table 'PosColumnMap'
CREATE TABLE [dbo].[PosColumnMap] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Year] varchar(50)  NULL,
    [MonthYear] varchar(50)  NULL,
    [Distributor] varchar(50)  NULL,
    [Customer] varchar(50)  NULL,
    [ISOCountryCode] varchar(50)  NULL,
    [Country] varchar(50)  NULL,
    [SalesArea] varchar(50)  NULL,
    [SalesManager] varchar(50)  NULL,
    [City] varchar(50)  NULL,
    [PostCode] varchar(50)  NULL,
    [PartNo] varchar(50)  NULL,
    [BaseCurrency] varchar(50)  NULL,
    [Qty] varchar(50)  NULL,
    [TotalSalesBaseCurrency] varchar(50)  NULL,
    [TotalSalesEUR] varchar(50)  NULL,
    [ProductSeries] varchar(50)  NULL,
    [Model] varchar(50)  NOT NULL
);
GO

-- Creating table 'PosData'
CREATE TABLE [dbo].[PosData] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Year] char(4)  NULL,
    [MonthYear] varchar(15)  NULL,
    [Distributor] varchar(50)  NULL,
    [Customer] nvarchar(255)  NULL,
    [ISOCountryCode] varchar(10)  NULL,
    [Country] varchar(50)  NULL,
    [SalesArea] varchar(50)  NULL,
    [SalesManager] varchar(50)  NULL,
    [City] varchar(50)  NULL,
    [PostCode] varchar(50)  NULL,
    [PartNo] varchar(50)  NULL,
    [BaseCurrency] varchar(10)  NULL,
    [Qty] int  NULL,
    [TotalSalesBaseCurreny] decimal(36,18)  NULL,
    [TotalSalesEUR] decimal(36,18)  NULL,
    [ProductSeries] varchar(20)  NULL,
    [FK_LoginId] int  NOT NULL,
    [UpdateTime] datetime  NULL
);
GO

-- Creating table 'Pos'
CREATE TABLE [dbo].[Pos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] varchar(20)  NULL,
    [Distributor] varchar(20)  NULL,
    [Branch] varchar(50)  NULL,
    [Address] varchar(50)  NULL,
    [City] varchar(50)  NULL,
    [State] varchar(20)  NULL,
    [ZIP] varchar(20)  NULL,
    [Series] varchar(50)  NULL,
    [PartNo] varchar(50)  NULL,
    [Quantity] int  NULL,
    [InvoiceNo] varchar(50)  NULL,
    [InvoiceDate] varchar(20)  NULL,
    [Cost] decimal(18,2)  NULL,
    [Price] decimal(18,2)  NULL,
    [ResellingExt] decimal(18,2)  NULL,
    [ACCT] varchar(50)  NULL,
    [CustomerName] varchar(255)  NULL,
    [MarketCode] varchar(20)  NULL,
    [Market] varchar(50)  NULL,
    [SubSegmentCode] varchar(20)  NULL,
    [SubSegment] varchar(50)  NULL,
    [Remarks] nvarchar(255)  NULL,
    [CustomerPO] varchar(50)  NULL,
    [ShipDate] varchar(20)  NULL,
    [ShipDate2] datetime  NULL,
    [ShipMonth] varchar(20)  NULL,
    [ShipQuarter] varchar(20)  NULL,
    [CountryCode] varchar(20)  NULL,
    [Country] varchar(50)  NULL,
    [Region] varchar(20)  NULL,
    [FK_LoginId] int  NULL,
    [UpdateTime] datetime  NULL,
    [Status] varchar(20)  NULL
);
GO

-- Creating table 'PosOrderMapping'
CREATE TABLE [dbo].[PosOrderMapping] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] int  NULL,
    [Distributor] int  NULL,
    [Branch] int  NULL,
    [Address] int  NULL,
    [City] int  NULL,
    [State] int  NULL,
    [ZIP] int  NULL,
    [Series] int  NULL,
    [PartNo] int  NULL,
    [Quantity] int  NULL,
    [InvoiceNo] int  NULL,
    [InvoiceDate] int  NULL,
    [Cost] int  NULL,
    [Price] int  NULL,
    [ResellingExt] int  NULL,
    [ACCT] int  NULL,
    [CustomerName] int  NULL,
    [MarketCode] int  NULL,
    [Market] int  NULL,
    [SubSegmentCode] int  NULL,
    [SubSegment] int  NULL,
    [Remarks] int  NULL,
    [CustomerPO] int  NULL,
    [ShipDate] int  NULL,
    [ShipDate2] int  NULL,
    [ShipMonth] int  NULL,
    [ShipQuarter] int  NULL,
    [CountryCode] int  NULL,
    [Country] int  NULL,
    [Region] int  NULL,
    [Model] int  NULL,
    [Start] int  NOT NULL,
    [SplitChar] varchar(20)  NULL
);
GO

-- Creating table 'ReportFile'
CREATE TABLE [dbo].[ReportFile] (
    [FK_LoginId] int  NULL,
    [FileName] nvarchar(250)  NULL,
    [Id] int  NOT NULL,
    [UpdateTime] varchar(20)  NULL,
    [Success] bit  NULL,
    [ServerFileName] nvarchar(250)  NULL,
    [Date] varchar(20)  NULL,
    [CanDownload] bit  NULL,
    [IsDel] bit  NULL,
    [ServerSimpleFileName] nvarchar(250)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UploadLog'
ALTER TABLE [dbo].[UploadLog]
ADD CONSTRAINT [PK_UploadLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Login'
ALTER TABLE [dbo].[Login]
ADD CONSTRAINT [PK_Login]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PosColumnMap'
ALTER TABLE [dbo].[PosColumnMap]
ADD CONSTRAINT [PK_PosColumnMap]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PosData'
ALTER TABLE [dbo].[PosData]
ADD CONSTRAINT [PK_PosData]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pos'
ALTER TABLE [dbo].[Pos]
ADD CONSTRAINT [PK_Pos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PosOrderMapping'
ALTER TABLE [dbo].[PosOrderMapping]
ADD CONSTRAINT [PK_PosOrderMapping]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReportFile'
ALTER TABLE [dbo].[ReportFile]
ADD CONSTRAINT [PK_ReportFile]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------