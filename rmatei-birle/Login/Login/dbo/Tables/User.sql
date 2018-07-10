CREATE TABLE [dbo].[User] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Email]       NVARCHAR (50) NOT NULL,
    [Password]    NCHAR (64)    NOT NULL,
    [FirstName]   NVARCHAR (40) NOT NULL,
    [LastName]    NVARCHAR (20) NOT NULL,
    [Details]     INT           NULL,
    [DisplayName] NVARCHAR (20) NOT NULL,
    [IsDeleted]   BIT           NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_User_PersonalDetails] FOREIGN KEY ([Details]) REFERENCES [dbo].[PersonalDetails] ([ID])
);

