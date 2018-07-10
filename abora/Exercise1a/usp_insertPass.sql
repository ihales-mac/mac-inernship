CREATE PROCEDURE usp_insertPass
(@tvpNewRainbowTable dbo.RainbowTableType READONLY)
AS 
BEGIN
	INSERT INTO Rainbow SELECT password, HASHBYTES('MD2', password),HASHBYTES('MD4', password),HASHBYTES('MD5', password),HASHBYTES('SHA', password),HASHBYTES('SHA1', password) from @tvpNewRainbowTable
END

declare @dt RainbowTableType;
insert into @dt values('a');
select * from @dt;

select * from Rainbow
delete from Rainbow
insert into Users values('andy',HASHBYTES('MD2','abcde'));
select * from Users;