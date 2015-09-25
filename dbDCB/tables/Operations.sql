CREATE TABLE [dbo].[Operations]
(
	[OperationId] INT NOT NULL PRIMARY KEY,
	Credit hierarchyid NOT NULL REFERENCES Accounts(AccountId),
	Debit hierarchyid NOT NULL REFERENCES Accounts(AccountId),
	Summ money not null CHECK (Summ>0),
	Note nvarchar(200)
)
