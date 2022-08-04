using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NtlSystem.Models;

namespace NtlSystem.Controllers
{
    public class RoleController : AdminController
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult RoleGridViewPartial()
        {
            var model = db.TNtlRoles;
            return PartialView("_RoleGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RoleGridViewPartialAddNew(TNtlRole item)
        {
            DbStoredProcedure.RoleInsert(item);

            var model = db.TNtlRoles;
            return PartialView("_RoleGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RoleGridViewPartialUpdate(TNtlRole item)
        {
            DbStoredProcedure.RoleUpdate(item);

            var model = db.TNtlRoles;
            return PartialView("_RoleGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RoleGridViewPartialDelete(int id)
        {
            DbStoredProcedure.RoleDelete(id);

            var model = db.TNtlRoles;
            return PartialView("_RoleGridViewPartial", model.ToList());
        }
    }
}