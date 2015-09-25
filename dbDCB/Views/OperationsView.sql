CREATE VIEW [dbo].[OperationsView]
	AS SELECT CreditId, Credit.AccountName AS CreditName, DebitId, Debit.AccountName AS DebitName, Summ, Note  
	FROM Operations 
	JOIN Accounts AS Credit ON Operations.CreditId=Credit.AccountId 
	JOIN Accounts AS Debit ON Operations.DebitId=Debit.AccountId
