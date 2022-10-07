CREATE PROCEDURE [dbo].[usp_UpdateConnectorCurrent]
     @id INT,
	 @stationId UNIQUEIDENTIFIER,
     @oldCurrent BIGINT,
     @newCurrent BIGINT
AS

    UPDATE
        [dbo].[Connectors]
    SET
        [MaxCurrent] = @newCurrent
    WHERE 
        [Id] = @id AND [StationId] = @stationId