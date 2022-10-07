CREATE PROCEDURE [dbo].[usp_GetStationById]
	@id UNIQUEIDENTIFIER
AS
	SELECT 
		[Id],
		[Name],
		[GroupId],
		[ConsumedCurrent]
	FROM
		[dbo].[Stations]
	WHERE 
		[Id] = @id			
