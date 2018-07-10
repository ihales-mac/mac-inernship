CREATE PROCEDURE [dbo].[MakeUser]
	@Email nvarchar(50)
AS
	DECLARE
	@RetrievedUserID int,
	@RetrievedPermissionID int

	SELECT @RetrievedUserID = ID FROM [User] WHERE Email = @Email
	SELECT @RetrievedPermissionID = ID FROM [Permission] WHERE [Name] = 'User'

	INSERT INTO [UserHasPermission] (UserID, PermissionID, IsDeleted) VALUES (@RetrievedUserID, @RetrievedPermissionID, 0)
	INSERT INTO [Log] (UserID, Message) VALUES (@RetrievedUserID, 'User with Email ' + @Email + ' was given User permission')
RETURN 0
