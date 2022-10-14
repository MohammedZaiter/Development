CREATE TABLE [dbo].[Connectors]
(
	[Id] INT NOT NULL , 
    [StationId] UNIQUEIDENTIFIER NOT NULL, 
    [MaxCurrent] BIGINT NOT NULL, 
    CONSTRAINT [CK_Connectors_MaxCurrent] CHECK ([MaxCurrent] > 0), 
    CONSTRAINT [FK_Connectors_Stations] FOREIGN KEY ([StationId]) REFERENCES [Stations]([Id]), 
    PRIMARY KEY ([StationId], [Id])
)
