﻿CREATE TABLE [dbo].[Message]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SenderId] NVARCHAR(MAX) NOT NULL, 
    [ReceiverId] NVARCHAR(MAX) NULL, 
    [GroupId] INT NULL, 
    [Text] TEXT NULL, 
    [SendDate] DATETIME NOT NULL
)