CREATE PROCEDURE [dbo].[MakeSealed]
	@Email nvarchar(50)
AS
	DECLARE
	@RetrievedUserID int,
	@RetrievedPermissionID int

	SELECT @RetrievedUserID = ID FROM [User] WHERE Email = @Email
	SELECT @RetrievedPermissionID = ID FROM [Permission] WHERE [Name] = 'Sealed'

	INSERT INTO [UserHasPermission] (UserID, PermissionID, IsDeleted) VALUES (@RetrievedUserID, @RetrievedPermissionID, 0)
	INSERT INTO [Log] (UserID, Message) VALUES (@RetrievedUserID, 'User with Email ' + @Email + ' was given Sealed permission')
RETURN 0
