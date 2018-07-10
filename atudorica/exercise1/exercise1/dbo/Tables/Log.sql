CREATE TABLE [dbo].[Log] (
    [ID]       INT        IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50) NULL,
    [Response] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([ID] ASC)
);

