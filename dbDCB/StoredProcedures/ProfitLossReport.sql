CREATE PROCEDURE [dbo].[ProfitLossReport]
	@parentId nvarchar(max),
	@startDate datetime2(0)=null,
	@endDate datetime2(0)=null
AS
BEGIN
DECLARE @parent hierarchyid
SET @parent=hierarchyid::Parse(@parentId)

IF  (@startDate IS NOT NULL) AND (@endDate IS NOT NULL)
		SELECT AccountId, AccountName, 
			ISNULL((SELECT SUM(Summ) FROM Operations WHERE CreditId.IsDescendantOf(AccountId)=1 AND OperationDate BETWEEN @startDate AND @endDate),0)
			-(ISNULL((SELECT SUM(Summ) FROM Operations WHERE DebitId.IsDescendantOf(AccountId)=1 AND OperationDate BETWEEN @startDate AND @endDate),0)) AS Balance
		FROM Accounts WHERE AccountId.IsDescendantOf(@parent)=1
ELSE IF (@startDate IS NOT NULL) AND (@endDate IS NULL)
		SELECT AccountId, AccountName, 
			ISNULL((SELECT SUM(Summ) FROM Operations WHERE CreditId.IsDescendantOf(AccountId)=1 AND OperationDate >= @startDate),0)
			-(ISNULL((SELECT SUM(Summ) FROM Operations WHERE DebitId.IsDescendantOf(AccountId)=1 AND OperationDate >= @startDate),0)) AS Balance
		FROM Accounts WHERE AccountId.IsDescendantOf(@parent)=1
ELSE IF (@startDate IS NULL) AND (@endDate IS NOT NULL)
		SELECT AccountId, AccountName, 
			ISNULL((SELECT SUM(Summ) FROM Operations WHERE CreditId.IsDescendantOf(AccountId)=1 AND OperationDate < @endDate),0)
			-(ISNULL((SELECT SUM(Summ) FROM Operations WHERE DebitId.IsDescendantOf(AccountId)=1 AND OperationDate < @endDate),0)) AS Balance
		FROM Accounts WHERE AccountId.IsDescendantOf(@parent)=1
ELSE
		SELECT AccountId, AccountName, 
			ISNULL((SELECT SUM(Summ) FROM Operations WHERE CreditId.IsDescendantOf(AccountId)=1),0)
			-(ISNULL((SELECT SUM(Summ) FROM Operations WHERE DebitId.IsDescendantOf(AccountId)=1),0)) AS Balance
		FROM Accounts WHERE AccountId.IsDescendantOf(@parent)=1
END
