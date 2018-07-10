CREATE VIEW [dbo].[ViewAllData]
	AS Select FirstName,LastName,Email,[Name],PhoneNumber,DateOfBirth,Gender 
				FROM [User],[Role],[UserHasRole],[UserInfo] 
				where UserHasRole.UserID=[User].ID and UserHasRole.RoleID=[Role].ID and [UserInfo].ID=[User].MoreInfo
