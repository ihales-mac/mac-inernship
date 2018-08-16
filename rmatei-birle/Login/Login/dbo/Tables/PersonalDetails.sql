CREATE TABLE [dbo].[PersonalDetails] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [PhoneNumber] NCHAR (14)    NULL,
    [Address]     NVARCHAR (50) NULL,
    [City]        NVARCHAR (20) NULL,
    [Country]     NVARCHAR (20) NULL,
    [IsDeleted]   BIT           NOT NULL,
    CONSTRAINT [PK_PersonalDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

