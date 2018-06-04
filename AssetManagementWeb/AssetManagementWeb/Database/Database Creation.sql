CREATE TABLE [dbo].[AssetLocation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[Code] NVARCHAR (100),
	[Name] NVARCHAR (200),
	[Address] NVARCHAR (500),

)

CREATE TABLE [dbo].[Assets]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[Code] NVARCHAR (100),
	[Type] NVARCHAR (200),
	[Model] NVARCHAR (500),

)
CREATE TABLE [dbo].[AssetLocations] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [LocationId] INT      NULL,
    [AssetId]    INT      NULL,
    [LastSeen]   DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_AssetLocations_Locations] FOREIGN KEY ([LocationId]) REFERENCES [AssetLocation]([Id]),
	CONSTRAINT [FK_AssetLocations_Assets] FOREIGN KEY ([AssetId]) REFERENCES [Assets]([Id])
);
