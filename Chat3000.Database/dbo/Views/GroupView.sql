CREATE VIEW [dbo].[GroupView] AS 
	SELECT 
		g.Id,
		g.Name,
		g.CreationDate, 
		g.Description,
		u.Email AS AdminEmail,
		g.AdminId, 
		COUNT(DISTINCT m.Id) AS TotalMessages, 
		COUNT(DISTINCT ug.UserId) AS Members

	FROM 
    [dbo].[Groups] g
    LEFT JOIN [dbo].[UserGroup] AS ug ON g.Id = ug.GroupId
    JOIN [dbo].[AspNetUsers] AS u ON g.AdminId = u.Id
    LEFT JOIN [dbo].[Messages] m ON g.Id = m.GroupId
	GROUP BY 
	g.Id, g.Name, u.Email, g.CreationDate, g.Description, g.AdminId


