Insert Into Accounts(AccountId, AccountName) Values(hierarchyid::GetRoot(), 'Accounts');
go
AddAccount 0x58, 'Deposits';
go
AddAccount 0x58, 'Cash';
go
AddAccount 0x58, 'Credit cards';

select AccountId.ToString() from accounts;