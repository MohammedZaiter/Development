CREATE PROCEDURE [dbo].[usp_UpdateStationCurrent]
     @id UNIQUEIDENTIFIER,
	 @addedCurrent BIGINT,
     @subtractedCurrent BIGINT
AS

    UPDATE 
        [dbo].[Stations]
    SET 
        [ConsumedCurrent] = ([ConsumedCurrent] - @subtractedCurrent) + @addedCurrent
    WHERE 
        [Id] = @id