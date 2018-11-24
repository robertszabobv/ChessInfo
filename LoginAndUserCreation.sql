USE [master]
GO

/****** Object:  Login [IIS APPPOOL\ChessInfo]    Script Date: 11/24/2018 10:08:34 PM ******/
CREATE LOGIN [IIS APPPOOL\ChessInfo] FROM WINDOWS WITH DEFAULT_DATABASE=[ChessInfo], DEFAULT_LANGUAGE=[us_english]
GO

ALTER SERVER ROLE [dbcreator] ADD MEMBER [IIS APPPOOL\ChessInfo]
GO


USE [ChessInfo]
GO

/****** Object:  User [IIS APPPOOL\ChessInfo]    Script Date: 11/24/2018 10:08:59 PM ******/
CREATE USER [IIS APPPOOL\ChessInfo] FOR LOGIN [IIS APPPOOL\ChessInfo] WITH DEFAULT_SCHEMA=[dbo]
GO

