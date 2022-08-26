using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.DashboardModels
{
    public class mDashboardSaleItem
    {
        public mDashboardSaleItem()
        {
            dataDict = new Dictionary<string, string>();
        }

        public mDashboardSaleItem(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            platform = dataDict["platform"];
            todayRuntime = Convert.ToInt32(dataDict["today_runtime"]);
            ntlSales = Convert.ToInt32(dataDict["ntl_sale"]);
            cancellation = 0;
            LMDSales = Convert.ToInt32(dataDict["lmd_sale"]);
            MTDSales = Convert.ToInt32(dataDict["mtd_sale"]);
            L5Sales = Convert.ToInt32(dataDict["l5_sale"]);
            status = dataDict["status"];
            lastRuntime = Convert.ToDateTime(dataDict["last_runtime"]);
        }

        public string platform { get; set; }
        public int todayRuntime { get; set; }
        public int ntlSales { get; set; }
        public int odooSales { get; set; }
        public int cancellation { get; set; }
        public int LMDSales { get; set; }
        public int MTDSales { get; set; }
        public int L5Sales { get; set; }
        public string status { get; set; }
        public DateTime lastRuntime { get; set; }

        public Dictionary<string, string> dataDict { get; set; }
    }
}