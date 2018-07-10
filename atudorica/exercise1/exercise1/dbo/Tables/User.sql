CREATE TABLE [dbo].[User] (
    [ID]        INT        IDENTITY (1, 1) NOT NULL,
    [Username]  NVARCHAR (50) NOT NULL,
    [Email]     NVARCHAR (50) NOT NULL,
    [FirstName] NVARCHAR (10) NOT NULL,
    [LastName]  NVARCHAR (10) NOT NULL,
    [MoreInfo]  INT        NULL,
    [Password]  NVARCHAR (50)  NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_User_UserInfo] FOREIGN KEY ([MoreInfo]) REFERENCES [dbo].[UserInfo] ([ID])
);



