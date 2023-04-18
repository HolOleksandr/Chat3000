IF NOT EXISTS(SELECT 1 FROM [dbo].[AspNetUserRoles] WHERE [UserId] = '57f9b20f-3d14-4f48-be5f-90084218b437')
BEGIN
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) 
       VALUES ('57f9b20f-3d14-4f48-be5f-90084218b437', N'a3f703ec-07d8-42a2-8915-5afa15b7da9e');
END
