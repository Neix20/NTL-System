using DevExpress.Web.Mvc;
using Newtonsoft.Json;
using NtlSystem.Models;
using NtlSystem.Models.OdooManufactureOrderModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class SummaryItemController : Controller
    {

        private static readonly HttpClient client = new HttpClient();

        // GET: SummaryItem
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult SummaryItemGridViewPartial()
        {
            var model = db.TNtlSummaryItems;
            return PartialView("_SummaryItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SummaryItemGridViewPartialAddNew(TNtlSummaryItem item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.SummaryItemInsert(item, username);

            var model = db.TNtlSummaryItems;
            return PartialView("_SummaryItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SummaryItemGridViewPartialUpdate(TNtlSummaryItem item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.SummaryItemUpdate(item, username);

            var model = db.TNtlSummaryItems;
            return PartialView("_SummaryItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SummaryItemGridViewPartialDelete(int id)
        {

            DbStoredProcedure.SummaryItemDelete(id);

            var model = db.TNtlSummaryItems;
            return PartialView("_SummaryItemGridViewPartial", model.ToList());
        }

        [HttpPost]
        public JsonResult CompleteSummaryItem(int summary_item_id)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            // Update Complete Status
            var summaryItem = db.TNtlSummaryItems.Find(summary_item_id);

            int c_sta_id = DbStoredProcedure.GetStatusID("complete", "Summary Item");
            summaryItem.status_id = c_sta_id;

            DbStoredProcedure.SummaryItemUpdate(summaryItem, username);

            Response.StatusCode = 200;
            return Json($"Successfully Updated Summary Item {summaryItem.sku} status to complete!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CompleteOdooProcess()
        {

            int si_c_sta_id = DbStoredProcedure.GetStatusID("complete", "Summary Item");
            int jo_c_sta_id = DbStoredProcedure.GetStatusID("complete", "Job Order");

            // Create Manufacturing Orders
            mOdooMan model = new mOdooMan();

            // Set Stock Name
            model.stock_name = "Stock";

            // Set Company Name
            model.company_name = "YourCompany";

            Debug.WriteLine(db.TNtlSummaryItems.Where(it => it.status_id == si_c_sta_id).ToList().Count);

            // Get All Complete Summary Item
            List<mOdooManProduct> product = db.TNtlSummaryItems.ToList()
                .Where(it => it.status_id == si_c_sta_id)
                .GroupBy(it => it.sku)
                .Select(it => new mOdooManProduct()
                {
                    sku = it.First().sku,
                    qty = it.Sum(x => (x.quantity * x.width * x.height / 100 / 100))
                }).ToList();

            // Set Product List
            model.product = product;

            // Call Manufacture Order API
            string json = JsonConvert.SerializeObject(model);
            Debug.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = client.PostAsync("http://localhost:8069/odoo_controller/addMO", content).Result;

            // Get All Complete Job Orders
            // Update Delivery
            db.TNtlJobOrders.Where(it => it.status_id == jo_c_sta_id).ToList().ForEach(it => {
                var delData = new {
                    sale_order = it.code
                };

                // Call Manufacture Order API
                string delJson = JsonConvert.SerializeObject(delData);

                var delContent = new StringContent(delJson, Encoding.UTF8, "application/json");
                var delResult = client.PostAsync("http://localhost:8069/odoo_controller/addDO", delContent).Result;
            });

            Response.StatusCode = 200;
            return Json("Successfully Completed Odoo Process!", JsonRequestBehavior.AllowGet);
        }
     }
}
