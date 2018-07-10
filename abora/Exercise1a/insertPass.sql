CREATE PROCEDURE insertPass(
@password varchar(5)
)
AS
BEGIN
	INSERT INTO Rainbow values(@password,HASHBYTES('MD2', @password),HASHBYTES('MD4', @password),HASHBYTES('MD5', @password),HASHBYTES('SHA', @password),HASHBYTES('SHA1', @password))
END

exec insertPass 'a'

select * from Users