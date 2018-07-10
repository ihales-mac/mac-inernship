CREATE TABLE [dbo].[Hash] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL,
    [Md5Hash]  NVARCHAR (32) NOT NULL,
    [Sha2Hash] NVARCHAR (64) NOT NULL,
    [Sha1Hash] NVARCHAR (40) NOT NULL
);

