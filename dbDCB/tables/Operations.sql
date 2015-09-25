CREATE TABLE [dbo].[Operations]
(
	[OperationId] INT NOT NULL PRIMARY KEY,
	[CreditId] hierarchyid NOT NULL REFERENCES Accounts(AccountId),
	[DebitId] hierarchyid NOT NULL REFERENCES Accounts(AccountId),
	Summ money not null CHECK (Summ>0),
	Note nvarchar(200)
)
