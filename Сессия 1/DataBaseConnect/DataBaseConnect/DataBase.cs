using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataBaseConnect
{
    public class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source = ADCLG1; Initial Catalog = LB_5_Гойсултанов; Integrated Security = True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }


        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }


        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
