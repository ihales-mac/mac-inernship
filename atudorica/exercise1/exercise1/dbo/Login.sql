CREATE PROCEDURE [dbo].[Login]
	@username nVARchar(50),
	@password varchar(50),
	@response nvarchar(50) out
AS
BEGIN
	
	BEGIN TRY
		DECLARE  @FullName nvarchar(21)
		set @FullName = (SELECT dbo.PrintName(FirstName,LastName) From [User] 
										WHERE ((Username=@username or Email=@username) and [Password]=Convert(nvarchar(50),HASHBYTES ('MD2',@password))));
		if (@FullName is NULL)
			BEGIN
				if((Select dbo.PrintName(FirstName,LastName) From [User] WHERE (Username=@username or Email=@username)) is null)
					set @response = 'INVALID USER'
				else
					begin
						set @response = 'INVALID PASSWORD' 
						select Convert(nvarchar(50),HASHBYTES ('MD2',@password))
					end
			END
		ELSE
			begin
				set @response = 'WELCOME ' + @FullName 
			end
		INSERT INTO [Log] (Response,Username) VALUES (@response,@username)
	END TRY
	BEGIN CATCH
		PRINT (ERROR_NUMBER() ) 
		PRINT (ERROR_SEVERITY()) 
		PRINT (ERROR_STATE() )   
		PRINT (ERROR_PROCEDURE() ) 
		PRINT (ERROR_LINE() ) 
		PRINT (ERROR_MESSAGE() )  
	END CATCH
END
		
