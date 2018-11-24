Application Pool - ChessInfo - Identity:  ApplicationPoolIdentity
SQL Server:
	- Create new database: ChessInfo
	- Open Sql Management Studio, open & run LoginAndUserCreation.sql
	- In Sql Management Studio, expand Databases/ChessInfo/Security/Users
	- Right click IIS APPPOOL\ChessInfo -> Properties -> Membership and select db_owner
	Server Security
	- add login IIS APPPOOL\ChessInfo
	- user mapping: ChessInfo database- user IIS APPPOOL\ChessInfo
	ChessInfo db Security: 
	- Users -> IIS APPPOOL\ChessInfo - Properties -> Membership - db_owner

https://blogs.msdn.microsoft.com/ericparvin/2015/04/14/how-to-add-the-applicationpoolidentity-to-a-sql-server-login/

Visual Studio 2017
	Package Manager Console: update-database
