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
                query = "SELECT * FROM VNtlJobOrderItem;";
                ls = dao.GetAllClsDataQuery(PostgresDal, query);

                // Generate List of mJobOrder item List
                List<TNtlSummaryItem> tmpItemList = ls.Select(obj => GeneralFunc.GenerateSummaryListing(obj.data)).ToList();
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
                    mJobOrder joItem = new mJobOrder(obj.data);

                    TNtlJobOrder item = joItem.GenJobOrder();
                    item.batch_id = batch_id;
                    item.status_id = jo_inc_sta_id;

                    DbStoredProcedure.JobOrderInsert(item, username);
                }

                // Add List of Job Order Items
                query = $"SELECT * FROM VNtlJobOrderItem;";
                ls = dao.GetAllClsDataQuery(PostgresDal, query);

                foreach(var it in ls)
                {
                    mJobOrderItem obj = new mJobOrderItem(it.data);
                    TNtlJobOrderItem item = obj.GenJobOrderItem();
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

                // Issue: Code Refactoring Required
                // Add to Existing TNtlSummaryItem If Unique Key Already Exists
                db.TNtlSummaryItems.ToList().ForEach(it =>
                {
                    string key = GeneralFunc.GenSummaryItemKey(it);
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

            List<mJobOrder> orderList = ls.Select(obj => new mJobOrder(obj.data)).ToList();

            query = $"SELECT * FROM VNtlJobOrderItem WHERE order_id={order_id};";
            ls = dao.GetAllClsDataQuery(PostgresDal, query);

            List<mJobOrderItem> itemList = ls.Select(it => new mJobOrderItem(it.data)).ToList();

            mPackageListing model = new mPackageListing();
            model.order = orderList.FirstOrDefault(it => it.id == order_id);
            model.itemList = itemList;

            Response.StatusCode = 200;
            return PartialView("_PackageListingPartial", model);
        }
    }
}
