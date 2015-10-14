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
go
AddAccount '/2/', 'Salary';
go
AddAccount '/2/', 'Deposit interest';
go
AddAccount '/3/', 'Food';
go
AddAccount '/3/', 'Clothes';
go
AddAccount '/3/', 'Entertainment';
