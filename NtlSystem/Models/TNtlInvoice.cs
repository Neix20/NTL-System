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
    
    public partial class TNtlInvoice
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string details { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> completed_date { get; set; }
        public Nullable<decimal> shipping_fee { get; set; }
        public string customer_address_line_1 { get; set; }
        public string customer_address_line_2 { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public Nullable<int> customer_zip_code { get; set; }
        public string customer_country { get; set; }
        public Nullable<int> payment_method_id { get; set; }
        public Nullable<int> status_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> order_id { get; set; }
        public string remark { get; set; }
        public Nullable<int> detail_id { get; set; }
    }
}
