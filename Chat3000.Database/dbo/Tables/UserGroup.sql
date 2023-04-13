CREATE TABLE [dbo].[UserGroup]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [GroupId] INT NOT NULL, 
    [UserId] VARCHAR(MAX) NOT NULL
)
