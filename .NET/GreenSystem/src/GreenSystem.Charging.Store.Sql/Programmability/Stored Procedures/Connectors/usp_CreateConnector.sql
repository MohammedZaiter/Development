CREATE PROCEDURE [dbo].[usp_CreateConnector]
     @id INT,
	 @stationId UNIQUEIDENTIFIER,
     @maxCurrent BIGINT
AS
    INSERT INTO [dbo].[Connectors] 
        ([Id],
        [StationId],
        [MaxCurrent])
    VALUEs
        (@id, @stationId, @maxCurrent)