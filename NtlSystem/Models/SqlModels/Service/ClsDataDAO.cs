using NtlSystem.Models.SqlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtlSystem.Models.SqlModels.Service
{
    interface ClsDataDAO
    {
        List<ClsData> GetAllClsData(DAL dal, string tblName);

        List<ClsData> GetAllClsDataQuery(DAL dal, string query);
        ClsData GetSingleClsDataQuery(DAL dal, string query);
        ClsData FindClsData(DAL dal, string tblName, int id);
        ClsData FindByName(DAL dal, string tblName, string name);
        void AddClsData(DAL dal, string tblName, string dataType);
        void UpdateClsData(DAL dal, string tblName, string dataType);
        void DeleteClsData(DAL dal, string tblName, int id);

    }
}
