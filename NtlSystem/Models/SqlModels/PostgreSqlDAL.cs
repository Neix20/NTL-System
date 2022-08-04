using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtlSystem.Models.SqlModels
{
    class PostgreSqlDAL : DAL
    {
        private string ConnectionString;

        public PostgreSqlDAL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void ExecuteNonQuery(string Query)
        {
            NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public IDataReader GetDataReader(string Query)
        {
            NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
            return cmd.ExecuteReader();
        }
    }
}
