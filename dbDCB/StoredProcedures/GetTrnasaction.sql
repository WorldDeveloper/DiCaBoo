CREATE PROCEDURE [dbo].[GetTransaction]
	@id int
AS
	SELECT OperationId, OperationDate, Credit.AccountName AS CreditName, Credit.AccountId AS CreditId, Debit.AccountName AS DebitName, Debit.AccountId AS DebitId, Summ , Note
	FROM Operations JOIN Accounts AS Credit ON CreditId=Credit.AccountId JOIN Accounts AS Debit ON DebitId=Debit.AccountId
	WHERE OperationId=@id;