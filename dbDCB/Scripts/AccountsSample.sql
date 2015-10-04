Delete from Accounts;

Insert Into Accounts(AccountId, AccountName) Values(hierarchyid::GetRoot(), 'Accounts');
go
AddAccount '/', 'Deposits';
go
AddAccount '/', 'Cash';
go
AddAccount '/', 'Credit cards';
