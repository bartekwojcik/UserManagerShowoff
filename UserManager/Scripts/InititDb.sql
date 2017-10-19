IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
BEGIN
  DROP TABLE dbo.Users; 
END

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](255) NOT NULL,
	[SaltedPassword] [nvarchar](max) NOT NULL,	
	[Token] [nvarchar](max) NULL,
	[TokenExpirationDate] [datetime] NULL,
 CONSTRAINT [PK_Users_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
),
 CONSTRAINT [Login_unique] UNIQUE NONCLUSTERED 
(
	[Login] ASC
))




