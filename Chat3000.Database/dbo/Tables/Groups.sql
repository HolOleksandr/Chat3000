CREATE TABLE [dbo].[Groups]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [AdminId] VARCHAR(MAX) NOT NULL, 
    [CreationDate] DATETIME NULL
)
