using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class UomController : AdminController
    {
        // GET: Uom
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult UomGridViewPartial()
        {
            var model = db.TNtlUoms;
            return PartialView("_UomGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UomGridViewPartialAddNew(TNtlUom item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.UomInsert(item, username);

            var model = db.TNtlUoms;
            return PartialView("_UomGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UomGridViewPartialUpdate(TNtlUom item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.UomUpdate(item, username);

            var model = db.TNtlUoms;
            return PartialView("_UomGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UomGridViewPartialDelete(int id)
        {
            DbStoredProcedure.UomDelete(id);

            var model = db.TNtlUoms;
            return PartialView("_UomGridViewPartial", model.ToList());
        }
    }
}