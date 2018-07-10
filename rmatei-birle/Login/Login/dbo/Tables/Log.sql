CREATE TABLE [dbo].[Log] (
    [ID]      INT  IDENTITY (1, 1) NOT NULL,
    [UserID]  INT  NULL,
    [Message] TEXT NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Log_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

