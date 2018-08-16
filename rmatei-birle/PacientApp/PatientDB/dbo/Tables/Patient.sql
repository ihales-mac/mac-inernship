CREATE TABLE [dbo].[Patient] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [firstname] NVARCHAR (50) NOT NULL,
    [lastname]  NVARCHAR (50) NOT NULL,
    [details]   INT           NULL,
    CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Patient_PatientDetails] FOREIGN KEY ([details]) REFERENCES [dbo].[PatientDetails] ([ID])
);

