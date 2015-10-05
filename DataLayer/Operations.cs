using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Operations
    {

        //public static int UpdateTransaction(Account updatedAccount)
        //{
        //    if (updatedAccount == null) return 0;

        //    using (SqlConnection connection = DB.SqlConnection)
        //    {
        //        using (SqlCommand command = connection.CreateCommand())
        //        {
        //            command.CommandText = @"UPDATE Accounts SET AccountName=@accountName WHERE AccountId=hierarchyid::Parse(@accountId);";
        //            command.Parameters.AddWithValue("@accountName", updatedAccount.AccountName);
        //            command.Parameters.AddWithValue("@accountId", updatedAccount.AccountId);

        //            connection.Open();
        //            return command.ExecuteNonQuery();
        //        }
        //    }
        //}

        public static int UpdateTransaction(int id, DateTime date, string credit, string debit, decimal amount, string note)
        {
            if (string.IsNullOrWhiteSpace(credit) || string.IsNullOrWhiteSpace(debit))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Operations SET OperationDate=@date, CreditId=sqlhierarchyid::Parse(@credit), DebitId=sqlhierarchy::Parse(@debit), Summ=@amount, Note=@note WHERE OperationId=@id;";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@credit", credit);
                    command.Parameters.AddWithValue("@debit", debit);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@note", note);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int AddTransaction(DateTime date, string credit, string debit, decimal amount, string note)
        {
            if (string.IsNullOrWhiteSpace(credit) || string.IsNullOrWhiteSpace(debit))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Operations VALUES(@date, sqlhierarchyid::Parse(@credit), sqlhierarchy::Parse(@debit), @amount, @note);";
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@credit", credit);
                    command.Parameters.AddWithValue("@debit", debit);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@note", note);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }


        public static int RemoveTransaction(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Operations WHERE OperationId=@operationId;";
                    command.Parameters.AddWithValue("@operationId", transactionId);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
