CREATE TABLE [dbo].[Groups]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(30) NOT NULL, 
    [Capacity] BIGINT NOT NULL,
    [ConsumedCapacity] BIGINT NOT NULL DEFAULT 0
    CONSTRAINT [CK_Groups_Column] CHECK ([Capacity] > 0)
)
