using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models
{
    public class mChartModel
    {
        public mChartModel()
        {
            x = "";
            y = "";
        }

        public mChartModel(string x, string y)
        {
            this.x = x;
            this.y = y;
        }

        public string x { get; set; }
        public string y { get; set; }
    }
}