CREATE FUNCTION [dbo].[Search]
(
	@searchWord nvarchar(50)
)
RETURNS @returntable TABLE
(
	FirstName nvarchar(50),
	LastName nvarchar(50),
	Email nvarchar(50),
	[Role] nvarchar(50),
	Phone nvarchar(50),
	DateOfBirth date,
	Gender nvarchar(50)
)
AS
BEGIN
	INSERT @returntable
	SELECT FirstName,LastName,Email,[Name],PhoneNumber,DateOfBirth,Gender From ViewAllData
		Where CONCAT(FirstName,LastName,Email,[Name],PhoneNumber,DateOfBirth,Gender)like '%'+@searchWord+'%'
	RETURN
END
