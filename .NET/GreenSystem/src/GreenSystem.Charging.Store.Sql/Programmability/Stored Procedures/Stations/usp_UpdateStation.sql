CREATE PROCEDURE [dbo].[usp_UpdateStation]
	 @id UNIQUEIDENTIFIER,
     @name VARCHAR(30), 
     @groupId UNIQUEIDENTIFIER

AS
    UPDATE  
        [dbo].[Stations]
    SET
        [Name] = @name, [GroupId] = @groupId
    WHERE
        [Id] = @id
