CREATE TABLE [dbo].[Groups]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(30) NOT NULL, 
    [Capacity] BIGINT NOT NULL
    CONSTRAINT [CK_Groups_Column] CHECK ([Capacity] > 0)
)
