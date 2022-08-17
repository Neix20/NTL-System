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
            status = "Online";
        }

        public void GenSales(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            platform = dataDict["Platform"];
            totalSales = Convert.ToInt32(dataDict["Sales"]);
            LMDSales = Convert.ToInt32(dataDict["LMD"]);
            MTDSales = Convert.ToInt32(dataDict["MTD"]);
            L5Sales = Convert.ToInt32(dataDict["L5"]);
        }

        public string platform { get; set; }
        public int totalRuntime { get; set; }
        public int totalSales { get; set; }
        public int LMDSales { get; set; }
        public int MTDSales { get; set; }
        public int L5Sales { get; set; }
        public string status { get; set; }
        public DateTime lastRuntime { get; set; }

        public Dictionary<string, string> dataDict { get; set; }
    }
}