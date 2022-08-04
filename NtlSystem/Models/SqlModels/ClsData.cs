using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace NtlSystem.Models.SqlModels
{
    class ClsData
    {
        public ClsData(string data)
        {
            this.data = data;
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(this.data);
            dataList = new List<string>();

            // Populate List
            foreach(var kvp in dataDict)
            {
                dataList.Add(kvp.Value);
            }

            propCount = dataList.Count;
        }

        public ClsData()
        {

        }

        public string data{ get; set; }
        public Dictionary<string, string> dataDict { get; set; }

        public List<string> dataList { get; set; }

        public int propCount { get; set; }

        public override string ToString()
        {
            return string.Join(", ", dataList);
        }

    }
}
