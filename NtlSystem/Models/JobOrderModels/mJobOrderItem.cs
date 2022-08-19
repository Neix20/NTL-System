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

        public mJobOrderItem(string JsonData)
        {
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);

            id = Convert.ToInt32(dataDict["id"]);

            name = dataDict["name"];
            sku = dataDict["sku"];

            width = Convert.ToDecimal(dataDict["width"]);
            height = Convert.ToDecimal(dataDict["height"]);
            quantity = Convert.ToDecimal(dataDict["quantity"]);

            uom = dataDict["uom"];

            unit_price = Convert.ToDecimal(dataDict["unit_price"]);
            tax_price = Convert.ToDecimal(dataDict["tax_price"]);
            total_price = Convert.ToDecimal(dataDict["total_price"]);

            order_id = Convert.ToInt32(dataDict["order_id"]);
        }

        public mJobOrderItem(TNtlJobOrderItem item)
        {
            id = item.id;
            name = item.name;
            sku = item.sku;
            width = item.width;
            height = item.height;
            quantity = item.quantity;
            uom = item.uom;
            unit_price = item.unit_price;
            tax_price = item.tax_price;
            total_price = item.total_price;
            order_id = (int) item.order_id;

            length = item.length;
            sub_total_price = item.sub_total_price;
            discount_fee = item.discount_fee;
        }

        public TNtlJobOrderItem GenJobOrderItem()
        {
            TNtlJobOrderItem item = new TNtlJobOrderItem();

            item.id = id;
            item.name = name;
            item.sku = sku;
            item.width = width;
            item.height = height;
            item.quantity = quantity;
            item.uom = uom;
            item.unit_price = unit_price;
            item.tax_price = tax_price;
            item.total_price = total_price;
            item.order_id = order_id;

            item.discount_fee = discount_fee;
            item.sub_total_price = sub_total_price;

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
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> width { get; set; }
        public Nullable<decimal> height { get; set; }
        public Nullable<decimal> unit_price { get; set; }
        public Nullable<decimal> tax_price { get; set; }
        public Nullable<decimal> total_price { get; set; }

        public Nullable<decimal> length { get; set; }
        public Nullable<decimal> discount_fee { get; set; }
        public Nullable<decimal> sub_total_price { get; set; }

        public Dictionary<string, string> dataDict { get; set; }

        public override string ToString()
        {
            return $"{name} {sku} {width} {height} {quantity}";
        }
    }
}
