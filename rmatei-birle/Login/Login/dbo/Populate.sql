GO

DELETE FROM [UserHasPermission]
GO
DELETE FROM [Log]
GO
DELETE FROM [User]
GO
DELETE FROM [PersonalDetails]
GO
DELETE FROM [Permission]
GO

EXEC AddUser @Email = 'andrei@gmail.com', @Password = 'andrei', @FirstName = 'Andrei', @LastName = 'Bora', @DisplayName = 'AB'
GO
EXEC AddUser @Email = 'imola@gmail.com', @Password = 'imola', @FirstName = 'Imola Katalin', @LastName = 'Deszi', @DisplayName = 'IKD'
GO
EXEC AddUser @Email = 'robert@gmail.com', @Password = 'robert', @FirstName = 'Robert', @LastName = 'Laszlo', @DisplayName = 'AL'
GO
EXEC AddUser @Email = 'andreit@gmail.com', @Password = 'andreit', @FirstName = 'Andrei', @LastName = 'Tudorica', @DisplayName = 'AT'
GO
EXEC AddUser @Email = 'matei@gmail.com', @Password = 'matei', @FirstName = 'Radu Matei', @LastName = 'Birle', @DisplayName = 'RMB'
GO

EXEC AddDetails @Email = 'andrei@gmail.com', @PhoneNumber = '0756985632', @Address = 'da', @City = 'Cluj', @Country = 'Romania'
GO
EXEC AddDetails @Email = 'imola@gmail.com', @PhoneNumber = '0789456258', @Address = 'da', @City = 'Cluj', @Country = 'Romania'
GO
EXEC AddDetails @Email = 'robert@gmail.com', @PhoneNumber = '0764582695', @Address = 'da', @City = 'Cluj', @Country = 'Romania'
GO
EXEC AddDetails @Email = 'andreit@gmail.com', @PhoneNumber = '0798652984', @Address = 'da', @City = 'Cluj', @Country = 'Romania'
GO
EXEC AddDetails @Email = 'matei@gmail.com', @PhoneNumber = '0744598625', @Address = 'da', @City = 'Cluj', @Country = 'Romania'
GO

INSERT INTO Permission (Name, Description, IsDeleted) VALUES('Administrator', 'Takes care of data', 0)
GO
INSERT INTO Permission (Name, Description, IsDeleted) VALUES('SysAdmin', 'Takes care of data and system', 0)
GO
INSERT INTO Permission (Name, Description, IsDeleted) VALUES('User', 'Takes care of nothing', 0)
GO
INSERT INTO Permission (Name, Description, IsDeleted) VALUES('Sealed', 'Can view private data', 0)
GO

EXEC [MakeAdmin] @Email = 'andrei@gmail.com'
GO
EXEC MakeSealed @Email = 'andrei@gmail.com'
GO
EXEC MakeSysAdmin @Email = 'imola@gmail.com'
GO
EXEC MakeUser @Email = 'robert@gmail.com'
GO
EXEC MakeAdmin @Email = 'matei@gmail.com'
GO
EXEC MakeUser @Email = 'andreit@gmail.com'
GO