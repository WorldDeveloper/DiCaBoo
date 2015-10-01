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

