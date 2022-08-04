using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.OdooManufactureOrderModels
{
    public class mOdooManProduct
    {
        public mOdooManProduct()
        {

        }

        public mOdooManProduct(string sku, decimal? qty)
        {
            this.sku = sku;
            this.qty = qty;
        }

        public string sku { get; set; }
        public decimal? qty { get; set; }

        public override string ToString()
        {
            return $"SKU Code: {sku}\nQuantity: {qty}";
        }
    }
}