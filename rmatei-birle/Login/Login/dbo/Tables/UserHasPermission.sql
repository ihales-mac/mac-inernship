CREATE TABLE [dbo].[UserHasPermission] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserID]       INT NOT NULL,
    [PermissionID] INT NOT NULL,
    [IsDeleted]    BIT NOT NULL,
    CONSTRAINT [PK_UserHasPermission] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserHasPermission_Permission] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[Permission] ([ID]),
    CONSTRAINT [FK_UserHasPermission_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

