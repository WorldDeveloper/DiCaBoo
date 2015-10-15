using Microsoft.SqlServer.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Account
    {
        private string mAccountId;
        public string AccountId
        {
            get
            {
                return mAccountId;
            }
        }

        private string mAccountName;
        public string AccountName
        {
            get
            {
                return mAccountName;
            }
        }

        public Account(string accountId, string accountName)
        {
            mAccountId = accountId;
            mAccountName = accountName;
        }

        public override string ToString()
        {
            return AccountName;
        }
    }
    public struct ShortAccountNode
    {
        public Account RootAccount { get; set; }
        public List<ShortAccountNode> ChildAccounts { get; set; }
        public override string ToString()
        {
            return RootAccount.AccountName;
        }
    }
    public class AccountInfo
    {
        private SqlHierarchyId mAccountId;
        public SqlHierarchyId AccountId
        {
            get
            {
                return mAccountId;
            }
        }

        private string mAccountName;
        public string AccountName
        {
            get
            {
                return mAccountName;
            }
        }
        private decimal mBalance;
        public decimal Balance
        {
            get
            {
                return mBalance;
            }
        }

        public AccountInfo(SqlHierarchyId accountId, string accountName, decimal balance)
        {
            mAccountId = accountId;
            mAccountName = accountName;
            mBalance = balance;
        }
    }

    public struct AccountNode
    {
        public AccountInfo RootAccount { get; set; }
        public List<AccountNode> ChildAccounts { get; set; }
    }

    public struct ChartPoint
    {
        public DateTime Date { get; private set; }
        public Decimal Value { get; private set; }
        public ChartPoint(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }
    }

    public class Accounts : IEnumerable<Account>
    {
        private List<Account> mAccounts;

        public Accounts()
        {
            mAccounts = new List<Account>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select AccountId, AccountName from Accounts";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mAccounts.Add(new Account(
                               reader["AccountId"].ToString(),
                                reader["AccountName"].ToString())
                                );
                        }
                    }
                }
            }
        }

        public static List<Account> GetList(string parent)
        {
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select AccountId, AccountName from Accounts WHERE AccountId.GetAncestor(1)=hierarchyid::Parse(@parentId)";
                    SqlParameter parentId = new SqlParameter("@parentId", SqlDbType.VarChar, -1);
                    parentId.Value = parent.ToString();
                    command.Parameters.Add(parentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accounts.Add(new Account(
                                reader["AccountId"].ToString(),
                                reader["AccountName"].ToString())
                                );
                        }

                        return accounts;
                    }
                }
            }
        }

        private static List<Account> mShortAccountsList;
        public static ShortAccountNode GetShortTree(string parent)
        {
            mShortAccountsList = new List<Account>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select AccountId, AccountName from Accounts WHERE AccountId.IsDescendantOf(hierarchyid::Parse(@parentId))=1";
                    SqlParameter parentId = new SqlParameter("@parentId", SqlDbType.VarChar, -1);
                    parentId.Value = parent.ToString();
                    command.Parameters.Add(parentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mShortAccountsList.Add(new Account(
                                reader["AccountId"].ToString(),
                                reader["AccountName"].ToString())
                                );
                        }
                    }
                }
            }

            ShortAccountNode accountNode = new ShortAccountNode();
            var rootAccount = from account in mShortAccountsList where account.AccountId.Equals(parent) select account;
            foreach (Account a in rootAccount)
                accountNode.RootAccount = a;

            accountNode.ChildAccounts = GetShortChildren(parent);

            return accountNode;
        }

        private static List<ShortAccountNode> GetShortChildren(string parent)
        {
            List<ShortAccountNode> childAccounts = new List<ShortAccountNode>();
            var children = from account in mShortAccountsList where SqlHierarchyId.Parse(account.AccountId).GetAncestor(1).ToString() == parent select account;
            foreach (Account account in children)
            {
                ShortAccountNode accountNode = new ShortAccountNode();
                accountNode.RootAccount = account;
                accountNode.ChildAccounts = GetShortChildren(account.AccountId);
                childAccounts.Add(accountNode);
            }

            return childAccounts;
        }



        private static List<AccountInfo> mAccountsList;
        public static AccountNode GetTree(string parent, DateTime date)
        {
            mAccountsList = new List<AccountInfo>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAccountsInfo";
                    command.Parameters.AddWithValue("@parentId", parent);
                    command.Parameters.AddWithValue("@date", date.Date.AddDays(1));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mAccountsList.Add(new AccountInfo(
                                SqlHierarchyId.Parse(reader["AccountId"].ToString()),
                                reader["AccountName"].ToString(),
                                (decimal)reader["Balance"]
                                ));
                        }
                    }
                }
            }

            AccountNode accountNode = new AccountNode();
            SqlHierarchyId root = SqlHierarchyId.Parse(parent);
            var rootAccount = from account in mAccountsList where account.AccountId.Equals(root) select account;
            foreach (AccountInfo a in rootAccount)
                accountNode.RootAccount = a;

            accountNode.ChildAccounts = GetChildren(root);

            return accountNode;
        }

        public static AccountNode ProfitLossReport(string parent, DateTime? startDate = null, DateTime? endDate = null)
        {
            mAccountsList = new List<AccountInfo>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ProfitLossReport";
                    command.Parameters.AddWithValue("@parentId", parent);
                    if (startDate != null)
                        command.Parameters.AddWithValue("@startDate", startDate.Value.Date);

                    if (endDate != null)
                        command.Parameters.AddWithValue("@endDate", endDate.Value.Date.AddDays(1));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mAccountsList.Add(new AccountInfo(
                                SqlHierarchyId.Parse(reader["AccountId"].ToString()),
                                reader["AccountName"].ToString(),
                                (decimal)reader["Balance"]
                                ));
                        }
                    }
                }
            }

            AccountNode accountNode = new AccountNode();
            SqlHierarchyId root = SqlHierarchyId.Parse(parent);
            var rootAccount = from account in mAccountsList where account.AccountId.Equals(root) select account;
            foreach (AccountInfo a in rootAccount)
                accountNode.RootAccount = a;

            accountNode.ChildAccounts = GetChildren(root);

            return accountNode;
        }

        public enum GroupBy { Day, Month, Year };
        public static List<ChartPoint> GetAmounts(string parent, GroupBy groupBy, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<ChartPoint> amountsList = new List<ChartPoint>();
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    string clause = null;
                    if (parent.StartsWith("/2/"))
                        clause = " WHERE CreditId.IsDescendantOf(@parentId)=1 ";
                    else if (parent.StartsWith("/3/"))
                        clause = " WHERE DebitId.IsDescendantOf(@parentId)=1 ";
                    else
                        return null;

                    if (startDate != null)
                    {
                        clause += " AND OperationDate>=@startDate ";
                        command.Parameters.AddWithValue("@startDate", startDate.Value.Date);
                    }

                    if (endDate != null)
                    {
                        clause += " AND OperationDate<@endDate ";
                        command.Parameters.AddWithValue("@endDate", endDate.Value.Date.AddDays(1));
                    }

                    if (groupBy == GroupBy.Year)
                        command.CommandText = string.Format("SELECT Datefromparts(Year(OperationDate), 1, 1) AS Date, Sum(Summ) AS Value FROM Operations {0} Group by Year(OperationDate)", clause);
                    else if (groupBy == GroupBy.Month)
                        command.CommandText = string.Format("SELECT Datefromparts(Year(OperationDate), Month(OperationDate), 1) AS Date, Sum(Summ) AS Value FROM Operations {0} Group by Year(OperationDate), Month(OperationDate)", clause);
                    else if (groupBy == GroupBy.Day)
                        command.CommandText = string.Format("SELECT CAST(OperationDate AS DATE) AS Date, Sum(Summ) AS Value FROM Operations {0} Group by CAST(OperationDate AS DATE)", clause);
                    else
                        return null;

                    command.Parameters.AddWithValue("@parentId", parent);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            amountsList.Add(new ChartPoint(
                                DateTime.Parse(reader["Date"].ToString()),
                                (decimal)reader["Value"]
                                ));
                        }

                        return amountsList;
                    }
                }
            }
        }

        private static List<AccountNode> GetChildren(SqlHierarchyId parent)
        {
            List<AccountNode> childAccounts = new List<AccountNode>();
            var children = from account in mAccountsList where account.AccountId.GetAncestor(1).Equals(parent) select account;
            foreach (AccountInfo account in children)
            {
                AccountNode accountNode = new AccountNode();
                accountNode.RootAccount = account;
                accountNode.ChildAccounts = GetChildren(account.AccountId);
                childAccounts.Add(accountNode);
            }

            return childAccounts;
        }


        public static int UpdateAccount(Account updatedAccount)
        {
            if (updatedAccount == null) return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE Accounts SET AccountName=@accountName WHERE AccountId=hierarchyid::Parse(@accountId);";
                    command.Parameters.AddWithValue("@accountName", updatedAccount.AccountName);
                    command.Parameters.AddWithValue("@accountId", updatedAccount.AccountId);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int AddAccount(string parentId, string accountName)
        {
            if (string.IsNullOrWhiteSpace(parentId) || string.IsNullOrWhiteSpace(accountName))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AddAccount";
                    command.Parameters.AddWithValue("@parentId", parentId);
                    command.Parameters.AddWithValue("@accountName", accountName);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }


        public static int RemoveAccount(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Accounts WHERE AccountId=hierarchyid::Parse(@accountId);";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator<Account> GetEnumerator()
        {
            return mAccounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return mAccounts.GetEnumerator();
        }
    }
}

