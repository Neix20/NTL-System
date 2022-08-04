using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class JobOrderItemController : Controller
    {
        // GET: JobOrderItem
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult JobOrderItemGridViewPartial(int order_id)
        {
            ViewData["order_id"] = order_id;
            var model = db.TNtlJobOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_JobOrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobOrderItemGridViewPartialAddNew(TNtlJobOrderItem item, int order_id)
        {
            var model = db.TNtlJobOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_JobOrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobOrderItemGridViewPartialUpdate(TNtlJobOrderItem item, int order_id)
        {
            var model = db.TNtlJobOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_JobOrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobOrderItemGridViewPartialDelete(int id, int order_id)
        {
            DbStoredProcedure.JobOrderItemDelete(id);

            var model = db.TNtlJobOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_JobOrderItemGridViewPartial", model.ToList());
        }
    }
}