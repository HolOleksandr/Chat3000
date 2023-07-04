DECLARE @UserId UNIQUEIDENTIFIER = '57f9b20f-3d14-4f48-be5f-90084218b437'

IF EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [Id] = @UserId)
BEGIN
    -- User already exists, perform an update
    UPDATE [dbo].[AspNetUsers]
    SET
        [UserName] = N'admin@example.com',
        [NormalizedUserName] = N'ADMIN@EXAMPLE.COM',
        [Email] = N'admin@example.com',
        [NormalizedEmail] = N'ADMIN@EXAMPLE.COM',
        [EmailConfirmed] = N'false',
        [PhoneNumberConfirmed] = N'false',
        [TwoFactorEnabled] = N'false',
        [LockoutEnabled] = N'false',
        [PasswordHash] = N'AQAAAAEAACcQAAAAEP66WnkvznniTm9z6U8yfcl/FR5psDTSR9qY6AtqFLOibeNEz0t2Iz6pAEbgTZR7Ew==',
        [SecurityStamp] = N'AINZRODVLWZRVOXULKAPL5DNTS3ZI6T3',
        [ConcurrencyStamp] = N'e7aa34fd-6916-4f70-b211-1ca027a4e5a4',
        [AccessFailedCount] = N'0',
        [FirstName] = N'Oleksandr',
        [LastName] = N'Holovachuk',
        [Nickname] = N'O_Holovachuk'
    WHERE [Id] = @UserId
END
ELSE
BEGIN
    -- User does not exist, perform an insert
    INSERT INTO [dbo].[AspNetUsers] (
        [Id],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnabled],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [AccessFailedCount],
        [FirstName],
        [LastName],
        [Nickname]
    )
    VALUES (
        @UserId,
        N'admin@example.com',
        N'ADMIN@EXAMPLE.COM',
        N'admin@example.com',
        N'ADMIN@EXAMPLE.COM',
        N'false',
        N'false',
        N'false',
        N'false',
        N'AQAAAAEAACcQAAAAEP66WnkvznniTm9z6U8yfcl/FR5psDTSR9qY6AtqFLOibeNEz0t2Iz6pAEbgTZR7Ew==',
        N'AINZRODVLWZRVOXULKAPL5DNTS3ZI6T3',
        N'e7aa34fd-6916-4f70-b211-1ca027a4e5a4',
        N'0',
        N'Oleksandr',
        N'Holovachuk',
        N'O_Holovachuk'
    )
END