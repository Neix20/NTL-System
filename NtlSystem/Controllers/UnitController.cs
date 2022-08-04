using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class UnitController : Controller
    {
        // GET: Unit
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult UnitTreeListPartial()
        {
            var model = db.TNtlUnits;
            return PartialView("_UnitTreeListPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UnitTreeListPartialAddNew(TNtlUnit item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.UnitInsert(item, username);

            var model = db.TNtlUnits;
            return PartialView("_UnitTreeListPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UnitTreeListPartialUpdate(TNtlUnit item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.UnitUpdate(item, username);

            var model = db.TNtlUnits;
            return PartialView("_UnitTreeListPartial", model.ToList());
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult UnitTreeListPartialDelete(int id)
        {
            DbStoredProcedure.UnitDelete(id);

            var model = db.TNtlUnits;
            return PartialView("_UnitTreeListPartial", model.ToList());
        }
    }
}