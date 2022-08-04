using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class PaymentMethodController : Controller
    {
        // GET: PaymentMethod
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartial()
        {
            var model = db.TNtlPaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialAddNew(NtlSystem.Models.TNtlPaymentMethod item)
        {
            string username = "admin";
            DbStoredProcedure.PaymentMethodInsert(item, username);

            var model = db.TNtlPaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialUpdate(NtlSystem.Models.TNtlPaymentMethod item)
        {
            string username = "admin";
            DbStoredProcedure.PaymentMethodUpdate(item, username);

            var model = db.TNtlPaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.PaymentMethodDelete(id);

            var model = db.TNtlPaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }
    }
}