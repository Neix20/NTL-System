//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NtlSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TNtlCustomerChat
    {
        public int id { get; set; }
        public string message { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string msg_type { get; set; }
        public string sender_type { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> status_id { get; set; }
        public Nullable<int> platform_id { get; set; }
    }
}