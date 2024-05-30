USE [master]
GO
/****** Object:  Database [PruebaTecnica]    Script Date: 29/05/2024 10:08:03 p. m. ******/
CREATE DATABASE [PruebaTecnica]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PruebaTecnica', FILENAME = N'/var/opt/mssql/data/PruebaTecnica.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PruebaTecnica_log', FILENAME = N'/var/opt/mssql/data/PruebaTecnica_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PruebaTecnica] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PruebaTecnica].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PruebaTecnica] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PruebaTecnica] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PruebaTecnica] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PruebaTecnica] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PruebaTecnica] SET ARITHABORT OFF 
GO
ALTER DATABASE [PruebaTecnica] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PruebaTecnica] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PruebaTecnica] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PruebaTecnica] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PruebaTecnica] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PruebaTecnica] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PruebaTecnica] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PruebaTecnica] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PruebaTecnica] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PruebaTecnica] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PruebaTecnica] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PruebaTecnica] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PruebaTecnica] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PruebaTecnica] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PruebaTecnica] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PruebaTecnica] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PruebaTecnica] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PruebaTecnica] SET RECOVERY FULL 
GO
ALTER DATABASE [PruebaTecnica] SET  MULTI_USER 
GO
ALTER DATABASE [PruebaTecnica] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PruebaTecnica] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PruebaTecnica] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PruebaTecnica] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PruebaTecnica] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PruebaTecnica] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PruebaTecnica', N'ON'
GO
ALTER DATABASE [PruebaTecnica] SET QUERY_STORE = ON
GO
ALTER DATABASE [PruebaTecnica] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PruebaTecnica]
GO
/****** Object:  Table [dbo].[Catalogo]    Script Date: 29/05/2024 10:08:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Catalogo](
	[IdCatalogo] [int] IDENTITY(1,1) NOT NULL,
	[NombreCatalogo] [nvarchar](150) NULL,
	[Code] [nvarchar](10) NULL,
	[Valor] [nvarchar](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ClusteredIndex-20240527-213153]    Script Date: 29/05/2024 10:08:03 p. m. ******/
