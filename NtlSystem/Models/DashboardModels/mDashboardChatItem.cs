using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.DashboardModels
{
    public class mDashboardChatItem
    {
        public mDashboardChatItem()
        {
            dataDict = new Dictionary<string, string>();
        }

        public string platform { get; set; }
        public int todayRuntime { get; set; }
        public int unread_msg { get; set; }
        public int total_msg { get; set; }
        public int total_customer { get; set; }
        public int opening_ticket { get; set; }
        public string status { get; set; }
        public DateTime lastMessageTime { get; set; }
        public DateTime lastRuntime { get; set; }

        public Dictionary<string, string> dataDict { get; set; }
    }
}