using DevExpress.Web.Mvc;
using Newtonsoft.Json;
using NtlSystem.Models;
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
    public class JobOrderController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: JobOrder
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult JobOrderGridViewPartial(int batch_id)
        {
            ViewData["batch_id"] = batch_id;
            var model = db.TNtlJobOrders.Where(it => it.batch_id == batch_id);
            return PartialView("_JobOrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobOrderGridViewPartialAddNew(TNtlJobOrder item, int batch_id)
        {
            var model = db.TNtlJobOrders.Where(it => it.batch_id == batch_id);
            return PartialView("_JobOrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobOrderGridViewPartialUpdate(TNtlJobOrder item, int batch_id)
        {
            var model = db.TNtlJobOrders.Where(it => it.batch_id == batch_id);
            return PartialView("_JobOrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobOrderGridViewPartialDelete(int id, int batch_id)
        {
            DbStoredProcedure.JobOrderDelete(id);

            var model = db.TNtlJobOrders.Where(it => it.batch_id == batch_id);
            return PartialView("_JobOrderGridViewPartial", model.ToList());
        }

        [HttpPost]
        public void CompleteOrder(int order_id)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            // Set Job Order Status to Complete
            int c_sta_id = DbStoredProcedure.GetStatusID("complete", "Job Order");

            var item = db.TNtlJobOrders.Find(order_id);
            item.status_id = c_sta_id;

            // Update Summary Item Quantity
            db.TNtlJobOrderItems.Where(it => it.order_id == item.odoo_sales_id).ToList().ForEach(jobOrderItem => {
                string jo_key = GeneralFunc.GenJobOrderItemKey(jobOrderItem);

                Debug.WriteLine(jo_key);

                db.TNtlSummaryItems.ToList().ForEach(sItem => {
                    string key = GeneralFunc.GenSummaryItemKey(sItem);

                    Debug.WriteLine(key);

                    if(key.Equals(jo_key)) {
                        sItem.used += jobOrderItem.quantity;
                        DbStoredProcedure.SummaryItemUpdate(sItem, username);
                    }
                });

            });

            // // Update Delivery
            // var data = new {
            //     sale_order = item.code
            // };

            // // Call Manufacture Order API
            // string json = JsonConvert.SerializeObject(data);

            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var result = client.PostAsync("http://localhost:8069/odoo_controller/addDO", content).Result;

            DbStoredProcedure.JobOrderUpdate(item, username);
        }
    }
}
