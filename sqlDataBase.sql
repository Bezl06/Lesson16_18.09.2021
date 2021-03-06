USE [TestDB]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 18.09.2021 22:25:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account] [nvarchar](5) NOT NULL,
	[Is_Active] [int] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 18.09.2021 22:25:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Created_At] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([Id], [Account], [Is_Active], [Created_At], [Updated_At]) VALUES (1, N'00001', 1, CAST(N'2021-09-17T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Accounts] ([Id], [Account], [Is_Active], [Created_At], [Updated_At]) VALUES (4, N'00002', 1, CAST(N'2021-09-17T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Accounts] ([Id], [Account], [Is_Active], [Created_At], [Updated_At]) VALUES (5, N'00003', 1, CAST(N'2021-09-17T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[Accounts] ([Id], [Account], [Is_Active], [Created_At], [Updated_At]) VALUES (6, N'00004', 1, CAST(N'2021-09-18T00:22:11.677' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Transactions] ON 

INSERT [dbo].[Transactions] ([Id], [Account_Id], [Amount], [Created_At]) VALUES (1, 1, CAST(100.00 AS Decimal(18, 2)), CAST(N'2021-09-17T00:00:00.000' AS DateTime))
INSERT [dbo].[Transactions] ([Id], [Account_Id], [Amount], [Created_At]) VALUES (6, 4, CAST(150.00 AS Decimal(18, 2)), CAST(N'2021-09-17T00:00:00.000' AS DateTime))
INSERT [dbo].[Transactions] ([Id], [Account_Id], [Amount], [Created_At]) VALUES (7, 5, CAST(40.00 AS Decimal(18, 2)), CAST(N'2021-09-17T00:00:00.000' AS DateTime))
INSERT [dbo].[Transactions] ([Id], [Account_Id], [Amount], [Created_At]) VALUES (1004, 6, CAST(-80.00 AS Decimal(18, 2)), CAST(N'2021-09-17T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Transactions] OFF
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_Is_Active]  DEFAULT ((1)) FOR [Is_Active]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Accounts] ([Id])
GO
