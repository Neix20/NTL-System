using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using NtlSystem.Models;

namespace NtlSystem.Models.SqlModels.Service.Impl
{
    class ClsDataDAOImpl : ClsDataDAO
    {
        public void AddClsData(DAL dal, string tblName, string dataType)
        {
            // Insert Without ID
            string query = (dal is MicrosoftSqlDAL) 
                ? $"EXEC NSP_{tblName}_Insert {dataType};"
                : (dal is PostgreSqlDAL) ? $"SELECT NSP_{tblName}_Insert({dataType})"
                : "";
            dal.ExecuteNonQuery(query);
        }

        public void DeleteClsData(DAL dal, string tblName, int id)
        {
            string query = (dal is MicrosoftSqlDAL) 
                ? $"EXEC NSP_{tblName}_Delete '{id}';"
                : (dal is PostgreSqlDAL) ? $"SELECT NSP_{tblName}_Delete('{id}')"
                : "";
            dal.ExecuteNonQuery(query);
        }

        public ClsData FindByName(DAL dal, string tblName, string name)
        {
            string query = $"SELECT * FROM {tblName} WHERE name = '{name}';";
            IDataReader rdr = dal.GetDataReader(query);

            if (rdr.Read())
            {
                Dictionary<string, string> propDict = Enumerable.Range(0, rdr.FieldCount).ToDictionary(rdr.GetName, rdr.GetValue).ToDictionary(k => k.Key, k => k.Value.ToString());
                string propJson = JsonConvert.SerializeObject(propDict);
                return new ClsData(propJson);
            }

            return new ClsData();
        }

        public ClsData FindClsData(DAL dal, string tblName, int id)
        {
            string query = $"SELECT * FROM {tblName} WHERE id = {id};";
            IDataReader rdr = dal.GetDataReader(query);

            Dictionary<string, string> propDict = Enumerable.Range(0, rdr.FieldCount).ToDictionary(rdr.GetName, rdr.GetValue).ToDictionary(k => k.Key, k => k.Value.ToString());
            string propJson = JsonConvert.SerializeObject(propDict);

            return new ClsData(propJson);
        }

        public List<ClsData> GetAllClsData(DAL dal, string tblName)
        {
            string query = $"SELECT * FROM {tblName};";
            IDataReader rdr = dal.GetDataReader(query);

            List<ClsData> ls = new List<ClsData>();
            while(rdr.Read())
            {
                Dictionary<string, string> propDict = Enumerable.Range(0, rdr.FieldCount).ToDictionary(rdr.GetName, rdr.GetValue).ToDictionary(k => k.Key, k => k.Value.ToString());
                string propJson = JsonConvert.SerializeObject(propDict);
                ls.Add(new ClsData(propJson));
            }

            return ls;
        }

        public List<ClsData> GetAllClsDataQuery(DAL dal, string query)
        {
            IDataReader rdr = dal.GetDataReader(query);

            List<ClsData> ls = new List<ClsData>();
            while (rdr.Read())
            {
                Dictionary<string, string> propDict = Enumerable.Range(0, rdr.FieldCount).ToDictionary(rdr.GetName, rdr.GetValue).ToDictionary(k => k.Key, k => k.Value.ToString());
                string propJson = JsonConvert.SerializeObject(propDict);
                ls.Add(new ClsData(propJson));
            }

            return ls;
        }

        public ClsData GetSingleClsDataQuery(DAL dal, string query)
        {
            IDataReader rdr = dal.GetDataReader(query);

            if (rdr.Read())
            {
                Dictionary<string, string> propDict = Enumerable.Range(0, rdr.FieldCount).ToDictionary(rdr.GetName, rdr.GetValue).ToDictionary(k => k.Key, k => k.Value.ToString());
                string propJson = JsonConvert.SerializeObject(propDict);
                return new ClsData(propJson);
            }

            return new ClsData();
        }

        public void UpdateClsData(DAL dal, string tblName, string dataType)
        {
            string query = (dal is MicrosoftSqlDAL) 
                ? $"EXEC NSP_{tblName}_Update {dataType};"
                : (dal is PostgreSqlDAL) ? $"SELECT NSP_{tblName}_Update({dataType});"
                : "";
            dal.ExecuteNonQuery(query);
        }
    }
}
