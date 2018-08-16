CREATE TABLE [dbo].[PatientDetails] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [address] NVARCHAR (200) NULL,
    [detail1] NVARCHAR (200) NULL,
    [detail2] NVARCHAR (200) NULL,
    [detail3] NVARCHAR (200) NULL,
    CONSTRAINT [PK_PatientDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