CREATE CLUSTERED INDEX [ClusteredIndex-20240527-213153] ON [dbo].[Catalogo]
(
	[NombreCatalogo] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TarjetaCredito]    Script Date: 29/05/2024 10:08:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TarjetaCredito](
	[idTarjeta] [int] IDENTITY(1,1) NOT NULL,
	[noTarjeta] [nvarchar](150) NOT NULL,
	[tipoTarjeta] [int] NOT NULL,
	[limiteCredito] [int] NOT NULL,
	[saldoDisponible] [decimal](9, 2) NOT NULL,
	[idUsuario] [int] NOT NULL,
	[Activa] [bit] NULL,
 CONSTRAINT [PK_TarjetaCredito] PRIMARY KEY CLUSTERED 
(
	[idTarjeta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transacciones]    Script Date: 29/05/2024 10:08:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transacciones](
	[idTransaccion] [int] IDENTITY(1,1) NOT NULL,
	[tipoMovimiento] [int] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[descripcion] [nvarchar](400) NOT NULL,
	[monto] [decimal](9, 2) NOT NULL,
	[idTarjeta] [int] NOT NULL,
	[idMovimientoRef] [int] NULL,
 CONSTRAINT [PK_Transacciones] PRIMARY KEY CLUSTERED 
(
	[idTransaccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 29/05/2024 10:08:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[usuarioID] [int] IDENTITY(1,1) NOT NULL,
	[usuario] [nvarchar](50) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[direccion] [nvarchar](250) NOT NULL,
	[noDocumento] [nvarchar](10) NOT NULL,
	[diaCorte] [int] NULL,
	[password] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK__Usuarios__A5B1ABAED3388DA8] PRIMARY KEY CLUSTERED 
(
	[usuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Catalogo] ON 
GO
INSERT [dbo].[Catalogo] ([IdCatalogo], [NombreCatalogo], [Code], [Valor]) VALUES (2, N'Cuota Mínima', N'CUOTAMIN', N'0.05')
GO
INSERT [dbo].[Catalogo] ([IdCatalogo], [NombreCatalogo], [Code], [Valor]) VALUES (1, N'Intereses Bonificables', N'INTBON', N'0.25')
GO
INSERT [dbo].[Catalogo] ([IdCatalogo], [NombreCatalogo], [Code], [Valor]) VALUES (3, N'TIPOMOVIMIENTO', N'1', N'COMPRAS')
GO
INSERT [dbo].[Catalogo] ([IdCatalogo], [NombreCatalogo], [Code], [Valor]) VALUES (4, N'TIPOMOVIMIENTO', N'2', N'PAGOS')
GO
INSERT [dbo].[Catalogo] ([IdCatalogo], [NombreCatalogo], [Code], [Valor]) VALUES (5, N'TIPOTARJETA', N'1', N'MasterCard')
GO
INSERT [dbo].[Catalogo] ([IdCatalogo], [NombreCatalogo], [Code], [Valor]) VALUES (6, N'TIPOTARJETA', N'2', N'VISA')
GO
SET IDENTITY_INSERT [dbo].[Catalogo] OFF
GO
SET IDENTITY_INSERT [dbo].[TarjetaCredito] ON 
GO
INSERT [dbo].[TarjetaCredito] ([idTarjeta], [noTarjeta], [tipoTarjeta], [limiteCredito], [saldoDisponible], [idUsuario], [Activa]) VALUES (1, N'5540500001000004 ', 1, 5000, CAST(677.50 AS Decimal(9, 2)), 2, 1)
GO
INSERT [dbo].[TarjetaCredito] ([idTarjeta], [noTarjeta], [tipoTarjeta], [limiteCredito], [saldoDisponible], [idUsuario], [Activa]) VALUES (2, N'5020470001370055 ', 2, 4000, CAST(1508.90 AS Decimal(9, 2)), 2, 1)
GO
INSERT [dbo].[TarjetaCredito] ([idTarjeta], [noTarjeta], [tipoTarjeta], [limiteCredito], [saldoDisponible], [idUsuario], [Activa]) VALUES (3, N'4548812049400004 ', 1, 5000, CAST(4100.00 AS Decimal(9, 2)), 4, 1)
GO
INSERT [dbo].[TarjetaCredito] ([idTarjeta], [noTarjeta], [tipoTarjeta], [limiteCredito], [saldoDisponible], [idUsuario], [Activa]) VALUES (4, N'4548032003933011 ', 2, 3500, CAST(3241.45 AS Decimal(9, 2)), 4, 1)
GO
SET IDENTITY_INSERT [dbo].[TarjetaCredito] OFF
GO
SET IDENTITY_INSERT [dbo].[Transacciones] ON 
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1, 1, CAST(N'2024-05-25T00:00:00.000' AS DateTime), N'Mouse', CAST(-55.50 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (2, 1, CAST(N'2024-05-23T00:00:00.000' AS DateTime), N'Almuerzos', CAST(-25.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (3, 1, CAST(N'2024-08-05T00:00:00.000' AS DateTime), N'Pedido Uber', CAST(-35.60 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (4, 1, CAST(N'2024-04-25T00:00:00.000' AS DateTime), N'Ropa', CAST(-100.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (5, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'Ropa', CAST(-200.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (6, 1, CAST(N'2024-05-15T00:00:00.000' AS DateTime), N'Ropa', CAST(-200.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (7, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'Ropa', CAST(-200.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (8, 1, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Laptop', CAST(-400.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (9, 1, CAST(N'2024-05-30T00:00:00.000' AS DateTime), N'Tablet Samsung', CAST(-300.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (10, 1, CAST(N'2024-05-30T00:00:00.000' AS DateTime), N'Tablet Samsung', CAST(-300.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (11, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'Tablet Samsung', CAST(-300.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (12, 1, CAST(N'2024-05-28T00:00:00.000' AS DateTime), N'TV 32 Pulgadas', CAST(-400.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (13, 1, CAST(N'2024-01-05T00:00:00.000' AS DateTime), N'TV 32 Pulgadas', CAST(-400.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (14, 1, CAST(N'2024-05-23T00:00:00.000' AS DateTime), N'TV 32 Pulgadas', CAST(-400.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (15, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'Juego de Sala', CAST(-500.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (16, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'TV 32 Pulgadas', CAST(-400.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (17, 1, CAST(N'2024-05-14T00:00:00.000' AS DateTime), N'TV 32 Pulgadas', CAST(-400.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (18, 1, CAST(N'2024-05-23T00:00:00.000' AS DateTime), N'TV 32 Pulgadas', CAST(-400.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (19, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'Audifonos Gamer', CAST(-300.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (20, 1, CAST(N'2024-05-28T00:00:00.000' AS DateTime), N'Silla Gamer', CAST(-400.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (21, 1, CAST(N'2024-05-23T00:00:00.000' AS DateTime), N'Silla Gamer', CAST(-300.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (22, 1, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Laptop ASUS', CAST(-500.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (23, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'Zapatos', CAST(-300.00 AS Decimal(9, 2)), 2, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (35, 1, CAST(N'2024-05-15T00:00:00.000' AS DateTime), N'ubereat', CAST(-40.55 AS Decimal(9, 2)), 4, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (36, 1, CAST(N'2024-09-05T00:00:00.000' AS DateTime), N'Zapatos', CAST(-100.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (37, 2, CAST(N'2024-05-14T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(15.00 AS Decimal(9, 2)), 4, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (38, 1, CAST(N'2024-05-18T00:00:00.000' AS DateTime), N'Cafe', CAST(-15.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (39, 2, CAST(N'2024-11-05T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(82.00 AS Decimal(9, 2)), 4, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (40, 1, CAST(N'2024-05-30T00:00:00.000' AS DateTime), N'Restaurante', CAST(-245.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (41, 1, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Restaurante', CAST(-300.00 AS Decimal(9, 2)), 4, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (42, 1, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Teatro en Casa', CAST(-458.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (43, 2, CAST(N'2024-05-28T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(488.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (44, 1, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Restaurante', CAST(-450.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (45, 1, CAST(N'2024-05-30T00:00:00.000' AS DateTime), N'Restaurante', CAST(-400.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1002, 1, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Restaurante', CAST(-150.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1003, 2, CAST(N'2024-05-21T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(150.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1020, 2, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1021, 2, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1022, 2, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1023, 2, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1024, 2, CAST(N'2024-05-28T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1025, 2, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(40.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1026, 2, CAST(N'2024-05-28T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(60.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1027, 2, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1028, 2, CAST(N'2024-05-29T00:00:00.000' AS DateTime), N'Pago de Tarjeta', CAST(50.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1030, 1, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Restaurante', CAST(-10.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1031, 1, CAST(N'2024-05-27T00:00:00.000' AS DateTime), N'Compra de un Celular', CAST(-260.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1032, 1, CAST(N'2023-05-18T00:00:00.000' AS DateTime), N'Starbuck', CAST(-15.00 AS Decimal(9, 2)), 4, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1033, 2, CAST(N'2024-05-25T00:00:00.000' AS DateTime), N'Restaurante ', CAST(100.00 AS Decimal(9, 2)), 3, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1034, 1, CAST(N'2024-05-10T00:00:00.000' AS DateTime), N'Ben´s Coffe', CAST(-20.00 AS Decimal(9, 2)), 1, NULL)
GO
INSERT [dbo].[Transacciones] ([idTransaccion], [tipoMovimiento], [fecha], [descripcion], [monto], [idTarjeta], [idMovimientoRef]) VALUES (1035, 2, CAST(N'2024-05-10T00:00:00.000' AS DateTime), N'Pago Tarjeta de Credito', CAST(22.50 AS Decimal(9, 2)), 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[Transacciones] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 
GO
INSERT [dbo].[Usuarios] ([usuarioID], [usuario], [nombre], [direccion], [noDocumento], [diaCorte], [password]) VALUES (2, N'admin@pruebatecnica.com.sv', N'Administrador', N'Col. Los pepetos', N'01399400-3', 5, N'혦궨잗ﱟ轔獨䝵㺞ယ㢗吞ἢ㺵틁')
GO
INSERT [dbo].[Usuarios] ([usuarioID], [usuario], [nombre], [direccion], [noDocumento], [diaCorte], [password]) VALUES (4, N'yesenia@pruebatecnica.com.sv', N'Yesenia Hernandez', N'Col. Los Almendros', N'02600588-4', 5, N'혦궨잗ﱟ轔獨䝵㺞ယ㢗吞ἢ㺵틁')
GO
INSERT [dbo].[Usuarios] ([usuarioID], [usuario], [nombre], [direccion], [noDocumento], [diaCorte], [password]) VALUES (1003, N'Gabriel@gmail.com', N'Gabriel', N'Malaga', N'10360052-4', 5, N'혦궨잗ﱟ轔獨䝵㺞ယ㢗吞ἢ㺵틁')
GO
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
/****** Object:  Index [PK_Catalogo]    Script Date: 29/05/2024 10:08:04 p. m. ******/
ALTER TABLE [dbo].[Catalogo] ADD  CONSTRAINT [PK_Catalogo] PRIMARY KEY NONCLUSTERED 
(
	[IdCatalogo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-20240526-185806]    Script Date: 29/05/2024 10:08:04 p. m. ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20240526-185806] ON [dbo].[Transacciones]
(
	[idTarjeta] ASC,
	[fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TarjetaCredito] ADD  CONSTRAINT [DF_TarjetaCredito_Activa]  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_diaCorte]  DEFAULT ((5)) FOR [diaCorte]
GO
ALTER TABLE [dbo].[TarjetaCredito]  WITH CHECK ADD  CONSTRAINT [FK_TarjetaCredito_Usuarios] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Usuarios] ([usuarioID])
GO
ALTER TABLE [dbo].[TarjetaCredito] CHECK CONSTRAINT [FK_TarjetaCredito_Usuarios]
GO
ALTER TABLE [dbo].[Transacciones]  WITH CHECK ADD  CONSTRAINT [FK_Transacciones_TarjetaCredito] FOREIGN KEY([idTarjeta])
REFERENCES [dbo].[TarjetaCredito] ([idTarjeta])
GO
ALTER TABLE [dbo].[Transacciones] CHECK CONSTRAINT [FK_Transacciones_TarjetaCredito]
GO
/****** Object:  StoredProcedure [dbo].[sp_EstadoCuenta]    Script Date: 29/05/2024 10:08:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EstadoCuenta]
@idtarjeta int,
@mes int,
@periodo int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FechaIni DATETIME = (SELECT DATEFROMPARTS(@Periodo, @Mes, 01))
	DECLARE @FechaFin DATETIME = (SELECT EOMONTH(@FechaIni))

	DECLARE @FechaIniBalAnt DATETIME = (SELECT DATEFROMPARTS(@Periodo, @Mes-1, 01))
	DECLARE @FechaFinBalAnt DATETIME = (SELECT EOMONTH(@FechaIniBalAnt))

	DECLARE @MontoBalanceAnt DECIMAL(19,2) = 0
	DECLARE @MontoCompras DECIMAL(19,2) = 0
	DECLARE @MontoPagos DECIMAL(19,2) = 0
	DECLARE @SaldoTotal DECIMAL(19,2) = 0
	DECLARE @PorcPagoMinimo DECIMAL(4,2) = (SELECT CAST(Valor AS DECIMAL(4,2)) FROM Catalogo WHERE Code = 'CUOTAMIN')
	DECLARE @PorcIntBonificable DECIMAL(4,2) = (SELECT CAST(Valor AS DECIMAL(4,2)) FROM Catalogo WHERE Code = 'INTBON')
	DECLARE @IntBonificables DECIMAL(9,2) = 0
	DECLARE @PagoMinimo DECIMAL(9,2) = 0

	SELECT 
		@MontoCompras = ISNULL(SUM(CASE WHEN tipomovimiento = 1 THEN monto ELSE 0 END), 0),
		@MontoPagos = ISNULL(SUM(CASE WHEN tipomovimiento = 2 THEN monto ELSE 0 END), 0)
	FROM Transacciones 
	WHERE idTarjeta = @idtarjeta
	AND fecha BETWEEN @FechaIni AND @FechaFin
	GROUP BY idTarjeta

	SELECT 
		@MontoBalanceAnt = ISNULL(SUM(monto), 0)
	FROM Transacciones 
	WHERE idTarjeta = @idtarjeta
	AND fecha BETWEEN @FechaIniBalAnt AND @FechaFinBalAnt
	GROUP BY idTarjeta

	SET @SaldoTotal = ABS(@MontoBalanceAnt + @MontoCompras + @MontoPagos + @IntBonificables)
	SET @IntBonificables = ROUND(@SaldoTotal * @PorcIntBonificable, 2)
	SET @PagoMinimo = ROUND(@SaldoTotal * @PorcPagoMinimo, 2)
	SET @SaldoTotal += @IntBonificables

	SELECT 
		FORMAT(@FechaIni, 'dd-MMM-yyyy', 'es-SV') FechaIni,
		FORMAT(@FechaFin, 'dd-MMM-yyyy', 'es-SV') FechaPago,
		b.saldoDisponible,
		b.limiteCredito,
		c.nombre nombreUsuario,
		c.direccion,
		ABS(@MontoBalanceAnt) montoBalanceAnterior,
		ABS(@MontoCompras) montoCompras,
		@MontoPagos montoPagos,
		@SaldoTotal SaldoTotal,
		@PagoMinimo pagoMinimo,
		@IntBonificables interesesBonificables
	FROM TarjetaCredito b 
	INNER JOIN Usuarios c
	ON  c.usuarioID = b.idUsuario
	WHERE b.idTarjeta = @idtarjeta

	SELECT 
		ABS(CASE WHEN tipomovimiento = 1 THEN monto ELSE NULL END) montoCompra,
		CASE WHEN tipomovimiento = 2 THEN monto ELSE NULL END montoPagos,
		FORMAT(fecha, 'dd-MMM-yyyy', 'es-SV') fecha,
		descripcion
	FROM Transacciones 
	WHERE idTarjeta = @idtarjeta
	AND fecha BETWEEN @FechaIni AND @FechaFin
	ORDER BY fecha
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTarjetasCredito]    Script Date: 29/05/2024 10:08:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetTarjetasCredito]
@usuarioID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		a.idTarjeta,
		a.noTarjeta,
		b.Valor tipoTarjeta,
		a.limiteCredito
	FROM TarjetaCredito a
	INNER JOIN Catalogo b
	ON b.NombreCatalogo = 'TIPOTARJETA' AND CAST(a.tipoTarjeta AS NVARCHAR(10)) = b.Code
	WHERE a.idUsuario = @usuarioID AND a.Activa = 1
END
GO
/****** Object:  StoredProcedure [dbo].[sp_TransaccionInsertar]    Script Date: 29/05/2024 10:08:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_TransaccionInsertar]
@idTarjeta INT,
@descripcion NVARCHAR(400),
@fecha DATETIME,
@monto DECIMAL(9,2),
@TipoMovimiento INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @result TABLE (IdTransaccion INT);
	DECLARE @SaldoDisponibleTarjeta AS DECIMAL(9,2) = 0

	IF @TipoMovimiento = 1
	BEGIN
		SET @SaldoDisponibleTarjeta = (SELECT SaldoDisponible FROM TarjetaCredito 
									   WHERE idTarjeta = @idTarjeta AND Activa = 1)

		IF (@SaldoDisponibleTarjeta + @monto) < 0
		BEGIN
			RAISERROR ('Saldo insuficiente', 16, 10);

			RETURN;
		END
	END

	INSERT INTO [dbo].[Transacciones]
           ([tipoMovimiento]
           ,[fecha]
           ,[descripcion]
           ,[monto]
           ,[idTarjeta])
	OUTPUT inserted.idTransaccion
	INTO @result
     VALUES
           (@TipoMovimiento
           ,@fecha
           ,@descripcion
           ,@monto
           ,@idTarjeta)

	SELECT IdTransaccion FROM @result
END
GO
/****** Object:  StoredProcedure [dbo].[sp_TransaccionObtener]    Script Date: 29/05/2024 10:08:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[sp_TransaccionObtener]
	@idTarjeta int,
	@Periodo int,
	@Mes int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FechaIni DATETIME = (SELECT DATEFROMPARTS(@Periodo, @Mes, 01))
	DECLARE @FechaFin DATETIME = (SELECT EOMONTH(@FechaIni))

	SELECT 
		b.Valor tipoMovimiento,
		FORMAT(a.fecha, 'dd-MMM-yyyy', 'es-SV') fecha,
		a.descripcion,
		a.monto 
	FROM Transacciones a
	INNER JOIN Catalogo b
	ON b.NombreCatalogo = 'TIPOMOVIMIENTO' AND CAST(a.tipoMovimiento AS NVARCHAR(10)) = b.Code
	WHERE a.idTarjeta = @idTarjeta AND a.fecha BETWEEN @FechaIni AND @FechaFin
	ORDER BY fecha DESC

END
GO
/****** Object:  StoredProcedure [dbo].[sp_UsuariosInsert]    Script Date: 29/05/2024 10:08:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_UsuariosInsert]
@usuario AS NVARCHAR(50),
@nombre AS NVARCHAR(100),
@direccion AS NVARCHAR(250),
@noDocumento AS NVARCHAR(10),
@password AS NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON;
	
	IF NOT EXISTS(SELECT usuario FROM Usuarios WHERE usuario = @usuario)
		INSERT INTO Usuarios
		(usuario, nombre, direccion, noDocumento, [password])
		VALUES
		(@usuario, @nombre, @direccion, @noDocumento, HASHBYTES('SHA2_256', CAST(@password AS NVARCHAR(4000))));
	ELSE
		RAISERROR ('Éste usuario ya se encuentra registrado', 16, 10);

END
GO
USE [master]
GO
ALTER DATABASE [PruebaTecnica] SET  READ_WRITE 
GO
