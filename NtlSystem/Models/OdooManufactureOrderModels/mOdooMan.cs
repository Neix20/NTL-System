using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models.OdooManufactureOrderModels
{
    public class mOdooMan
    {
        public mOdooMan()
        {

        }

        public mOdooMan(string stock_name, string company_name, List<mOdooManProduct> product)
        {
            this.stock_name = stock_name;
            this.company_name = company_name;
            this.product = product;
        }

        public string stock_name { get; set; }
        public string company_name { get; set; }
        public List<mOdooManProduct> product { get; set; }
    }
}