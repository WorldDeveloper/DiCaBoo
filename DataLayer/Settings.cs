using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Settings
    {
        public int ID { get; }
        public string FacebookAppToken { get; }
        public string FacebookUserId { get; }

        public Settings()
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select * from Settings;";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ID = (int)reader["Id"];
                            FacebookAppToken = reader["FacebookAppToken"] as String;
                            FacebookUserId=reader["FacebookUserId"] as String;
                        }
                    }
                }
            }
        }

        public Settings(string facebookAppToken, string facebookUserId)
        {
            using (SqlConnection connection = DB.SqlConnection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Settings SET FacebookAppToken=@fbAppToken, FacebookUserId=@fbUserId WHERE id=1;";

                    SqlParameter fbAppToken = new SqlParameter("@fbAppToken", SqlDbType.NVarChar, -1);
                    fbAppToken.Value = facebookAppToken;
                    command.Parameters.Add(fbAppToken);

                    SqlParameter fbUserId = new SqlParameter("@fbUserId", SqlDbType.NVarChar, -1);
                    fbUserId.Value = facebookUserId;
                    command.Parameters.Add(fbUserId);

                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        ID = 1;
                        FacebookAppToken = facebookAppToken;
                        FacebookUserId = facebookUserId;
                    }


                }
            }
        }
    }
}
