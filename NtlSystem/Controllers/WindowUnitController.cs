using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class WindowUnitController : Controller
    {
        // GET: WindowUnit
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult WindowUnitGridViewPartial()
        {
            var model = db.TNtlWindowUnits;
            return PartialView("_WindowUnitGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WindowUnitGridViewPartialAddNew(TNtlWindowUnit item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.WindowUnitInsert(item, username);

            var model = db.TNtlWindowUnits;
            return PartialView("_WindowUnitGridViewPartial", model.ToList());
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult WindowUnitGridViewPartialUpdate(TNtlWindowUnit item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.WindowUnitUpdate(item, username);

            var model = db.TNtlWindowUnits;
            return PartialView("_WindowUnitGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WindowUnitGridViewPartialDelete(int id)
        {
            DbStoredProcedure.WindowUnitDelete(id);

            var model = db.TNtlWindowUnits;
            return PartialView("_WindowUnitGridViewPartial", model.ToList());
        }
    }
}