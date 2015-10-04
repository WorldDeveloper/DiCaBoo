CREATE VIEW [dbo].[Transactions]
	AS 
	SELECT OperationId, OperationDate, Credit.AccountName AS Credit, Debit.AccountName AS Debit, Summ , Note
	FROM Operations JOIN Accounts AS Credit ON CreditId=Credit.AccountId 
		JOIN Accounts AS Debit ON DebitId=Debit.AccountId
