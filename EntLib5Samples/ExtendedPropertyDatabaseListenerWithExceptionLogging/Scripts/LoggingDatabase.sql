/****** Object:  Database Logging    Script Date: 8/22/2005 ******/
USE [master]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Logging')
	DROP DATABASE [Logging]
GO

CREATE DATABASE [Logging]
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

exec sp_dboption N'Logging', N'autoclose', N'false'
GO

exec sp_dboption N'Logging', N'bulkcopy', N'false'
GO

exec sp_dboption N'Logging', N'trunc. log', N'false'
GO

exec sp_dboption N'Logging', N'torn page detection', N'true'
GO

exec sp_dboption N'Logging', N'read only', N'false'
GO

exec sp_dboption N'Logging', N'dbo use', N'false'
GO

exec sp_dboption N'Logging', N'single', N'false'
GO

exec sp_dboption N'Logging', N'autoshrink', N'false'
GO

exec sp_dboption N'Logging', N'ANSI null default', N'false'
GO

exec sp_dboption N'Logging', N'recursive triggers', N'false'
GO

exec sp_dboption N'Logging', N'ANSI nulls', N'false'
GO

exec sp_dboption N'Logging', N'concat null yields null', N'false'
GO

exec sp_dboption N'Logging', N'cursor close on commit', N'false'
GO

exec sp_dboption N'Logging', N'default to local cursor', N'false'
GO

exec sp_dboption N'Logging', N'quoted identifier', N'false'
GO

exec sp_dboption N'Logging', N'ANSI warnings', N'false'
GO

exec sp_dboption N'Logging', N'auto create statistics', N'true'
GO

exec sp_dboption N'Logging', N'auto update statistics', N'true'
GO

use [Logging]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Category]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[CategoryLog]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[CategoryLog](
	[CategoryLogID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[LogID] [int] NOT NULL,
 CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED 
(
	[CategoryLogID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Log]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Log](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NULL,
	[Priority] [int] NOT NULL,
	[Severity] [nvarchar](32) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[MachineName] [nvarchar](32) NOT NULL,
	[AppDomainName] [nvarchar](512) NOT NULL,
	[ProcessID] [nvarchar](256) NOT NULL,
	[ProcessName] [nvarchar](512) NOT NULL,
	[ThreadName] [nvarchar](512) NULL,
	[Win32ThreadId] [nvarchar](128) NULL,
	[Message] [nvarchar](1500) NULL,
	[FormattedMessage] [ntext] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[ExtendedProperties]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[ExtendedProperties](
	[ExtendedPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[LogID] [int] NOT NULL,
	[Key] nvarchar(max) NOT NULL,
	[Value] nvarchar(max) NULL
 CONSTRAINT [PK_ExtendedProperties] PRIMARY KEY CLUSTERED 
(
		[ExtendedPropertyID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[InsertCategoryLog]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE InsertCategoryLog
	@CategoryID INT,
	@LogID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CatLogID INT
	SELECT @CatLogID FROM CategoryLog WHERE CategoryID=@CategoryID and LogID = @LogID
	IF @CatLogID IS NULL
	BEGIN
		INSERT INTO CategoryLog (CategoryID, LogID) VALUES(@CategoryID, @LogID)
		RETURN @@IDENTITY
	END
	ELSE RETURN @CatLogID
END
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[AddCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[AddCategory]
	-- Add the parameters for the function here
	@CategoryName nvarchar(64),
	@LogID int
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM Category WHERE CategoryName = @CategoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO Category (CategoryName) VALUES(@CategoryName)
		SELECT @CatID = @@IDENTITY
	END

	EXEC InsertCategoryLog @CatID, @LogID 

	RETURN @CatID
END

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[ClearLogs]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE ClearLogs
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM ExtendedProperties
	DELETE FROM CategoryLog
	DELETE FROM [Log]
    DELETE FROM Category
END
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[WriteLog]') AND type in (N'P', N'PC'))
--BEGIN
CREATE PROCEDURE [dbo].[WriteLog]
(
	@EventID int, 
	@Priority int, 
	@Severity nvarchar(32), 
	@Title nvarchar(256), 
	@Timestamp datetime,
	@MachineName nvarchar(32), 
	@AppDomainName nvarchar(512),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(512),
	@ThreadName nvarchar(512),
	@Win32ThreadId nvarchar(128),
	@Message nvarchar(1500),
	@FormattedMessage ntext,
	@extendedProperties nvarchar(max),	
	@LogId int OUTPUT
)
AS 
BEGIN
	INSERT INTO [Log] (
		EventID,
		[Priority],
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		[Message],
		FormattedMessage
	)
	VALUES (
		@EventID, 
		@Priority, 
		@Severity, 
		@Title, 
		@Timestamp,
		@MachineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@Message,
		@FormattedMessage)

	SET @LogID = @@IDENTITY

	if @extendedProperties is not null and len(@extendedProperties) > 0 
	BEGIN
		declare @x XML
		set @x = convert(XML, @extendedProperties)
		INSERT INTO ExtendedProperties([Key], [Value], [LogID])
		SELECT 
			replace(replace(replace(replace(replace(cast (T.c.query('./Key/text()')   as nvarchar(max)),'&quot;' , '"'), '&lt;', '<'), '&gt;', '>'), '&amp;', '&'), '&apos;', '''') as [Key],
			replace(replace(replace(replace(replace(cast (T.c.query('./Value/text()') as nvarchar(max)),'&quot;' , '"'), '&lt;', '<'), '&gt;', '>'), '&amp;', '&'), '&apos;', '''') as [Value],
			@LogId
		FROM   
			@x.nodes('/ExtendedProperties/ExtendedProperty') T(c) 
	END
	
	RETURN @LogID

END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'FK_CategoryLog_Category') AND parent_obj = OBJECT_ID(N'[dbo].[CategoryLog]'))
ALTER TABLE [dbo].[CategoryLog]  WITH CHECK ADD  CONSTRAINT [FK_CategoryLog_Category] FOREIGN KEY(	[CategoryID])
REFERENCES [dbo].[Category] (	[CategoryID])
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'FK_CategoryLog_Log') AND parent_obj = OBJECT_ID(N'[dbo].[CategoryLog]'))
ALTER TABLE [dbo].[CategoryLog]  WITH CHECK ADD  CONSTRAINT [FK_CategoryLog_Log] FOREIGN KEY(	[LogID])
REFERENCES [dbo].[Log] (	[LogID])
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'FK_ExtendedProperties_Log') AND parent_obj = OBJECT_ID(N'[dbo].[ExtendedProperties]'))
ALTER TABLE [dbo].[ExtendedProperties]  WITH CHECK ADD  CONSTRAINT [FK_ExtendedProperties_Log] FOREIGN KEY(	[LogID])
REFERENCES [dbo].[Log] (	[LogID])
GO

SET QUOTED_IDENTIFIER ON 
SET ARITHABORT ON 
SET CONCAT_NULL_YIELDS_NULL ON 
SET ANSI_NULLS ON 
SET ANSI_PADDING ON 
SET ANSI_WARNINGS ON 
SET NUMERIC_ROUNDABORT OFF 
go

DECLARE @bErrors as bit

BEGIN TRANSACTION
SET @bErrors = 0

CREATE NONCLUSTERED INDEX [ixCategoryLog] ON [dbo].[CategoryLog] ([LogID] ASC, [CategoryID] ASC )
IF( @@error <> 0 ) SET @bErrors = 1

IF( @bErrors = 0 )
  COMMIT TRANSACTION
ELSE
  ROLLBACK TRANSACTION
