CREATE PROCEDURE [dbo].[usp_RemoveConnector]
     @id INT,
	 @stationId UNIQUEIDENTIFIER
AS
    DELETE FROM 
        [dbo].[Connectors]
    WHERE
        [StationId] = @stationId AND [Id] = @id