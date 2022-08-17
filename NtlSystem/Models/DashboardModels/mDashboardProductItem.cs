using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NtlSystem.Models.DashboardModels
{
    public class mDashboardProductItem
    {
        public mDashboardProductItem()
        {
            dataDict = new Dictionary<string, string>();
        }

        public void GenProduct(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            measurement = dataDict["measurement"];
            black_silver = Convert.ToDecimal(dataDict["black_silver"]);
            deep_blue_silver = Convert.ToDecimal(dataDict["deep_blue_silver"]);
            gold_silver = Convert.ToDecimal(dataDict["gold_silver"]);
            green_silver = Convert.ToDecimal(dataDict["green_silver"]);
            silver_light = Convert.ToDecimal(dataDict["silver_light"]);
            price = Convert.ToDecimal(dataDict["price"]);

            string rgx = @"(\d+) x (\d+)";
            width = Convert.ToDecimal(Regex.Replace(measurement, rgx, "$1"));
            height = Convert.ToDecimal(Regex.Replace(measurement, rgx, "$2"));
        }

        public string measurement { get; set; }
        public decimal black_silver { get; set; }
        public decimal deep_blue_silver { get; set; }
        public decimal gold_silver { get; set; }
        public decimal green_silver { get; set; }
        public decimal silver_light { get; set; }
        public decimal price { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }

        public Dictionary<string, string> dataDict { get; set; }
    }
}