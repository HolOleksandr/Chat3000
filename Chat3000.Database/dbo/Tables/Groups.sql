CREATE TABLE [dbo].[Groups]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [AdminId] VARCHAR(MAX) NOT NULL, 
    [CreationDate] DATETIME NULL
)
