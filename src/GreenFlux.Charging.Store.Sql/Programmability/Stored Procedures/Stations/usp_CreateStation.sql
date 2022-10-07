CREATE PROCEDURE [dbo].[usp_CreateStation]
	 @id UNIQUEIDENTIFIER,
     @name VARCHAR(30), 
     @groupId UNIQUEIDENTIFIER
AS

    INSERT INTO [dbo].[Stations] 
        ([Id], [Name], [GroupId])
    VALUEs
        (@id, @name, @groupId)