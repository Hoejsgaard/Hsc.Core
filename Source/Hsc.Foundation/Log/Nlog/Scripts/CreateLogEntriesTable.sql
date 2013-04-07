USE [Hsc_DB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LogEntries](
	[DateTime] [datetime2](7) NOT NULL,
	[Server] [nchar](100) NULL,
	[Application] [nchar](100) NULL,
	[LogLevel] [varchar](100) NULL,
	[Callsite] [varchar](200) NULL,
	[Message] [varchar](max) NULL,
	[EventId] [int] NULL,
	[Exception] [varchar](max) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

