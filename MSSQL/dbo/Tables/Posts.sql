﻿CREATE TABLE [dbo].[Posts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[UserId] INT NOT NULL, 
	[Title] NVARCHAR(255) NULL, 
	[Body] NVARCHAR(4000) NULL
)