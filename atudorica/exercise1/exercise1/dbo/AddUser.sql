CREATE PROCEDURE [dbo].[AddUser]
	@Email nvarchar(50),
	@Username nvarchar(50),
	@Password nvarchar(50),
	@FirstName nvarchar(10),
	@LastName nvarchar(10)
AS
	INSERT INTO [User] (Email, Username, [Password], FirstName,LastName)
	VALUES (@Email,@Username,HASHBYTES ('MD2',@Password),@FirstName,@LastName);
RETURN 0
