CREATE VIEW [dbo].[FullUserInfo]
	AS SELECT [User].Email, [User].FirstName, [User].LastName, [User].DisplayName, PersonalDetails.Address, PersonalDetails.City, PersonalDetails.Country, PersonalDetails.PhoneNumber
	FROM [User] INNER JOIN PersonalDetails ON [User].Details = PersonalDetails.ID where [User].IsDeleted = 0 and PersonalDetails.IsDeleted = 0
