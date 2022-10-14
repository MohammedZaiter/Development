CREATE PROCEDURE [dbo].[usp_RemoveGroup]
	 @id UNIQUEIDENTIFIER
AS
	BEGIN TRANSACTION;

		BEGIN TRY;

			DELETE FROM [dbo].[Connectors]
			WHERE
				[StationId] IN (
					SELECT
						[Id]
					FROM 
						[dbo].[Stations]
					WHERE 
						[GroupId] = @id
				);

			DELETE FROM [dbo].[Stations]
			WHERE
				[GroupId] = @id

			DELETE FROM [dbo].[Groups]
			WHERE
				[Id] = @id

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
			THROW;
		END CATCH

	COMMIT TRANSACTION;
        