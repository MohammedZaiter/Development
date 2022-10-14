CREATE PROCEDURE [dbo].[usp_GetConnectorsByStationId]
	 @stationId UNIQUEIDENTIFIER
AS

	SELECT 
		[Id],
		[MaxCurrent],
		@stationId AS StationId
	FROM
		[dbo].[Connectors]
	WHERE
		[StationId] = @stationId
