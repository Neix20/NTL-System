using NtlSystem.Models.SqlModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtlSystem.Models.SqlModels
{
    class MicrosoftSqlDAL : DAL
    {
        private string ConnectionString;

        public MicrosoftSqlDAL(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public void ExecuteNonQuery(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public IDataReader GetDataReader(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            return cmd.ExecuteReader();
        }

    }
}
