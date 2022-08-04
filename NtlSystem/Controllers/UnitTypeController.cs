using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class UnitTypeController : Controller
    {
        // GET: UnitType
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult UnitTypeGridViewPartial()
        {
            var model = db.TNtlUnitTypes;
            return PartialView("_UnitTypeGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UnitTypeGridViewPartialAddNew(TNtlUnitType item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.UnitTypeInsert(item, username);

            var model = db.TNtlUnitTypes;
            return PartialView("_UnitTypeGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UnitTypeGridViewPartialUpdate(TNtlUnitType item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.UnitTypeUpdate(item, username);

            var model = db.TNtlUnitTypes;
            return PartialView("_UnitTypeGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UnitTypeGridViewPartialDelete(int id)
        {
            DbStoredProcedure.UnitTypeDelete(id);

            var model = db.TNtlUnitTypes;
            return PartialView("_UnitTypeGridViewPartial", model.ToList());
        }
    }
}