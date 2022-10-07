CREATE PROCEDURE [dbo].[usp_GetGroupById]
	@id UNIQUEIDENTIFIER
AS
	SELECT 
		[Id],
		[Name],
		[Capacity],
		[ConsumedCapacity]
	FROM
		[dbo].[Groups]
	WHERE 
		[Id] = @id			
