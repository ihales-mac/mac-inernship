CREATE PROCEDURE [dbo].[MakeSysAdmin]
	@Email nvarchar(50)
AS
	DECLARE
	@RetrievedUserID int,
	@RetrievedPermissionID int

	SELECT @RetrievedUserID = ID FROM [User] WHERE Email = @Email
	SELECT @RetrievedPermissionID = ID FROM [Permission] WHERE [Name] = 'SysAdmin'

	INSERT INTO [UserHasPermission] (UserID, PermissionID, IsDeleted) VALUES (@RetrievedUserID, @RetrievedPermissionID, 0)
	INSERT INTO [Log] (UserID, Message) VALUES (@RetrievedUserID, 'User with Email ' + @Email + ' was given SysAdmin permission')
RETURN 0
