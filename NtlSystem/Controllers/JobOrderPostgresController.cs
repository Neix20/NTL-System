using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NtlSystem.Models;
using NtlSystem.Models.JobOrderModels;
using NtlSystem.Models.SqlModels;
using NtlSystem.Models.SqlModels.Service.Impl;
using System.Diagnostics;
using NtlSystem.Models.OdooManufactureOrderModels;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace NtlSystem.Controllers
{
    public class JobOrderPostgresController : AdminController
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: JobOrderPostgres
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [HttpPost]
        public ActionResult SummaryListingPartial()
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            //string dbHost = "47.254.232.38";
            string dbHost = "localhost";
            string dbName = "odoo_new";
            string dbUsername = "txe1";
            string dbPassword = "arf11234";

            // Create Postgres Connection and Get List of Products
            string PostGreSQLConn = $"Host={dbHost};Port=5432;Username={dbUsername};Password={dbPassword};Database={dbName}";
            DAL PostgresDal = new PostgreSqlDAL(PostGreSQLConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            string query = "";
            List<ClsData> ls;

            int jb_inc_sta_id = DbStoredProcedure.GetStatusID("incomplete", "Job Batch");
            int jo_inc_sta_id = DbStoredProcedure.GetStatusID("incomplete", "Job Order");

            // Get List of Summary Item that are incomplete
            int si_inc_sta_id = DbStoredProcedure.GetStatusID("incomplete", "Summary Item");
            List<TNtlSummaryItem> summaryItemList = db.TNtlSummaryItems.Where(it => it.status_id == si_inc_sta_id).ToList();

            int batch_id = DbStoredProcedure.GetID("TNtlJobBatch");
            var JobBatchModel = db.TNtlJobBatches.Find(batch_id);

            if (JobBatchModel != null && JobBatchModel.status_id == jb_inc_sta_id)
            {

            }
            else
            {
                // If there no incomplete Job Batch
                query = "SELECT * FROM VNtlJobOrderItem WHERE item_description LIKE '%|%';";
                ls = dao.GetAllClsDataQuery(PostgresDal, query);

                // Generate List of mJobOrder item List
                var tmpItemList = ls.Select(obj => GeneralFunc.GenerateSummaryListing(obj.data)).ToList();
                summaryItemList.AddRange(tmpItemList);

                // Add Job Batch
                JobBatchModel = new TNtlJobBatch();

                JobBatchModel.name = $"Job Batch #{batch_id + 1}";
                JobBatchModel.created_date = DateTime.Now;
                JobBatchModel.staff_name = username;
                JobBatchModel.status_id = jb_inc_sta_id;

                DbStoredProcedure.JobBatchInsert(JobBatchModel, username);

                // Update Batch ID
                batch_id = DbStoredProcedure.GetID("TNtlJobBatch");

                // Add List of Job Orders
                query = $"SELECT * FROM VNtlJobOrder;";
                ls = dao.GetAllClsDataQuery(PostgresDal, query);

                foreach (var obj in ls)
                {
                    var item = (new mJobOrder(obj.data)).GenJobOrder();
                    item.batch_id = batch_id;
                    item.status_id = jo_inc_sta_id;
                    DbStoredProcedure.JobOrderInsert(item, username);
                }

                // Add List of Job Order Items
                query = $"SELECT * FROM VNtlJobOrderItem;";
                ls = dao.GetAllClsDataQuery(PostgresDal, query);

                for (int i = 0; i < ls.Count; i += 2)
                {
                    mJobOrderItem obj = new mJobOrderItem();
                    obj.PackageListing(ls.ElementAt(i).data);
                    if (i + 1 < ls.Count) obj.SummaryListing(ls.ElementAt(i + 1).data);
                    var item = obj.GenJobOrderItem();
                    DbStoredProcedure.JobOrderItemInsert(item, username);
                }

                Dictionary<string, TNtlSummaryItem> summaryItemDict = new Dictionary<string, TNtlSummaryItem>();

                // Fix Duplicate
                foreach (var obj in summaryItemList)
                {
                    string key = GeneralFunc.GenSummaryItemKey(obj);

                    if (summaryItemDict.ContainsKey(key))
                    {
                        summaryItemDict[key].quantity += obj.quantity;
                    }
                    else
                    {
                        summaryItemDict[key] = obj;
                    }
                }

                summaryItemList = new List<TNtlSummaryItem>();
                foreach (var kvp in summaryItemDict)
                {
                    summaryItemList.Add(kvp.Value);
                }

                // Create Dictionary of Summary Item With Unique Key
                summaryItemDict = new Dictionary<string, TNtlSummaryItem>();


                db.TNtlSummaryItems.ToList().ForEach(it =>
                {
                    string key = GeneralFunc.GenSummaryItemKey(it);
                    Debug.WriteLine(key);
                    summaryItemDict[key] = it;
                });

                summaryItemList.ForEach(item =>
                {
                    string key = GeneralFunc.GenSummaryItemKey(item);
                    if (summaryItemDict.ContainsKey(key))
                    {
                        // Update Quantity
                        var obj = summaryItemDict[key];
                        obj.quantity = item.quantity;
                        DbStoredProcedure.SummaryItemUpdate(obj, username);

                        item = obj;
                    }
                    else
                    {
                        // Create New Item
                        DbStoredProcedure.SummaryItemInsert(item, username);
                    }

                });
            }

            // // Create Manufacturing Orders
            // mOdooMan model = new mOdooMan();

            // // Set Stock Name
            // model.stock_name = "Stock";

            // // Set Company Name
            // model.company_name = "YourCompany";

            // List<mOdooManProduct> product = summaryItemList
            //     .Where(it => it.status_id == si_inc_sta_id)
            //     .GroupBy(it => it.sku)
            //     .Select(it => new mOdooManProduct()
            //     {
            //         sku = it.First().sku,
            //         qty = it.Sum(x => (x.quantity * x.width * x.height / 100 / 100))
            //     }).ToList();

            // // Remove Product Code 'FGSTSB150'
            // // product = product.Where(it => !it.sku.Equals("FGSTSB150")).ToList();

            // // Set Product List
            // model.product = product;

            // // Call Manufacture Order API
            // string json = JsonConvert.SerializeObject(model);

            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var result = client.PostAsync("http://localhost:8069/odoo_controller/addMO", content).Result;

            return PartialView("_SummaryListingPartial", summaryItemList);
        }

        [HttpGet]
        public ActionResult PackageIdListingPartial()
        {
            //string dbHost = "47.254.232.38";
            string dbHost = "localhost";
            string dbName = "odoo_new";
            string dbUsername = "txe1";
            string dbPassword = "arf11234";

            // Create Postgres Connection and Get List of Products
            string PostGreSQLConn = $"Host={dbHost};Port=5432;Username={dbUsername};Password={dbPassword};Database={dbName}";
            DAL PostgresDal = new PostgreSqlDAL(PostGreSQLConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            List<ClsData> ls;
            string query;

            query = "SELECT id, name FROM VNtlJobOrder ORDER BY id ASC;";
            ls = dao.GetAllClsDataQuery(PostgresDal, query);

            List<mPackageId> model = ls.Select(obj => new mPackageId(obj.data)).ToList();
            return PartialView("_PackageIdListingPartial", model);
        }

        [HttpPost]
        public ActionResult PackageListingPartial(int order_id)
        {
            //string dbHost = "47.254.232.38";
            string dbHost = "localhost";
            string dbName = "odoo_new";
            string dbUsername = "txe1";
            string dbPassword = "arf11234";

            // Create Postgres Connection and Get List of Products
            string PostGreSQLConn = $"Host={dbHost};Port=5432;Username={dbUsername};Password={dbPassword};Database={dbName}";
            DAL PostgresDal = new PostgreSqlDAL(PostGreSQLConn);
            ClsDataDAOImpl dao = new ClsDataDAOImpl();

            List<ClsData> ls;
            string query;

            Debug.WriteLine(order_id);

            query = $"SELECT * FROM VNtlJobOrder WHERE id={order_id};";
            ls = dao.GetAllClsDataQuery(PostgresDal, query);

            Debug.WriteLine(ls.ToList().Count);

            List<mJobOrder> orderList = ls.Select(obj => new mJobOrder(obj.data)).ToList();

            query = $"SELECT * FROM VNtlJobOrderItem WHERE order_id={order_id};";
            ls = dao.GetAllClsDataQuery(PostgresDal, query);

            List<mJobOrderItem> itemList = new List<mJobOrderItem>();

            for(int i = 0; i < ls.Count; i+=2)
            {
                mJobOrderItem item = new mJobOrderItem();
                item.PackageListing(ls.ElementAt(i).data);
                item.SummaryListing(ls.ElementAt(i + 1).data);
                itemList.Add(item);
            }

            mPackageListing model = new mPackageListing();
            model.order = orderList.FirstOrDefault(it => it.id == order_id);
            model.itemList = itemList;

            Response.StatusCode = 200;
            return PartialView("_PackageListingPartial", model);
        }
    }
}
