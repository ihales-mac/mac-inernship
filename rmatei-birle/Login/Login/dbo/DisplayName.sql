CREATE FUNCTION [dbo].[DisplayName]
(
	@FirstName nvarchar(40),
	@LastName nvarchar(20)
)
RETURNS INT
AS
BEGIN
	RETURN @FirstName + ' ' + @LastName
END 
