CREATE PROCEDURE [dbo].[usp_UpdateGroupCurrent]
     @id UNIQUEIDENTIFIER,
	 @addedCurrent BIGINT,
     @subtractedCurrent BIGINT
AS

    UPDATE 
        [dbo].[Groups]
    SET 
        [ConsumedCapacity] = ([ConsumedCapacity] - @subtractedCurrent) + @addedCurrent
    WHERE 
        [Id] = @id