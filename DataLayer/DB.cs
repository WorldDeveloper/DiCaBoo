using System.Configuration;
using System.Data.SqlClient;


namespace DataLayer
{
    class DB
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["dbDCB"].ConnectionString;
            }
        }

        public static SqlConnection SqlConnection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }
    }
}
