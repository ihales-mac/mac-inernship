USE [AspNetIdentity]
GO
/****** Object:  Table [dbo].[AspNetUserDetails]    Script Date: 8/27/2018 5:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[DOB] [datetime] NULL,
 CONSTRAINT [PK_dbo.AspNetUserDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserDetails_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserDetails] CHECK CONSTRAINT [FK_dbo.AspNetUserDetails_dbo.AspNetUsers_UserId]
GO
