CREATE TABLE [dbo].[Role] (
    [ID]          INT        IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (10) NOT NULL,
    [Permission1] BIT        NOT NULL,
    [Permission2] BIT        NOT NULL,
    [Permission3] BIT        NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([ID] ASC)
);

