CREATE VIEW [dbo].[UserInfoSearch]
	AS SELECT [User].ID, [User].FirstName, [User].LastName, [User].Email, [User].DisplayName, [PersonalDetails].PhoneNumber, [PersonalDetails].Address, [PersonalDetails].City, [PersonalDetails].Country, [Permission].Name AS Permission, CONCAT ([User].ID, ' ', [User].FirstName, ' ', [User].LastName, ' ', [User].Email, ' ', [User].DisplayName, ' ', [PersonalDetails].PhoneNumber, ' ', [PersonalDetails].Address, ' ', [PersonalDetails].City, ' ', [PersonalDetails].Country, ' ', [Permission].Name) AS SearchString
	FROM 

	[User] 

	INNER JOIN [PersonalDetails] ON [User].Details = [PersonalDetails].ID
	
	INNER JOIN [UserHasPermission] ON [User].ID = [UserHasPermission].UserID 

	INNER JOIN [Permission] ON [UserHasPermission].PermissionID = [Permission].ID 
	
	WHERE [User].IsDeleted = 0
