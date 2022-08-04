using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.JobOrderModels
{
    public class mPackageListing
    {
        public mJobOrder order { get; set; }
        public List<mJobOrderItem> itemList { get; set; }
    }
}