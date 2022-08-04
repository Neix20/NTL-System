using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class CustomerController : AdminController
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult CustomerGridViewPartial()
        {
            var model = db.TNtlCustomers;
            return PartialView("_CustomerGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialAddNew(NtlSystem.Models.TNtlCustomer item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.CustomerInsert(item, username);

            var model = db.TNtlCustomers;
            return PartialView("_CustomerGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialUpdate(NtlSystem.Models.TNtlCustomer item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.CustomerUpdate(item, username);

            var model = db.TNtlCustomers;
            return PartialView("_CustomerGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.CustomerDelete(id);

            var model = db.TNtlCustomers;
            return PartialView("_CustomerGridViewPartial", model.ToList());
        }
    }
}