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

namespace NtlSystem.Controllers
{
    public class DashboardController : AdminController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

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

        [ValidateInput(false)]
        public ActionResult dsiGridViewPartial() {

            // Microsoft SQL
            string MicrosoftSqlConn = $"Data Source={mssql_dbHost};Initial Catalog={mssql_dbName};Persist Security Info=True;User ID={mssql_dbUsername};Password={mssql_dbPassword};";
            DAL MicrosoftDal = new MicrosoftSqlDAL(MicrosoftSqlConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            string query = "";
            List<ClsData> ls;

            DateTime today = DateTime.Today;
            DateTime nextMonth = today.AddMonths(1);

            string lmStartDt = GeneralFunc.GetStartDate(today);
            string lmEndDt = GeneralFunc.GetEndDate(today);

            string tmStartDt = GeneralFunc.GetStartDate(nextMonth);
            string tmEndDt = GeneralFunc.GetEndDate(nextMonth);

            string lfStartDt = GeneralFunc.GetLastFiveDays(today);
            string lfEndDt = today.ToString("yyyy-MM-dd");

            query = "SELECT p.name \"Platform\", COUNT(*) \"Sales\", " +
                    "(SELECT COUNT(*) " +
                    "FROM dbo.TNtlOrder  " +
                    "WHERE 1=1 " +
                    $"AND created_date >= '{lmStartDt}' AND created_date <= '{lmEndDt}') \"LMD\", " +
                    "(SELECT COUNT(*) " +
                    "FROM dbo.TNtlOrder  " +
                    "WHERE 1=1 " +
                    $"AND created_date >= '{tmStartDt}' AND created_date <= '{tmEndDt}') \"MTD\", " +
                    "(SELECT COUNT(*) " +
                    "FROM dbo.TNtlOrder " +
                    "WHERE 1=1 " +
                    $"AND created_date >= '{lfStartDt}' AND created_date <= '{lfEndDt}') \"L5\" " +
                    "FROM dbo.TNtlOrder o " +
                    "LEFT JOIN dbo.TNtlCustomer c " +
                    "ON o.customer_id = c.id " +
                    "LEFT JOIN dbo.TNtlPlatform p " +
                    "ON c.platform_id = p.id " +
                    "GROUP BY p.name; ";
            ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

            mDashboardSaleItem item = new mDashboardSaleItem();

            if(ls.Count > 0)
            {
                item.GenSales(ls.ElementAt(0).data);

                query = "SELECT COUNT(*) \"Total Runtime\", " +
                        "(SELECT TOP 1 end_date " +
                        "FROM dbo.TNtlSeleniumLog " +
                        "WHERE 1=1  " +
                        "AND type=2 " +
                        "ORDER BY end_date DESC) \"Last Runtime\" " +
                        "FROM dbo.TNtlSeleniumLog " +
                        "WHERE 1=1 " +
                        "AND type=2; ";
                ls = dao.GetAllClsDataQuery(MicrosoftDal, query);

                Dictionary<string, string> dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(ls.ElementAt(0).data);
                item.totalRuntime = Convert.ToInt32(dataDict["Total Runtime"]);
                item.lastRuntime = Convert.ToDateTime(dataDict["Last Runtime"]);
            }

            List<mDashboardSaleItem> model = new List<mDashboardSaleItem>();
            model.Add(item);
            return PartialView("_dsiGridViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult mdpiGridViewPartial() {
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

            List<mDashboardProductItem> model = new List<mDashboardProductItem>();

            foreach(var obj in ls)
            {
                mDashboardProductItem item = new mDashboardProductItem();
                item.GenProduct(obj.data);
                model.Add(item);
            }

            // Sort By Width
            model = model.OrderBy(x => x.width).ThenBy(x => x.height).ToList();

            return PartialView("_mdpiGridViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult mswiGridViewPartial() {
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

            List<mDashboardOdooStockItem> model = new List<mDashboardOdooStockItem>();

            foreach(var obj in ls)
            {
                mDashboardOdooStockItem item = new mDashboardOdooStockItem();
                item.GenStock(obj.data);
                model.Add(item);
            }

            return PartialView("_mswiGridViewPartial", model);
        }


        [ValidateInput(false)]
        public ActionResult mciGridViewPartial() {
            Random rand = new Random();
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
            return PartialView("_mciGridViewPartial", model);
        }
    }
}
