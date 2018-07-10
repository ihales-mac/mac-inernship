CREATE VIEW [dbo].[ViewUserRole]
	AS SELECT FirstName,LastName,Email,[Name] FROM [User],[Role],[UserHasRole] where UserHasRole.UserID=[User].ID and UserHasRole.RoleID=[Role].ID

