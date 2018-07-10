CREATE VIEW [dbo].[fullUserDetails]
	AS SELECT FirstName,LastName,Email,PhoneNumber,DateOfBirth FROM [User] inner join [UserInfo] on [User].MoreInfo=[UserInfo].ID 
