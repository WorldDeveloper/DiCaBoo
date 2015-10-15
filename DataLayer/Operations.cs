using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Operation
    {
        public int ID { get; private set; }
        public DateTime Date { get; private set; }
        public Account Credit { get; private set; }
        public Account Debit { get; private set; }
        public decimal Amount { get; private set; }
        public string Note { get; set; }

        public Operation(int id, DateTime date, Account credit, Account debit, decimal amount, string note)
        {
            ID = id;
            Date = date;
            Credit = credit;
            Debit = debit;
            Amount = amount;
            Note = note;
        }
    }

    public class Operations
    {
        public static Operation GetTransaction(int id)
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetTransaction";
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Operation(
                                (int)reader["OperationId"],
                                 (DateTime)reader["OperationDate"],
                                 new Account(reader["CreditId"].ToString(), reader["CreditName"].ToString()),
                                 new Account(reader["DebitId"].ToString(), reader["DebitName"].ToString()),
                                 (decimal)reader["Summ"],
                                 reader["Note"].ToString()
                                 );
                        }
                    }
                }
            }
            return null;
        }

        public static int UpdateTransaction(int id, DateTime date, string credit, string debit, decimal amount, string note)
        {
            if (string.IsNullOrWhiteSpace(credit) || string.IsNullOrWhiteSpace(debit))
                return 0;

            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Operations SET OperationDate=@date, CreditId=hierarchyid::Parse(@credit), DebitId=hierarchyid::Parse(@debit), Summ=@amount, Note=@note WHERE OperationId=@id;";
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
                    command.CommandText = "INSERT INTO Operations VALUES(@date, hierarchyid::Parse(@credit), hierarchyid::Parse(@debit), @amount, @note);";
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
