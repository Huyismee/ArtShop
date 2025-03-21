USE [master]
GO
/****** Object:  Database [ArtShop]    Script Date: 3/7/2025 9:02:03 PM ******/
CREATE DATABASE [ArtShop2]
GO
USE [ArtShop2]
GO
/****** Object:  Table [dbo].[Art]    Script Date: 3/7/2025 9:02:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Art](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Image] [varchar](200) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [float] NOT NULL,
 CONSTRAINT [PK_Art] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/7/2025 9:02:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[PasswordHash] [varchar](50) NOT NULL,
	[Balance] [float] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Art] ON 

INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (1, 3, N'/hehe/huhu', N'cai lz ma', 4000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (2, 1, N'/hehe/hihi', N'cai lz ma', 10000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (3, 1, N'uploads\1\img\1.png', NULL, 0)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (4, 1, N'uploads\1\img\1.jpg', NULL, 0)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (5, 1, N'uploads\1\img\1.jpg', N'Test', 5000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (6, 1, N'uploads\1\img\1.jpg', N'Test2', 500)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (7, 1, N'uploads\1\img\6J0ET.png', N'Khokk', 100)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (8, 1, N'uploads\1\img\JJ8F06.png', N'Test landscape', 100)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (9, 1, N'uploads\1\img\NGO5TRU.jpg', N'Test3', 4000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (10, 1, N'uploads\1\img\BWE.jpg', N'HEHI', 3000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (11, 1, N'uploads\1\img\9LETE0.jpg', N'Test Subject
', 3000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (12, 1, N'uploads\1\img\RU8MXG5.jpg', N'Test Subject
', 3000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (13, 3, N'uploads\3\img\0HWKUIW.jpg', N'This is Art', 10000)
INSERT [dbo].[Art] ([Id], [UserId], [Image], [Description], [Price]) VALUES (14, 1, N'uploads\3\img\V1RVDKV.jpg', N'Test NodeJS API', 5000)
SET IDENTITY_INSERT [dbo].[Art] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Email], [Username], [PasswordHash], [Balance]) VALUES (1, N'huyhuyhuy@gmail.com', N'huyminer', N'quanghuan43', 11977000)
INSERT [dbo].[User] ([Id], [Email], [Username], [PasswordHash], [Balance]) VALUES (2, N'huy2@gmail.com', N'huy2312002', N'quanghuan43', 22000)
INSERT [dbo].[User] ([Id], [Email], [Username], [PasswordHash], [Balance]) VALUES (3, N'huy2312002@gmail.com', N'huyismeee', N'quanghuan43', 1000)
INSERT [dbo].[User] ([Id], [Email], [Username], [PasswordHash], [Balance]) VALUES (4, N'huy22@gmail.com', N'huytest', N'quanghuan43', 0)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[Art]  WITH CHECK ADD  CONSTRAINT [FK_Art_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Art] CHECK CONSTRAINT [FK_Art_User]
GO
USE [master]
GO
ALTER DATABASE [ArtShop] SET  READ_WRITE 
GO
