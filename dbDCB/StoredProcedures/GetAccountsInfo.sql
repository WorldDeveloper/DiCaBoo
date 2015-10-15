CREATE PROCEDURE dbo.GetAccountsInfo(@parentId nvarchar(max), @date datetime2(0))
AS
BEGIN
	DECLARE @parent hierarchyid
	SET @parent=hierarchyid::Parse(@parentId)

	SELECT AccountId, AccountName, 
		(ISNULL((SELECT SUM(Summ) FROM Operations WHERE DebitId.IsDescendantOf(AccountId)=1 AND OperationDate<=@date),0)
		-ISNULL((SELECT SUM(Summ) FROM Operations WHERE CreditId.IsDescendantOf(AccountId)=1 AND OperationDate<=@date),0)) AS Balance
		FROM Accounts WHERE AccountId.IsDescendantOf(@parent)=1 

	--SELECT Accounts.AccountId, Accounts.AccountName,  (ISNULL(SUM(Debit.Summ), 0)-ISNULL(SUM(Credit.Summ),0)) AS Balance
	--FROM Accounts LEFT JOIN Operations AS Credit ON Credit.CreditId.IsDescendantOf(AccountId)=1
	--	 LEFT JOIN Operations AS Debit ON Debit.DebitId.IsDescendantOf(AccountId)=1
	--WHERE AccountId.IsDescendantOf(@parent)=1
	--GROUP BY AccountId, AccountName;
END


