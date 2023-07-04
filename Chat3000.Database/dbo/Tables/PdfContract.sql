CREATE TABLE [dbo].[PdfContract]
(
	[Id]                   UNIQUEIDENTIFIER   NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[FileName]             NVARCHAR (450)     NULL,
    [FileUrl]              NVARCHAR (256)     NULL,
    [UploaderId]           NVARCHAR (256)     NULL,
    [ReceiverId]           NVARCHAR (256)     NULL,
    [IsSigned]             BIT                NOT NULL DEFAULT 0,
    [SignFieldsNum]        INT                NULL,
    [UploadDate]           DATETIME     NULL,
    [SignDate]             DATETIME     NULL,
    [IsSyncF]              BIT                NOT NULL DEFAULT 0,
    [IsPsPdf]              BIT                NOT NULL DEFAULT 0,

)
