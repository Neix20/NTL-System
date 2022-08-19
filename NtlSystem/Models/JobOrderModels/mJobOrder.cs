using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.JobOrderModels
{
    public class mJobOrder
    {
        public mJobOrder()
        {
            dataDict = new Dictionary<string, string>();
        }

        public mJobOrder(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            id = Convert.ToInt32(dataDict["id"]);
            name = dataDict["name"];
            code = dataDict["code"];
            created_date = Convert.ToDateTime(dataDict["created_date"]);
            client_order_ref = dataDict["client_order_ref"];
            sub_total_price = Convert.ToDecimal(dataDict["sub_total_price"]);
            discount_fee = 0;
            tax_price = Convert.ToDecimal(dataDict["tax_price"]);
            total_price = Convert.ToDecimal(dataDict["total_price"]);

            customer_id = Convert.ToInt32(dataDict["customer_id"]);
            customer_name = dataDict["customer_name"];
            customer_street = dataDict["customer_street"];
            customer_street2 = dataDict["customer_street2"];
            customer_zip_code = Convert.ToInt32(dataDict["customer_zip_code"].Equals("") ? "0" : dataDict["customer_zip_code"]);
            customer_city = dataDict["customer_city"];
            customer_state = dataDict["customer_state"];
            customer_country = dataDict["customer_country"];
        }

        public TNtlJobOrder GenJobOrder()
        {
            TNtlJobOrder item = new TNtlJobOrder();

            item.odoo_sales_id = id;
            item.name = name;
            item.code = code;
            item.created_date = created_date;
            item.client_order_ref = client_order_ref;
            item.sub_total_price = sub_total_price;
            item.discount_fee = discount_fee;
            item.tax_price = tax_price;
            item.total_price = total_price;

            item.customer_id = customer_id;
            item.customer_name = customer_name;
            item.customer_street = customer_street;
            item.customer_street2 = customer_street2;
            item.customer_zip_code = customer_zip_code;
            item.customer_city = customer_city;
            item.customer_state = customer_state;
            item.customer_country = customer_country;

            return item;
        }
        
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string client_order_ref { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> completed_date { get; set; }
        public Nullable<decimal> sub_total_price { get; set; }
        public Nullable<decimal> tax_price { get; set; }
        public Nullable<decimal> discount_fee { get; set; }
        public Nullable<decimal> total_price { get; set; }
        public string status { get; set; }

        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_street { get; set; }
        public string customer_street2 { get; set; }
        public int customer_zip_code { get; set; }
        public string customer_city { get; set; }
        public string customer_state { get; set; }
        public string customer_country { get; set; }

        public Dictionary<string, string> dataDict { get; set; }

        public override string ToString()
        {
            return $"{id} {name} {code} {client_order_ref}";
        }
    }
}