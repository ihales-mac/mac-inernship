CREATE PROCEDURE [dbo].[GetPasswordFromHash]
	@HashValue nvarchar(64)
AS
	SELECT [Password] from dbo.Hash where Sha1Hash=@HashValue or Sha2Hash=@HashValue or Md5Hash=@HashValue
RETURN 0

