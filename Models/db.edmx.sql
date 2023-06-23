
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/23/2023 21:34:39
-- Generated from EDMX file: D:\school\university\大三\asp_net\Asp_Net_FinalProject\Models\db.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [D:\school\university\大三\asp_net\Asp_Net_FinalProject\App_Data\db.mdf];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Comment__Post_id__3E52440B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK__Comment__Post_id__3E52440B];
GO
IF OBJECT_ID(N'[dbo].[FK__Comment__User_id__3D5E1FD2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK__Comment__User_id__3D5E1FD2];
GO
IF OBJECT_ID(N'[dbo].[FK__Post__User_id__3A81B327]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Post] DROP CONSTRAINT [FK__Post__User_id__3A81B327];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comment];
GO
IF OBJECT_ID(N'[dbo].[Post]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Post];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[User_Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User_Role];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comment'
CREATE TABLE [dbo].[Comment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [User_id] int  NOT NULL,
    [Post_id] int  NOT NULL,
    [Comment_date] datetime  NOT NULL
);
GO

-- Creating table 'Post'
CREATE TABLE [dbo].[Post] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [User_id] int  NOT NULL,
    [Post_date] datetime  NOT NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Registration_date] datetime  NOT NULL,
    [Role_id] int  NOT NULL
);
GO

-- Creating table 'User_Role'
CREATE TABLE [dbo].[User_Role] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Role_Name] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Comment'
ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT [PK_Comment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Post'
ALTER TABLE [dbo].[Post]
ADD CONSTRAINT [PK_Post]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User_Role'
ALTER TABLE [dbo].[User_Role]
ADD CONSTRAINT [PK_User_Role]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Post_id] in table 'Comment'
ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT [FK__Comment__Post_id__3E52440B]
    FOREIGN KEY ([Post_id])
    REFERENCES [dbo].[Post]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Comment__Post_id__3E52440B'
CREATE INDEX [IX_FK__Comment__Post_id__3E52440B]
ON [dbo].[Comment]
    ([Post_id]);
GO

-- Creating foreign key on [User_id] in table 'Comment'
ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT [FK__Comment__User_id__3D5E1FD2]
    FOREIGN KEY ([User_id])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Comment__User_id__3D5E1FD2'
CREATE INDEX [IX_FK__Comment__User_id__3D5E1FD2]
ON [dbo].[Comment]
    ([User_id]);
GO

-- Creating foreign key on [User_id] in table 'Post'
ALTER TABLE [dbo].[Post]
ADD CONSTRAINT [FK__Post__User_id__3A81B327]
    FOREIGN KEY ([User_id])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Post__User_id__3A81B327'
CREATE INDEX [IX_FK__Post__User_id__3A81B327]
ON [dbo].[Post]
    ([User_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------