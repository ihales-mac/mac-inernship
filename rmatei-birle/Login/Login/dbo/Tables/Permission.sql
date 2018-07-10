CREATE TABLE [dbo].[Permission] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (20) NOT NULL,
    [Description] TEXT          NULL,
    [IsDeleted]   BIT           NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([ID] ASC)
);

