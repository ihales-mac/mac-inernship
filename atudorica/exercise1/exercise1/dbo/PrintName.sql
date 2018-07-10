CREATE FUNCTION [dbo].[PrintName]
(
	@FirstName nvarchar(10),
	@LastName nvarchar(10)
)
RETURNS nvarchar(21)
AS
BEGIN
	RETURN @FirstName + ' ' + @LastName
END
