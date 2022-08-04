using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class PermissionController : Controller
    {
        // GET: Permission
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult PermissionGridViewPartial()
        {
            var model = db.TNtlPermissions;
            return PartialView("_PermissionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PermissionGridViewPartialAddNew(NtlSystem.Models.TNtlPermission item)
        {
            DbStoredProcedure.PermissionInsert(item);

            var model = db.TNtlPermissions;
            return PartialView("_PermissionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PermissionGridViewPartialUpdate(NtlSystem.Models.TNtlPermission item)
        {
            DbStoredProcedure.PermissionUpdate(item);

            var model = db.TNtlPermissions;
            return PartialView("_PermissionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PermissionGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.PermissionDelete(id);

            var model = db.TNtlPermissions;
            return PartialView("_PermissionGridViewPartial", model.ToList());
        }
    }
}