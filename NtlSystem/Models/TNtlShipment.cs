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
    
    public partial class TNtlShipment
    {
        public int id { get; set; }
        public string start_address_line_1 { get; set; }
        public string start_address_line_2 { get; set; }
        public string start_city { get; set; }
        public string start_state { get; set; }
        public Nullable<int> start_zip_code { get; set; }
        public string start_country { get; set; }
        public string dest_address_line_1 { get; set; }
        public string dest_address_line_2 { get; set; }
        public string dest_city { get; set; }
        public string dest_state { get; set; }
        public Nullable<int> dest_zip_code { get; set; }
        public string dest_country { get; set; }
        public string tracking_no { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> expected_date { get; set; }
        public Nullable<System.DateTime> due_date { get; set; }
        public Nullable<int> invoice_id { get; set; }
        public Nullable<int> carrier_id { get; set; }
        public Nullable<int> status_id { get; set; }
        public string remark { get; set; }
        public Nullable<int> detail_id { get; set; }
    }
}