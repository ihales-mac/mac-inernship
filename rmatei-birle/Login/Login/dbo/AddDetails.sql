CREATE PROCEDURE [dbo].[AddDetails]
	@Email NVARCHAR(50),
    @PhoneNumber NCHAR (14),
    @Address NVARCHAR (50),
    @City NVARCHAR (20),
    @Country NVARCHAR (20)
AS
	DECLARE 
	@DetailsID int,
	@UserID int

	INSERT INTO PersonalDetails (PhoneNumber, Address, City, Country, IsDeleted) VALUES (@PhoneNumber, @Address, @City, @Country, 0)
	SELECT @DetailsID = SCOPE_IDENTITY()
	
	UPDATE [User] SET Details = @DetailsID where Email = @Email
	SELECT @UserID = ID FROM [User] WHERE Email = @Email
	
	INSERT INTO [Log] (UserID, Message) VALUES (@UserID, CONCAT('User ' + @Email + ' had details added at ID ' ,@DetailsID))
RETURN 0
