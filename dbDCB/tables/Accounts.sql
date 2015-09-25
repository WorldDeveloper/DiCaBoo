CREATE TABLE [dbo].[Accounts]
(
	[AccountId] hierarchyid NOT NULL PRIMARY KEY,
	LastChild hierarchyid NULL,
	AccountName nvarchar(100) NOT NULL
)
