CREATE PROCEDURE [dbo].[usp_UpdateGroup]
	 @id UNIQUEIDENTIFIER,
     @name VARCHAR(30), 
     @capacity BIGINT
AS
    UPDATE  
        [dbo].[Groups]
    SET
        [Name] = @name, [Capacity] = @capacity
    WHERE
        [Id] = @id