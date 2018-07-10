CREATE PROCEDURE getPassFromHash(
@username varchar(200)
)
AS
BEGIN
	DECLARE @hashpass varchar(200);
	DECLARE @searchPass varchar(5);
	SELECT @hashpass  = password from Users where username = @username;
	SELECT @searchPass = password from Rainbow where md2hash = @hashpass or md4hash = @hashpass or md5hash = @hashpass or shahash = @hashpass or sha1hash = @hashpass
END

select * from Rainbow
select * from Users

exec insertUser 'a','ab','MD2'

