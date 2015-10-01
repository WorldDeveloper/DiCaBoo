CREATE procedure [dbo].AddAccount(@parentId nvarchar(max), @accountName nvarchar(100))
AS
BEGIN
DECLARE @lastChild hierarchyid
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION

UPDATE Accounts 
SET @lastChild=LastChild=AccountId.GetDescendant(LastChild, NULL)
WHERE AccountId=hierarchyid::Parse(@parentId)
INSERT Accounts(AccountId, AccountName) VALUES(@lastChild, @accountName)
COMMIT
END;