CREATE PROCEDURE [dbo].[usp_GetStationsByGroupId]
	@groupId UNIQUEIDENTIFIER
AS
	SELECT 
		[Id],
		[Name],
		[GroupId]
	FROM
		[dbo].[Stations]
	WHERE 
		[GroupId] = @groupId			
