using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class OrderController : AdminController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult OrderGridViewPartial()
        {
            var model = db.TNtlOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialAddNew(TNtlOrder item)
        {
            TNtlCustomer customer = db.TNtlCustomers.Find(item.customer_id);

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            item.sub_total_price = 0;
            item.discount_fee = 0;
            item.total_price = 0;
            item.tax_price = 0;

            item.created_by = customer.name;
            item.last_updated_by = username;
            item.last_updated_date = DateTime.Today;     

            item.status_id = DbStoredProcedure.GetStatusID("draft", "Order");

            DbStoredProcedure.OrderInsert(item, username);

            var model = db.TNtlOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialUpdate(TNtlOrder item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            item.last_updated_by = username;
            item.last_updated_date = DateTime.Today;     

            DbStoredProcedure.OrderUpdate(item, username);

            var model = db.TNtlOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialDelete(int id)
        {
            DbStoredProcedure.OrderDelete(id);

            var model = db.TNtlOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        public JsonResult CompleteQuotation(int order_id)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            // Update Complete Status
            var order = db.TNtlOrders.Find(order_id);
            int sta_id = DbStoredProcedure.GetStatusID("Quotation", "Order");
            order.status_id = sta_id;

            order.last_updated_by = username;
            order.last_updated_date = DateTime.Today;     

            DbStoredProcedure.OrderUpdate(order, username);

            Response.StatusCode = 200;
            return Json($"Completed Order #{order_id}!", JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult CompleteSale(int order_id)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            // Update Complete Status
            var order = db.TNtlOrders.Find(order_id);
            int sta_id = DbStoredProcedure.GetStatusID("Sale", "Order");
            order.status_id = sta_id;

            order.last_updated_by = username;
            order.last_updated_date = DateTime.Today;     

            DbStoredProcedure.OrderUpdate(order, username);

            Response.StatusCode = 200;
            return Json($"Completed Order #{order_id}!", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CompleteDelivery(int order_id)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            Response.StatusCode = 200;
            return Json($"Completed Order #{order_id}!", JsonRequestBehavior.AllowGet);
        }
    }
}