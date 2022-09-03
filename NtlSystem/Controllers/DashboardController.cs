using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NtlSystem.Models;
using NtlSystem.Models.SqlModels;
using NtlSystem.Models.SqlModels.Service.Impl;
using System.Diagnostics;
using NtlSystem.Models.DashboardModels;
using Newtonsoft.Json;
using Bogus;

namespace NtlSystem.Controllers
{
    public class DashboardController : Controller
    {
        // Global Variables
        string psql_dbHost = "localhost";
        string psql_dbName = "odoo_new";
        string psql_dbUsername = "postgres";
        string psql_dbPassword = "postgres";

        string mssql_dbHost = "47.254.234.86";
        string mssql_dbName = "NTL";
        string mssql_dbUsername = "ntl";
        string mssql_dbPassword = "ILoveVigtech88!";

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order()
        {
            Random rand = new Random();

            int numOfSale = db.TNtlOrders.ToList().Count;
            ViewData["numOfSale"] = numOfSale;

            double totalRevenue = Convert.ToDouble(db.TNtlOrders.Select(it => it.total_price).Sum());
            ViewData["totalRevenue"] = totalRevenue;

            double dailyRevenue = rand.Next(500, 1000) * rand.NextDouble();
            ViewData["dailyRevenue"] = dailyRevenue;

            double monthlyRevenue = rand.Next(500, 1000) * rand.NextDouble();
            ViewData["monthlyRevenue"] = monthlyRevenue;

            return View();
        }

