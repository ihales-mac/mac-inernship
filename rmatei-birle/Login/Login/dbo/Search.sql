CREATE FUNCTION [dbo].[Search]
(
	@SearchTerm nvarchar(40)
)
RETURNS TABLE
AS
RETURN(
	SELECT ID, FirstName, LastName, Email, DisplayName, PhoneNumber, Address, City, Country, Permission FROM UserInfoSearch WHERE SearchString LIKE CONCAT('%',@SearchTerm,'%')
)
