/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


--IF (EXISTS(SELECT * FROM [dbo].[AspNetRoles]))  
--BEGIN  
--    SET IDENTITY_INSERT [dbo].[AspNetRoles] ON
--    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName]) 
--    VALUES ('1ce0547f-71b8-4105-826a-3478d53a927f', N'User', N'USER')
--    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName]) 
--    VALUES ('78dcbd25-7f20-4fcd-954a-50e0ecc8a1d2', N'Administrator', N'ADMINISTRATOR')
--    SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF  
--    --INSERT dbo.AspNetRoles SELECT '1ce0547f-71b8-4105-826a-3478d53a927f', 'User', 'USER'
--    --WHERE NOT EXISTS (SELECT 1 FROM dbo.AspNetRoles WHERE Name = 'User')
--END