CREATE PROCEDURE insertUser(
@username varchar(50),
@password varchar(50),
@hashAlgorithm varchar(10)
)
AS
BEGIN
	INSERT INTO Users values(@username,HASHBYTES(@hashAlgorithm, @password))
END
