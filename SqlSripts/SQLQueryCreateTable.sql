USE MyDatabase;

CREATE TABLE [dbo].[Markers] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NULL,
    [Latitude]  FLOAT (53)    NULL,
    [Longitude] FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
