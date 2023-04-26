CREATE TABLE [dbo].[Message]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [SenderId] NVARCHAR(MAX) NOT NULL, 
    [ReceiverId] NVARCHAR(MAX) NULL, 
    [GroupId] INT NULL, 
    [Text] TEXT NULL, 
    [SendDate] DATETIME NOT NULL
)
