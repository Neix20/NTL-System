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
    
    public partial class TNtlJobOrderItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public string uom { get; set; }
        public Nullable<decimal> unit_price { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> length { get; set; }
        public Nullable<decimal> width { get; set; }
        public Nullable<decimal> height { get; set; }
        public Nullable<decimal> sub_total_price { get; set; }
        public Nullable<decimal> discount_fee { get; set; }
        public Nullable<decimal> tax_price { get; set; }
        public Nullable<decimal> total_price { get; set; }
        public Nullable<int> order_id { get; set; }
        public Nullable<int> detail_id { get; set; }
    }
}
