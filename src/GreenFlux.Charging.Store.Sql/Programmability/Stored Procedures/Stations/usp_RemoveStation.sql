CREATE PROCEDURE [dbo].[usp_RemoveStation]
	 @id UNIQUEIDENTIFIER
AS

	BEGIN TRANSACTION;

		BEGIN TRY;

			DELETE FROM [dbo].[Connectors]
			WHERE
				[StationId] = @id

			DELETE FROM [dbo].[Stations]
			WHERE 
				[Id] = @id

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
			THROW;
		END CATCH

	COMMIT TRANSACTION;