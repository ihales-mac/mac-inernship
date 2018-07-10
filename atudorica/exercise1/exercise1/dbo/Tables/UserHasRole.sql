CREATE TABLE [dbo].[UserHasRole] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [UserID] INT NOT NULL,
    [RoleID] INT NOT NULL,
    CONSTRAINT [PK_UserHasRole] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserHasRole_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID]),
    CONSTRAINT [FK_UserHasRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

