CREATE procedure [dbo].AddAccount(@parentId hierarchyid, @accountName nvarchar(100))
AS
BEGIN
DECLARE @lastChild hierarchyid
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION

UPDATE Accounts 
SET @lastChild=LastChild=AccountId.GetDescendant(LastChild, NULL)
WHERE AccountId=@parentId
INSERT Accounts(AccountId, AccountName) VALUES(@lastChild, @accountName)
COMMIT
END;