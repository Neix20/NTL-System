using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.DashboardModels
{
    public class mDashboardOdooStockItem
    {
        public mDashboardOdooStockItem()
        {
            dataDict = new Dictionary<string, string>();
        }

        public mDashboardOdooStockItem(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            model = dataDict["model"];
            description = dataDict["description"];
            sku = dataDict["sku"];
            w50 = Convert.ToDecimal(dataDict["w50"]);
            w60 = Convert.ToDecimal(dataDict["w60"]);
            w70 = Convert.ToDecimal(dataDict["w70"]);
            w80 = Convert.ToDecimal(dataDict["w80"]);
            w90 = Convert.ToDecimal(dataDict["w90"]);
            w150 = Convert.ToDecimal(dataDict["w150"]);
        }

        public string model { get; set; }
        public string description { get; set; }
        public string sku { get; set; }
        public decimal w50 { get; set; }
        public decimal w60 { get; set; }
        public decimal w70 { get; set; }
        public decimal w80 { get; set; }
        public decimal w90 { get; set; }
        public decimal w150 { get; set; }

        public Dictionary<string, string> dataDict { get; set; }
    }
}