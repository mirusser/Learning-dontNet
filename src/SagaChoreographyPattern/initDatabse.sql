USE [SagaDB]
GO

CREATE TABLE [dbo].[User](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Address] [varchar](100) NOT NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Product](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[Type] [varchar](50) NOT NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Order](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UpdatedTime] [datetime] NOT NULL,
	[UserName] [varchar](50) NULL,
	FOREIGN KEY ([UserId]) REFERENCES [User](Id),
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[OrderDetail](
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductName] [varchar](50) NULL,
	FOREIGN KEY ([OrderId]) REFERENCES [Order](Id),
	FOREIGN KEY ([ProductId]) REFERENCES [Product](Id)
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Inventory](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	FOREIGN KEY ([ProductId]) REFERENCES [Product](Id)
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Warehouse](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[InventoryId] [int] NOT NULL,
	FOREIGN KEY ([InventoryId]) REFERENCES [Inventory](Id),
) ON [PRIMARY]

GO

CREATE PROCEDURE [dbo].[CREATE_ORDER]
	-- Add the parameters for the stored procedure here
	@userId int, 
	@userName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Order] VALUES (@userId, GETDATE(), @userName)

	SELECT @@IDENTITY
END

GO

CREATE PROCEDURE [dbo].[CREATE_ORDER_DETAILS]
	-- Add the parameters for the stored procedure here
	@orderId int, 
	@productId int,
	@quantity int,
	@productName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[OrderDetail]
	([OrderId],[ProductId],[Quantity],[ProductName]) VALUES (@orderId, @productId, @quantity, @productName)

END

GO

CREATE PROCEDURE [dbo].[UPDATE_INVENTORY]
	-- Add the parameters for the stored procedure here
	@productId int, 
	@quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Inventory] SET [Quantity] = @quantity WHERE [ProductId] = @productId

END

GO


