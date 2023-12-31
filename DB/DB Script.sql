USE [master]
GO
/****** Object:  Database [PracticalProjectDB]    Script Date: 13/09/2023 6:14:00 AM ******/
CREATE DATABASE [PracticalProjectDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PracticalProjectDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PracticalProjectDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PracticalProjectDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PracticalProjectDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PracticalProjectDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PracticalProjectDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PracticalProjectDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PracticalProjectDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PracticalProjectDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PracticalProjectDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PracticalProjectDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET RECOVERY FULL 
GO
ALTER DATABASE [PracticalProjectDB] SET  MULTI_USER 
GO
ALTER DATABASE [PracticalProjectDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PracticalProjectDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PracticalProjectDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PracticalProjectDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PracticalProjectDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PracticalProjectDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PracticalProjectDB', N'ON'
GO
ALTER DATABASE [PracticalProjectDB] SET QUERY_STORE = OFF
GO
USE [PracticalProjectDB]
GO
/****** Object:  Table [dbo].[ItemInfo]    Script Date: 13/09/2023 6:14:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemInfo](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](350) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ItemInfo] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillChilds]    Script Date: 13/09/2023 6:14:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillChilds](
	[BillDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[BillMasterID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[UnitPrice] [numeric](18, 2) NOT NULL,
	[ItemQty] [numeric](18, 2) NOT NULL,
	[TotalPrice] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_BillChilds] PRIMARY KEY CLUSTERED 
(
	[BillDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillMaster]    Script Date: 13/09/2023 6:14:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillMaster](
	[BillMasterID] [int] IDENTITY(1,1) NOT NULL,
	[BillDate] [datetime] NOT NULL,
	[CustomerName] [nvarchar](100) NOT NULL,
	[ContactNo] [nvarchar](100) NULL,
 CONSTRAINT [PK_BillMaster] PRIMARY KEY CLUSTERED 
(
	[BillMasterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Report]    Script Date: 13/09/2023 6:14:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Report]
AS
SELECT        dbo.BillMaster.BillMasterID, dbo.BillMaster.BillDate, dbo.BillMaster.CustomerName, dbo.BillMaster.ContactNo, dbo.ItemInfo.ItemName, dbo.BillChilds.UnitPrice, dbo.BillChilds.ItemQty, dbo.BillChilds.TotalPrice
FROM            dbo.BillMaster INNER JOIN
                         dbo.BillChilds ON dbo.BillMaster.BillMasterID = dbo.BillChilds.BillMasterID INNER JOIN
                         dbo.ItemInfo ON dbo.BillChilds.ItemID = dbo.ItemInfo.ItemID
GO
SET IDENTITY_INSERT [dbo].[BillChilds] ON 

INSERT [dbo].[BillChilds] ([BillDetailsID], [BillMasterID], [ItemID], [UnitPrice], [ItemQty], [TotalPrice]) VALUES (50, 27, 1, CAST(1.00 AS Numeric(18, 2)), CAST(2.00 AS Numeric(18, 2)), CAST(2.00 AS Numeric(18, 2)))
INSERT [dbo].[BillChilds] ([BillDetailsID], [BillMasterID], [ItemID], [UnitPrice], [ItemQty], [TotalPrice]) VALUES (51, 27, 7, CAST(2.00 AS Numeric(18, 2)), CAST(2.00 AS Numeric(18, 2)), CAST(4.00 AS Numeric(18, 2)))
INSERT [dbo].[BillChilds] ([BillDetailsID], [BillMasterID], [ItemID], [UnitPrice], [ItemQty], [TotalPrice]) VALUES (55, 51, 4, CAST(7.00 AS Numeric(18, 2)), CAST(2.00 AS Numeric(18, 2)), CAST(14.00 AS Numeric(18, 2)))
INSERT [dbo].[BillChilds] ([BillDetailsID], [BillMasterID], [ItemID], [UnitPrice], [ItemQty], [TotalPrice]) VALUES (57, 51, 1, CAST(5.00 AS Numeric(18, 2)), CAST(6.00 AS Numeric(18, 2)), CAST(30.00 AS Numeric(18, 2)))
SET IDENTITY_INSERT [dbo].[BillChilds] OFF
GO
SET IDENTITY_INSERT [dbo].[BillMaster] ON 

INSERT [dbo].[BillMaster] ([BillMasterID], [BillDate], [CustomerName], [ContactNo]) VALUES (27, CAST(N'2023-09-10T00:00:00.000' AS DateTime), N'My Rahim', N'172458')
INSERT [dbo].[BillMaster] ([BillMasterID], [BillDate], [CustomerName], [ContactNo]) VALUES (51, CAST(N'2023-09-13T00:00:00.000' AS DateTime), N'test 3', N'0183225518')
SET IDENTITY_INSERT [dbo].[BillMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[ItemInfo] ON 

INSERT [dbo].[ItemInfo] ([ItemID], [ItemName], [IsActive]) VALUES (1, N'Mouse', 1)
INSERT [dbo].[ItemInfo] ([ItemID], [ItemName], [IsActive]) VALUES (4, N'Keyboard', 1)
INSERT [dbo].[ItemInfo] ([ItemID], [ItemName], [IsActive]) VALUES (5, N'HardDisk', 1)
INSERT [dbo].[ItemInfo] ([ItemID], [ItemName], [IsActive]) VALUES (6, N'RAM', 1)
INSERT [dbo].[ItemInfo] ([ItemID], [ItemName], [IsActive]) VALUES (7, N'Processor', 1)
SET IDENTITY_INSERT [dbo].[ItemInfo] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__ItemInfo__4E4373F7E47751CA]    Script Date: 13/09/2023 6:14:00 AM ******/
ALTER TABLE [dbo].[ItemInfo] ADD  CONSTRAINT [UQ__ItemInfo__4E4373F7E47751CA] UNIQUE NONCLUSTERED 
(
	[ItemName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ItemInfo] ADD  CONSTRAINT [DF_ItemInfo_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BillChilds]  WITH CHECK ADD  CONSTRAINT [FK_BillChilds_BillMaster] FOREIGN KEY([BillMasterID])
REFERENCES [dbo].[BillMaster] ([BillMasterID])
GO
ALTER TABLE [dbo].[BillChilds] CHECK CONSTRAINT [FK_BillChilds_BillMaster]
GO
/****** Object:  StoredProcedure [dbo].[spMasterDetail]    Script Date: 13/09/2023 6:14:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spMasterDetail]  
(  
    @BillMasterID INT = NULL,  
    @BillDate Datetime = NULL,  
    @CustomerName VARCHAR(250) = NULL,  
    @ContactNo VARCHAR(20) = NULL,  
	@BillDetailsID INT = NULL, 
	@ItemID INT = NULL, 
	@UnitPrice numeric(18,2) = NULL, 
	@ItemQty numeric(18,2) = NULL,  
	@TotalPrice numeric(18,2) = NULL,
	@id int output,
    @ActionType VARCHAR(25)  
)  
AS  
BEGIN  
    IF @ActionType = 'SaveOrUpdateMasterData'  
    BEGIN  
        IF NOT EXISTS (SELECT * FROM BillMaster WHERE BillMasterID=@BillMasterID)  
        BEGIN  
            INSERT INTO BillMaster (BillDate, CustomerName, ContactNo)  
            VALUES (@BillDate, @CustomerName, @ContactNo)  
			SET @id=SCOPE_IDENTITY()
            RETURN  @id
        END  
        ELSE  
        BEGIN  
            UPDATE BillMaster SET BillDate=@BillDate, CustomerName=@CustomerName, ContactNo=@ContactNo WHERE BillMasterID=@BillMasterID  
			SET @id=1
		    RETURN  @id
        END  
    END  
    IF @ActionType = 'DeleteMasterData'  
    BEGIN  
        DELETE BillMaster WHERE BillMasterID=@BillMasterID  
		SET @id=1
		RETURN  @id
    END  
	IF @ActionType = 'DeleteDetailData'  
    BEGIN  
        DELETE BillChilds WHERE BillDetailsID=@BillDetailsID  
		SET @id=1
		RETURN  @id
    END  
	IF @ActionType = 'DeleteDetailDataByMasterID'  
    BEGIN  
        DELETE BillChilds WHERE BillMasterID=@BillMasterID  
		SET @id=1
		RETURN  @id
    END  
    IF @ActionType = 'GetAllMasterData'  
    BEGIN  
       SELECT BillMasterID,convert(varchar, BillDate, 103) as BillDate, CustomerName, ContactNo FROM dbo.BillMaster ORDER BY BillMasterID DESC
	   SET @id=1
	   RETURN  @id
    END  
    IF @ActionType = 'GetMasterDataByID'  
    BEGIN  
        SELECT BillMasterID,BillDate=format(BillDate,'yyyy-MM-dd'), CustomerName, ContactNo FROM dbo.BillMaster WHERE BillMasterID=@BillMasterID 
		SET @id=1
		RETURN  @id
    END  

    IF @ActionType = 'GetDetailDataByID'  
    BEGIN  
        SELECT BillDetailsID, ItemID, UnitPrice, ItemQty, TotalPrice FROM BillChilds WHERE BillMasterID=@BillMasterID 
		SET @id=1
		RETURN  @id
    END  

	 IF @ActionType = 'SaveOrUpdateDetailData'  
    BEGIN  
        IF NOT EXISTS (SELECT * FROM BillChilds WHERE BillDetailsID=@BillDetailsID)  
        BEGIN  
            INSERT INTO BillChilds (BillMasterID, ItemID, UnitPrice, ItemQty, TotalPrice) VALUES (@BillMasterID, @ItemID, @UnitPrice, @ItemQty, @TotalPrice)  
			SET @id=SCOPE_IDENTITY()
            RETURN  @id
        END  
        ELSE  
        BEGIN  
            UPDATE BillChilds SET BillMasterID=@BillMasterID, ItemID=@ItemID, UnitPrice=@UnitPrice, ItemQty=@ItemQty, TotalPrice=@TotalPrice WHERE BillDetailsID=@BillDetailsID 
			SET @id=1
			RETURN  @id
        END  
    END  
END  
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[8] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "BillMaster"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 197
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BillChilds"
            Begin Extent = 
               Top = 6
               Left = 249
               Bottom = 210
               Right = 419
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ItemInfo"
            Begin Extent = 
               Top = 6
               Left = 457
               Bottom = 119
               Right = 627
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Report'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Report'
GO
USE [master]
GO
ALTER DATABASE [PracticalProjectDB] SET  READ_WRITE 
GO
