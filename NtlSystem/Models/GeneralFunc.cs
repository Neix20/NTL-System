using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NtlSystem.Models
{
    public class GeneralFunc
    {
        public static string TrimStr(string str)
        {
            return (str == null) ? "" : str.Trim('"');
        }

        public static string[] FormatAddress(string address)
        {
            address = TrimStr(address);

            string[] tmp_address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string[] address_arr = (tmp_address_arr.Length != 6) ?
                "address line 1, address line 2, city, 00000, state, country".Split(new[] { ", " }, StringSplitOptions.None) :
                address.Split(new[] { ", " }, StringSplitOptions.None);

            // Validate Zip Code
            string zipCode_pattern = @"^\d{5}$", zipCode_str = address_arr[3];
            var flag = Regex.Match(zipCode_str, zipCode_pattern, RegexOptions.IgnoreCase).Success;
            address_arr[3] = (flag) ? address_arr[3] : "00000";

            return address_arr;
        }

        public static string ParseZero(Decimal? num)
        {
            return (num == 0) ? "" : num.ToString();
        }

        public static TNtlSummaryItem GenerateSummaryListing(string JsonData)
        {
            var item = new TNtlSummaryItem();

            Dictionary<string, string> dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData);
            item.id = Convert.ToInt32(dataDict["id"]);

            string itemDescription = dataDict["item_description"];

            if (itemDescription.Contains("["))
            {
                string rgx = @"\[(.*)\] (([A-Za-z -]*)((\d+)cm)?)(\|(\d+)cm)?(\|(\d+))?";
                item.sku = Regex.Replace(itemDescription, rgx, "$1");
                item.name = Regex.Replace(itemDescription, rgx, "$2");
                item.width = Convert.ToDecimal(Regex.Replace(itemDescription, rgx, "$5"));
                item.height = Convert.ToDecimal(Regex.Replace(itemDescription, rgx, "$7"));
                item.quantity = Convert.ToDecimal(Regex.Replace(itemDescription, rgx, "$9"));
            }

            int inc_sta_id = DbStoredProcedure.GetStatusID("incomplete", "Summary Item");
            item.status_id = inc_sta_id;

            item.used = 0;

            item.created_date = DateTime.Today;

            return item;
        }

        public static string GenSummaryItemKey(TNtlSummaryItem item) {
            string sku = item.sku;
            string height = Convert.ToInt32(item.height).ToString();
            string width = Convert.ToInt32(item.width).ToString();
            return $"{sku}-{height}-{width}";
        }

        public static string GenJobOrderItemKey(TNtlJobOrderItem item) {
            string sku = item.sku;
            string height = Convert.ToInt32(item.height).ToString();
            string width = Convert.ToInt32(item.width).ToString();
            return $"{sku}-{height}-{width}";
        }

        public static DateTime ConvertStrToDateTime(string dtStr)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(dtStr, "yyyy-MM-dd", provider);
        }

        public static string FormatNum(int n)
        {
            return (n < 10) ? $"0{n}" : $"{n}";
        }

        public static string GetStartDate(DateTime dt)
        {
            DateTime _dt = new DateTime(dt.Year, dt.Month, 1);
            _dt = _dt.AddMonths(-1);
            return $"{_dt.Year}-{FormatNum(_dt.Month)}-{FormatNum(_dt.Day)}";
        }

        public static string GetEndDate(DateTime dt)
        {
            DateTime _dt = new DateTime(dt.Year, dt.Month, 1);
            _dt = _dt.AddDays(-1);
            return $"{_dt.Year}-{FormatNum(_dt.Month)}-{FormatNum(_dt.Day)}";
        }

        public static string GetLastFiveDays(DateTime dt)
        {
            DateTime _dt = dt.AddDays(-5);
            return $"{_dt.Year}-{FormatNum(_dt.Month)}-{FormatNum(_dt.Day)}";
        }
	    
	    public static bool DateInRange(DateTime startDate, DateTime endDate, DateTime date){
	    	return startDate <= date && date < endDate;
	    }

    }
}
