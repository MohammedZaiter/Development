CREATE PROCEDURE [dbo].[usp_GetStationById]
	@id UNIQUEIDENTIFIER
AS
	SELECT 
		[Id],
		[Name],
		[GroupId]
	FROM
		[dbo].[Stations]
	WHERE 
		[Id] = @id			
