CREATE PROCEDURE [dbo].[Login]
	@Email nvarchar(50),
	@Password nvarchar(30),
	@Response nvarchar(20) OUT
AS
	DECLARE
	@RetrievedDisplayName nvarchar(20),
	@RetrievedPass nvarchar(64),
	@RetrievedID int = -1,
	@HashedPass nchar(64)

	SELECT @RetrievedPass = Password, @RetrievedID = ID, @RetrievedDisplayName = DisplayName FROM [User]
	WHERE Email = @Email

	IF (@RetrievedID = -1) 
	BEGIN
		INSERT INTO [Log] (UserID, Message) VALUES (null, 'Login unsuccessful. Email ' + @Email + ' not found')
		SET @Response = 'ERROR'
		RETURN -1
	END
	
	SELECT @HashedPass = HASHBYTES('SHA2_256', @Password)

	IF(@HashedPass = @RetrievedPass)
	BEGIN
		INSERT INTO [Log] (UserID, Message) VALUES (@RetrievedID, 'Login successful. Email ' + @Email + ' and password were correct')
		SET @Response = @RetrievedDisplayName
		RETURN 1
	END

	ELSE 
	BEGIN
		INSERT INTO [Log] (UserID, Message) VALUES (@RetrievedID, 'Login unsuccessful. ' + @Email + ', password was incorrect')
		SET @Response = 'ERROR'
		RETURN 0
	END