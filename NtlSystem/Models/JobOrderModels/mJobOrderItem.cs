using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text.RegularExpressions;

namespace NtlSystem.Models.JobOrderModels
{
    public class mJobOrderItem
    {
        public mJobOrderItem()
        {
            dataDict = new Dictionary<string, string>();
        }

        public mJobOrderItem(TNtlJobOrderItem item)
        {
            id = item.id;
            order_id = (int) item.order_id;
            name = item.name;
            sku = item.sku;
            uom = item.uom;
            unit_price = item.unit_price;
            quantity = item.quantity;
            length = item.length;
            width = item.width;
            height = item.height;
            sub_total_price = item.sub_total_price;
            tax_price = item.tax_price;
            discount_fee = item.discount_fee;
            total_price = item.total_price;
        }

        public void SummaryListing(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);
            id = Convert.ToInt32(dataDict["id"]);

            string itemDescription = dataDict["item_description"];

            if (itemDescription.Contains("|"))
            {
                string rgx = @"\[(.*)\] (([A-Za-z -]*)((\d+)cm)?)(\|(\d+)cm)?(\|(\d+))?";
                sku = Regex.Replace(itemDescription, rgx, "$1");
                name = Regex.Replace(itemDescription, rgx, "$2");
                width = Convert.ToDecimal(Regex.Replace(itemDescription, rgx, "$5"));
                height = Convert.ToDecimal(Regex.Replace(itemDescription, rgx, "$7"));
                quantity = Convert.ToDecimal(Regex.Replace(itemDescription, rgx, "$9"));
            }
        }



        public void PackageListing(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            id = Convert.ToInt32(dataDict["id"]);
            order_id = Convert.ToInt32(dataDict["order_id"]);

            string itemDescription = dataDict["item_description"];

            // Check if Item Description contains "["
            if (itemDescription.Contains("["))
            {
                string rgx = @"\[(.*)\] (([A-Za-z -]*)((\d+)cm)?)(\|(\d+)cm)?(\|(\d+))?";
                sku = Regex.Replace(itemDescription, rgx, "$1");
                name = Regex.Replace(itemDescription, rgx, "$2");

                uom = dataDict["uom"];

                quantity = Convert.ToDecimal(dataDict["quantity"]);
                unit_price = Convert.ToDecimal(dataDict["unit_price"]);
                tax_price = Convert.ToDecimal(dataDict["tax_price"]);
                total_price = Convert.ToDecimal(dataDict["total_price"]);
            }

            item_description = dataDict["item_description"];
        }

        public TNtlJobOrderItem GenJobOrderItem()
        {
            TNtlJobOrderItem item = new TNtlJobOrderItem();

            item.order_id = order_id;
            item.name = name;
            item.sku = sku;
            item.uom = uom;
            item.unit_price = unit_price;
            item.quantity = quantity;
            item.width = width;
            item.height = height;
            item.sub_total_price = sub_total_price;
            item.tax_price = tax_price;
            item.discount_fee = discount_fee;
            item.total_price = total_price;

            return item;
        }

        public string GenUnique()
        {
            return $"{sku}-{height.ToString()}-{width.ToString()}";
        }

        public int id { get; set; }
        public int order_id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public string uom { get; set; }
        public Nullable<decimal> unit_price { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> length { get; set; }
        public Nullable<decimal> width { get; set; }
        public Nullable<decimal> height { get; set; }
        public Nullable<decimal> sub_total_price { get; set; }
        public Nullable<decimal> tax_price { get; set; }
        public Nullable<decimal> discount_fee { get; set; }
        public Nullable<decimal> total_price { get; set; }

        public string item_description { get; set; }

        public Dictionary<string, string> dataDict { get; set; }

        public override string ToString()
        {
            return $"{name} {sku} {width} {height} {quantity}";
        }
    }
}