        public ActionResult Chat()
        {
            Random rand = new Random();

            // Microsoft SQL
            string MicrosoftSqlConn = $"Data Source={mssql_dbHost};Initial Catalog={mssql_dbName};Persist Security Info=True;User ID={mssql_dbUsername};Password={mssql_dbPassword};";
            DAL MicrosoftDal = new MicrosoftSqlDAL(MicrosoftSqlConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            string query = "";
            List<ClsData> ls;

            query = "SELECT COUNT(DISTINCT customer_id) AS total_visitor FROM dbo.TNtlCustomerChat;";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            int totalVisitor = Convert.ToInt32(ls.ElementAt(0).dataDict["total_visitor"]);
            ViewData["totalVisitor"] = totalVisitor;

            query = "SELECT COUNT(*) AS total_chat FROM dbo.TNtlCustomerChat;";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            int totalChat = Convert.ToInt32(ls.ElementAt(0).dataDict["total_chat"]);
            ViewData["totalChat"] = totalChat;

            int status_id = DbStoredProcedure.GetStatusID("Unsend", "Platform Message");

            query = $"SELECT COUNT(*) AS unsend_msg FROM dbo.TNtlCustomerChat WHERE status_id = {status_id};";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            int totalUnsendMsg = Convert.ToInt32(ls.ElementAt(0).dataDict["unsend_msg"]);
            ViewData["totalUnsendMsg"] = totalUnsendMsg;

            int totalOpeningTicket = rand.Next(1, 100);
            ViewData["totalOpeningTicket"] = totalOpeningTicket;

            return View();
        }

        public ActionResult ShopeeProductListing()
        {
            return View();
        }

        public ActionResult OdooStockListing()
        {
            return View();
        }

        public JsonResult DownTimeChatChart()
        {
            string query = "";
            List<ClsData> ls;
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            // Microsoft SQL
            string MicrosoftSqlConn = $"Data Source={mssql_dbHost};Initial Catalog={mssql_dbName};Persist Security Info=True;User ID={mssql_dbUsername};Password={mssql_dbPassword};";
            DAL MicrosoftDal = new MicrosoftSqlDAL(MicrosoftSqlConn);

            query = "SELECT datepart(hour,start_date) as X , sum(downtime) AS Y " +
                "FROM [NTL].[dbo].[TNTLSeleniumChatDownTime]	" +
                "WHERE cast(start_date as date) = '2022-08-24' " +
                "GROUP BY cast(start_date as date)  , datepart(hour,start_date) " +
                "ORDER BY cast(start_date as date)  , datepart(hour,start_date)";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            List<mChartModel> coordsList = ls.Select(it => new mChartModel(it.dataDict["X"], it.dataDict["Y"])).ToList();
            Dictionary<string, string> tmpDict = new Dictionary<string, string>()
            {
                {"coords", JsonConvert.SerializeObject(coordsList) }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(JsonConvert.SerializeObject(tmpDict), JsonRequestBehavior.AllowGet);
        }

        public List<mChartModel> CummulativeSalesList(DateTime startDt, string platformName)
        {
            string query = "";
            List<ClsData> ls;
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            // Microsoft SQL
            string MicrosoftSqlConn = $"Data Source={mssql_dbHost};Initial Catalog={mssql_dbName};Persist Security Info=True;User ID={mssql_dbUsername};Password={mssql_dbPassword};";
            DAL MicrosoftDal = new MicrosoftSqlDAL(MicrosoftSqlConn);

            DateTime endDt = startDt.AddDays(1);

            query = "SELECT DATEPART(hour, created_date) as H,  " +
                "ROUND(SUM(total_price), 2) AS total_sales " +
                "FROM dbo.TNtlOrder o " +
                "LEFT JOIN dbo.TNtlCustomer c " +
                "ON o.customer_id = c.id " +
                "LEFT JOIN dbo.TNtlPlatform p " +
                "ON c.platform_id = p.id " +
                "WHERE 1 = 1 " +
                $"AND (o.created_date >= '{startDt.ToString("yyyy-MM-dd")}' AND o.created_date < '{endDt.ToString("yyyy-MM-dd")}') " +
                $"AND p.name = '{platformName}' " +
                "GROUP BY DATEPART(HOUR, created_date); ";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            Dictionary<string, decimal> timeDict = new Dictionary<string, decimal>();

            ls.ForEach(it =>
            {
                string hr = it.dataDict["H"];
                decimal total_sales = Convert.ToDecimal(it.dataDict["total_sales"]);

                timeDict.Add(hr, total_sales);
            });

            List<mChartModel> coordsList = new List<mChartModel>();
            mChartModel coords;

            decimal cur_sales = 0;

            // 24 Hours
            for (int i = 0; i < 24; i++)
            {
                decimal sales = 0;

                if(timeDict.ContainsKey(i + "")) { sales = timeDict[i + ""]; }

                cur_sales += sales;

                coords = new mChartModel(GeneralFunc.FormatNum(i), string.Format("{0:0.00}", cur_sales));

                coordsList.Add(coords);
            }

            return coordsList;
        }

        [HttpPost]
        public JsonResult CummulativeSalesChart(DateTime? start_date, string platform_name, string past_day)
        {
            Debug.WriteLine($"{start_date}, {platform_name}, {past_day}");
            DateTime startDt = (start_date == null) ? DateTime.Today : (DateTime) start_date;
            List<mChartModel> current = CummulativeSalesList(startDt, platform_name);

            int pastDay = Convert.ToInt32(GeneralFunc.ReturnZero(past_day));
            DateTime pastDt = startDt.AddDays(-1 * pastDay);

            List<mChartModel> past = CummulativeSalesList(pastDt, platform_name);

            Dictionary<string, string> tmpDict = new Dictionary<string, string>()
            {
                {"Current", JsonConvert.SerializeObject(current) },
                {$"Past (+{past_day})", JsonConvert.SerializeObject(past) }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(JsonConvert.SerializeObject(tmpDict), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CummulativeSales()
        {
            return View();
        }

        public JsonResult PopulateDummyChatModels()
        {
            Random rand = new Random();

            // Create Dummy Chat Items
            int platform_id = DbStoredProcedure.GetPlatformID("Shopee");
            int customer_id = DbStoredProcedure.GetCustomerId("Matthew Ting");

            DateTime startDt = GeneralFunc.ConvertStrToDateTime("2022-08-21");
            DateTime endDt = GeneralFunc.ConvertStrToDateTime("2022-08-22");

            int chatCount = rand.Next(50, 101);
            for (int i = 0; i < chatCount; i++)
            {
                TNtlCustomerChat chat = new Faker<TNtlCustomerChat>()
                    .RuleFor(t => t.message, f => f.Lorem.Sentence())
                    .RuleFor(t => t.created_date, f => f.Date.Between(startDt, endDt))
                    .RuleFor(t => t.sender_type, f => f.PickRandom(new[] { "client", "sender" }));

                chat.msg_type = "Text";
                chat.customer_id = customer_id;
                chat.status_id = rand.Next(7, 11);
                chat.user_id = rand.Next(8, 12);
                chat.platform_id = platform_id;

                DbStoredProcedure.CustomerChatInsert(chat);
            }

            Response.StatusCode = 200;
            return Json("Successfully populate Chat Models", JsonRequestBehavior.AllowGet);
        }

        public JsonResult TopPlatformChatChart()
        {
            List<mChartModel> coordsList = new List<mChartModel>();
            mChartModel coords;

            List<int> platformIds = db.TNtlCustomerChats.Select(it => (int)it.platform_id).Distinct().ToList();

            foreach (var platform_id in platformIds)
            {
                int totalMsg = db.TNtlCustomerChats.Where(it => it.platform_id == platform_id).ToList().Count;
                TNtlPlatform platform = db.TNtlPlatforms.Where(it => it.id == platform_id).FirstOrDefault();

                coords = new mChartModel(platform.name, totalMsg + "");
                coordsList.Add(coords);
            }

            Dictionary<string, string> tmpDict = new Dictionary<string, string>()
            {
                {"coords", JsonConvert.SerializeObject(coordsList) }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(JsonConvert.SerializeObject(tmpDict), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RealTimeChatChart()
        {
            List<mChartModel> coordsList = new List<mChartModel>();
            mChartModel coords;

            DateTime endDt = GeneralFunc.ConvertStrToDateTime("2022-08-22");

            // 24 Hours
            for (int i = 0; i < 24; i++)
            {
                DateTime curDate = endDt.AddHours(-1 * i);

                int chatCount = 0;

                foreach (var chat in db.TNtlCustomerChats)
                {
                    double hour = ((DateTime)chat.created_date - curDate).TotalHours;
                    hour = Math.Floor(Math.Abs(hour));
                    chatCount += (hour == 0) ? 1 : 0;
                }

                coords = new mChartModel(GeneralFunc.FormatNum(i), chatCount + "");

                coordsList.Add(coords);
            }

            Dictionary<string, string> tmpDict = new Dictionary<string, string>()
            {
                {"coords", JsonConvert.SerializeObject(coordsList) }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(JsonConvert.SerializeObject(tmpDict), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PopulateDummyOrderModels()
        {
            // List of Product SKU
            List<string> productSkus = new List<string>() { "FGSTSB060", "FGSTSB150030", "FGSTSL050", "FGSTSL050110" };
            List<int> productIds = productSkus.Select(it => DbStoredProcedure.GetProductID(it)).ToList();

            Random rand = new Random();

            List<decimal> productPrice = productSkus.Select(it => Convert.ToDecimal(rand.Next(1, 7))).ToList();

            DateTime startDt = GeneralFunc.ConvertStrToDateTime("2022-08-26");
            DateTime endDt = GeneralFunc.ConvertStrToDateTime("2022-08-29");

            List<int> customerIds = new List<int>() { 100, 102, 103 };

            for (int i = 1; i < 61; i++)
            {
                TNtlOrder order = new Faker<TNtlOrder>()
                    .RuleFor(t => t.name, f => $"O{f.Lorem.Letter(6)}")
                    .RuleFor(t => t.created_date, f => f.Date.Between(startDt, endDt));

                order.code = order.name;
                order.status_id = rand.Next(13, 17);
                order.customer_id = customerIds[rand.Next(0, 3)];

                DbStoredProcedure.OrderInsert(order, "Admin");

                order.id = DbStoredProcedure.GetID("TNtlOrder");

                for (int j = 0; j < productIds.Count; j++)
                {
                    int random = rand.Next(1, 3);

                    if (j > 0 && random == 1) continue;

                    TNtlOrderItem orderItem = new Faker<TNtlOrderItem>()
                        .RuleFor(t => t.quantity, f => f.Random.Decimal());

                    int product_id = productIds[j];
                    orderItem.unit_price = productPrice[j];

                    TNtlProduct randProduct = db.TNtlProducts.Where(it => it.id == product_id).FirstOrDefault();

                    orderItem.product_id = randProduct.id;
                    orderItem.uom_id = randProduct.uom_id;
                    orderItem.name = randProduct.name;
                    orderItem.sku = randProduct.SKU;

                    orderItem.sub_total_price = orderItem.unit_price * orderItem.quantity;
                    orderItem.total_price = orderItem.sub_total_price;

                    orderItem.order_id = order.id;

                    DbStoredProcedure.OrderItemInsert(orderItem, "Admin");
                }

                // Update Total Price
                decimal total_price = (decimal) db.TNtlOrderItems.Where(it => it.order_id == order.id).Select(it => it.total_price).Sum();
                order.sub_total_price = total_price;
                order.total_price = total_price;

                DbStoredProcedure.OrderUpdate(order, "Admin");
            }

            return Json("Successfully Populated Dummy Order Models!", JsonRequestBehavior.AllowGet);
        }

        public JsonResult TopProductChart()
        {
            List<mChartModel> coordsList = new List<mChartModel>();
            mChartModel coords;

            List<int> productIds = db.TNtlOrderItems.Select(it => (int)it.product_id).Distinct().ToList();

            foreach (var productId in productIds)
            {
                decimal totalRevenue = (decimal)db.TNtlOrderItems.Where(it => it.product_id == productId).Select(it => it.total_price).Sum();

                TNtlProduct product = db.TNtlProducts.Where(it => it.id == productId).FirstOrDefault();

                string name = product.name;

                coords = new mChartModel(name, totalRevenue + "");
                coordsList.Add(coords);
            }

            Dictionary<string, string> tmpDict = new Dictionary<string, string>()
            {
                {"coords", JsonConvert.SerializeObject(coordsList) }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(JsonConvert.SerializeObject(tmpDict), JsonRequestBehavior.AllowGet);
        }

        public JsonResult RealTimeSalesChart()
        {
            List<mChartModel> coordsList = new List<mChartModel>();
            mChartModel coords;

            DateTime endDt = GeneralFunc.ConvertStrToDateTime("2022-08-22");

            // 24 Hours
            for (int i = 0; i < 24; i++)
            {
                DateTime curDate = endDt.AddHours(-1 * i);

                int orderCount = 0;

                foreach (var order in db.TNtlOrders)
                {
                    double hour = ((DateTime)order.created_date - curDate).TotalHours;
                    hour = Math.Floor(Math.Abs(hour));
                    orderCount += (hour == 0) ? 1 : 0;
                }

                coords = new mChartModel(GeneralFunc.FormatNum(i), orderCount + "");

                coordsList.Add(coords);
            }

            Dictionary<string, string> tmpDict = new Dictionary<string, string>()
            {
                {"coords", JsonConvert.SerializeObject(coordsList) }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            return Json(JsonConvert.SerializeObject(tmpDict), JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderSalesTablePartial()
        {
            List<TNtlOrder> model = db.TNtlOrders.OrderByDescending(it => it.total_price).ToList();

            if (model.Count > 5)
            {
                model = model.GetRange(0, 5);
            }

            return PartialView("_dostPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult dsiPartial()
        {
            string query = "";
            List<ClsData> ls;
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            // Microsoft SQL
            string MicrosoftSqlConn = $"Data Source={mssql_dbHost};Initial Catalog={mssql_dbName};Persist Security Info=True;User ID={mssql_dbUsername};Password={mssql_dbPassword};";
            DAL MicrosoftDal = new MicrosoftSqlDAL(MicrosoftSqlConn);

            DateTime today = DateTime.Today;
            DateTime nextMonth = today.AddMonths(1);

            string tdStartDt = $"{today.ToString("yyyy-MM-dd")} 00:00:00";
            string tdEndDt = $"{today.ToString("yyyy-MM-dd")} 23:59:59";

            string lmStartDt = GeneralFunc.GetStartDate(today);
            string lmEndDt = GeneralFunc.GetEndDate(today);

            string tmStartDt = GeneralFunc.GetStartDate(nextMonth);
            string tmEndDt = GeneralFunc.GetEndDate(nextMonth);

            string l5StartDt = GeneralFunc.GetLastFiveDays(today);
            string l5EndDt = today.ToString("yyyy-MM-dd");

            query = "EXEC dbo.NSP_TNtlOrderAutomation_View " +
                    $"'{tdStartDt}', '{tdEndDt}', " +
                    $"'{lmStartDt}', '{lmEndDt}', " +
                    $"'{tmStartDt}', '{tmEndDt}', " +
                    $"'{l5StartDt}', '{l5EndDt}'; ";
            Debug.WriteLine(query);
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            mDashboardSaleItem item = new mDashboardSaleItem(ls.ElementAt(0).data);

            // Create Postgres Connection and Get List of Products
            string PostGreSQLConn = $"Host={psql_dbHost};Port=5432;Username={psql_dbUsername};Password={psql_dbPassword};Database={psql_dbName}";
            DAL PostgresDal = new PostgreSqlDAL(PostGreSQLConn);

            query = "SELECT COUNT(*) AS odoo_sales " +
                    "FROM public.sale_order " +
                    "WHERE 1 = 1 " +
                    "AND create_date >= '2022-08-19 00:00:00' AND create_date < '2022-08-19 23:59:59'; ";
            ls = dao.GetAllClsDataQuery(PostgresDal, query);

            Dictionary<string, string> dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(ls.ElementAt(0).data);
            item.odooSales = Convert.ToInt32(dataDict["odoo_sales"]);

            List<mDashboardSaleItem> model = new List<mDashboardSaleItem>();
            model.Add(item);
            return PartialView("_dsiPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult mdpiPartial()
        {
            // SQL
            int total_capacity = Convert.ToInt32(db.TNtlSummaryItems.Select(it => it.quantity).Sum());
            total_capacity = (total_capacity == 0) ? 1 : total_capacity;

            int total_available = total_capacity - Convert.ToInt32(db.TNtlSummaryItems.Select(it => it.used).Sum());
            double percentage = total_available * 100.0 / total_capacity;

            // Card Data
            ViewData["total_capacity"] = total_capacity;
            ViewData["total_available"] = total_available;
            ViewData["percentage"] = percentage;

            // Microsoft SQL
            string MicrosoftSqlConn = $"Data Source={mssql_dbHost};Initial Catalog={mssql_dbName};Persist Security Info=True;User ID={mssql_dbUsername};Password={mssql_dbPassword};";
            DAL MicrosoftDal = new MicrosoftSqlDAL(MicrosoftSqlConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            string query = "";
            List<ClsData> ls;

            query = "SELECT " +
                    "	tab1.measurement AS measurement, " +
                    "	SUM(case when tab1.name = 'Black Silver' then tab1.quantity else 0 end) AS black_silver, " +
                    "	SUM(case when tab1.name = 'Deep Blue Silver' then tab1.quantity else 0 end) AS deep_blue_silver, " +
                    "	SUM(case when tab1.name = 'Gold Silver' then tab1.quantity else 0 end) AS gold_silver, " +
                    "	SUM(case when tab1.name = 'Green Silver' then tab1.quantity else 0 end) AS green_silver, " +
                    "	SUM(case when tab1.name = 'Silver Light' then tab1.quantity else 0 end) AS silver_light, " +
                    "	tab1.price AS price " +
                    "FROM ( " +
                    "	SELECT name, CONCAT(width, ' x ', length) AS measurement, quantity, price " +
                    "	FROM VNtlDashboardProduct " +
                    ") tab1 " +
                    "GROUP BY tab1.measurement, tab1.price; ";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            // List<mDashboardProductItem> model = ls.Select(it => new mDashboardProductItem(it.data)).ToList();
            List<mDashboardProductItem> model = new List<mDashboardProductItem>();

            foreach (var it in ls)
            {
                mDashboardProductItem obj = new mDashboardProductItem(it.data);

                obj.used = 0;
                obj.quantity = 0;
                obj.percentage = 0;

                foreach (var sItem in db.TNtlSummaryItems)
                {
                    string key = $"{Convert.ToInt32(sItem.width)} x {Convert.ToInt32(sItem.height)}";

                    if (key.Equals(obj.measurement))
                    {
                        obj.used = sItem.used;
                        obj.quantity = sItem.quantity;
                        obj.percentage = Convert.ToDecimal(((double)obj.quantity - (double)obj.used) * 100.0 / (double)obj.quantity);
                        break;
                    }
                }

                model.Add(obj);
            }

            // Sort By Width
            model = model.OrderBy(x => x.width).ThenBy(x => x.height).ToList();
            return PartialView("_mdpiPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult mswiPartial()
        {
            // Create Postgres Connection and Get List of Products
            string PostGreSQLConn = $"Host={psql_dbHost};Port=5432;Username={psql_dbUsername};Password={psql_dbPassword};Database={psql_dbName}";
            DAL PostgresDal = new PostgreSqlDAL(PostGreSQLConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            string query = "";
            List<ClsData> ls;

            query = "SELECT a.model, a.description, b.* " +
                    "FROM VWarehouseStockDesc a " +
                    "LEFT JOIN VWarehouseStockCT b " +
                    "ON a.sku = b.sku; ";
            ls = dao.GetAllClsDataQuery(PostgresDal, query);

            List<mDashboardOdooStockItem> model = ls.Select(
                it => new mDashboardOdooStockItem(it.data)
            ).ToList();

            return PartialView("_mswiPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult mciPartial()
        {
            mDashboardChatItem item = new mDashboardChatItem();
            item.platform = "Shopee";
            item.todayRuntime = 2;
            item.unread_msg = 0;
            item.total_msg = 4;
            item.total_customer = 4;
            item.opening_ticket = 0;
            item.status = "Online";
            item.lastMessageTime = DateTime.Today;
            item.lastRuntime = DateTime.Today;

            List<mDashboardChatItem> model = new List<mDashboardChatItem>();
            model.Add(item);
            return PartialView("_mciPartial", model);
        }
    }
}
