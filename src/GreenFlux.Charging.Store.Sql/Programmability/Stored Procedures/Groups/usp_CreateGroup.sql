CREATE PROCEDURE [dbo].[usp_CreateGroup]
	 @id UNIQUEIDENTIFIER,
     @name VARCHAR(30), 
     @capacity BIGINT
AS

    INSERT INTO [dbo].[Groups] 
        ([Id], [Name], [Capacity])
    VALUEs
        (@id, @name, @capacity)