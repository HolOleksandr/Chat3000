:r .\ScriptUserConfig.sql


IF NOT EXISTS(SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Name] = 'User')
BEGIN
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName]) 
       VALUES ('1ce0547f-71b8-4105-826a-3478d53a927f', N'User', N'USER');
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Name] = 'Administrator')
BEGIN
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName]) 
       VALUES ('a3f703ec-07d8-42a2-8915-5afa15b7da9e', N'Administrator', N'ADMINISTRATOR');
END


:r .\ScriptUserRoleConfig.sql