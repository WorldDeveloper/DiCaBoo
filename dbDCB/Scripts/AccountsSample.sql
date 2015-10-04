Delete from Accounts;

Insert Into Accounts(AccountId, AccountName) Values(hierarchyid::GetRoot(), 'Accounts');
go
AddAccount '/', 'My resources';
go
AddAccount '/', 'Incomes';
go
AddAccount '/', 'Expenses';
go
AddAccount '/1/', 'Cash';
go
AddAccount '/1/', 'Deposits';
go
AddAccount '/1/', 'Credit cards';
