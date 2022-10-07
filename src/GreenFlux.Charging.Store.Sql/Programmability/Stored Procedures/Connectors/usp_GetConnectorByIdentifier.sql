CREATE PROCEDURE [dbo].[usp_GetConnectorByIdentifier]
     @id INT,
	 @stationId UNIQUEIDENTIFIER
AS

    SELECT
        [Id],
        [StationId],
        [MaxCurrent]
    FROM
        [dbo].[Connectors]
    WHERE
        [Id] = @id AND [StationId] = @stationId