using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.JobOrderModels
{
    public class mPackageId
    {
        public mPackageId()
        {
            dataDict = new Dictionary<string, string>();
        }

        public mPackageId(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public mPackageId(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            id = Convert.ToInt32(dataDict["id"]);
            name = dataDict["name"];
        }

        public int id { get; set; }
        public string name { get; set; }

        public Dictionary<string, string> dataDict { get; set; }
    }
}