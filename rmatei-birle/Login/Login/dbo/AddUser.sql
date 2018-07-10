CREATE PROCEDURE [dbo].[AddUser]
	@Email nvarchar(50),
	@Password nvarchar(30),
	@FirstName nvarchar(40),
	@LastName nvarchar(20),
	@DisplayName nvarchar(20)
AS
	DECLARE 
	@HashedPassword nchar(64),
	@UserID int

	SELECT @HashedPassword = HASHBYTES('SHA2_256', @Password)

	INSERT INTO [User]  (Email, Password, FirstName, LastName, DisplayName, IsDeleted) VALUES (@Email, @HashedPassword, @FirstName, @LastName , @DisplayName, 0)
	SELECT @UserID = SCOPE_IDENTITY()
	
	INSERT INTO [Log] (UserID, Message) VALUES (@UserID, 'User with Email ' + @Email + ' was created')

RETURN 0
