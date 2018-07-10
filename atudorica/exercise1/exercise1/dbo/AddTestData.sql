CREATE PROCEDURE [dbo].[AddTestData]
AS
	INSERT INTO [UserInfo] (PhoneNumber,DateOfBirth,Gender) VALUES ('000000000','1996-12-12','Male');
	INSERT INTO [UserInfo] (PhoneNumber,DateOfBirth,Gender) VALUES ('111111111','1991-11-11','Male');
	INSERT INTO [UserInfo] (PhoneNumber,DateOfBirth,Gender) VALUES ('222222222','1997-03-19','Male');
	INSERT INTO [UserInfo] (PhoneNumber,DateOfBirth,Gender) VALUES ('333333333','1900-10-10','Female');
	INSERT INTO [UserInfo] (PhoneNumber,DateOfBirth,Gender) VALUES ('444444444','1997-12-12','Female');
	INSERT INTO [UserInfo] (PhoneNumber,DateOfBirth,Gender) VALUES ('0749810437','1997-03-23','Male');

	INSERT INTO [User] (Email, Username, [Password], FirstName, LastName, MoreInfo) VALUES ('andreitudoricacj@gmail.com','andreitudorica',Convert(nvarchar(50),HASHBYTES ('MD2','andrei')),'andrei','tudorica',11);
	INSERT INTO [User] (Email, Username, [Password], FirstName, LastName, MoreInfo) VALUES ('matei@matei.com','mateimatei',Convert(nvarchar(50),HASHBYTES ('MD2','matei')),'matei','birle',1);
	INSERT INTO [User] (Email, Username, [Password], FirstName, LastName, MoreInfo) VALUES ('andrei@andrei.com','andreiandrei',Convert(nvarchar(50),HASHBYTES ('MD2','andrei')),'andrei','bora',2);
	INSERT INTO [User] (Email, Username, [Password], FirstName, LastName, MoreInfo) VALUES ('robi@robi.com','robirobi',Convert(nvarchar(50),HASHBYTES ('MD2','robi')),'robi','laszlo',3);
	INSERT INTO [User] (Email, Username, [Password], FirstName, LastName, MoreInfo) VALUES ('ana@ana.com','anaana',Convert(nvarchar(50),HASHBYTES ('MD2','ana')),'ana','a lu ion',4);
	INSERT INTO [User] (Email, Username, [Password], FirstName, LastName, MoreInfo) VALUES ('imola@imola.com','imolaimola',Convert(nvarchar(50),HASHBYTES ('MD2','imola')),'imola','imola',5);

	INSERT INTO [ROLE] ([Name],Permission1,Permission2,Permission3) VALUES ('Admin',1,1,1);
	INSERT INTO [ROLE] ([Name],Permission1,Permission2,Permission3) VALUES ('Student',1,1,0);
	INSERT INTO [ROLE] ([Name],Permission1,Permission2,Permission3) VALUES ('Guest',1,0,0);
	INSERT INTO [ROLE] ([Name],Permission1,Permission2,Permission3) VALUES ('Freyer',0,0,0);

	Insert INTO [UserHasRole] (UserID,RoleID) Values ((select ID From [User] where Email='matei@matei.com'),4);
	Insert INTO [UserHasRole] (UserID,RoleID) Values ((select ID From [User] where Email='andrei@andrei.com'),3);
	Insert INTO [UserHasRole] (UserID,RoleID) Values ((select ID From [User] where Email='robi@robi.com'),2);
	Insert INTO [UserHasRole] (UserID,RoleID) Values ((select ID From [User] where Email='ana@ana.com'),1);
	Insert INTO [UserHasRole] (UserID,RoleID) Values ((select ID From [User] where Email='andreitudoricacj@gmail.com'),1);


RETURN 0
