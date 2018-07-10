CREATE TABLE [dbo].[UserInfo] (
    [ID]          INT        IDENTITY (1, 1) NOT NULL,
    [PhoneNumber] NVARCHAR (10) NULL,
    [DateOfBirth] DATE       NULL,
    [Gender]      NVARCHAR (10) NULL,
    CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED ([ID] ASC)
);

