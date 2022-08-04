using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtlSystem.Models.SqlModels
{
    interface DAL
    {
        void ExecuteNonQuery(string Query);
        IDataReader GetDataReader(string Query);
    }
}
