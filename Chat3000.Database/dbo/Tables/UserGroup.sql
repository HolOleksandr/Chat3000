﻿CREATE TABLE [dbo].[UserGroup]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [GroupId] INT NOT NULL, 
    [UserId] VARCHAR(MAX) NOT NULL, 
    [JoinDate] DATETIME NULL
)
