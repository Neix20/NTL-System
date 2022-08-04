using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class RolePermissionController : Controller
    {
        // GET: RolePermission
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult RolePermissionGridViewPartial(int role_id)
        {
            ViewData["role_id"] = role_id;

            var model = db.TNtlRolePermissions;
            return PartialView("_RolePermissionGridViewPartial", model.Where(it => it.role_id == role_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RolePermissionGridViewPartialAddNew(TNtlRolePermission item, int role_id)
        {
            ViewData["role_id"] = role_id;
            item.role_id = role_id;

            DbStoredProcedure.RolePermissionInsert(item);

            var model = db.TNtlRolePermissions;
            return PartialView("_RolePermissionGridViewPartial", model.Where(it => it.role_id == role_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RolePermissionGridViewPartialUpdate(TNtlRolePermission item, int role_id)
        {
            ViewData["role_id"] = role_id;
            item.role_id = role_id;

            DbStoredProcedure.RolePermissionUpdate(item);

            var model = db.TNtlRolePermissions;
            return PartialView("_RolePermissionGridViewPartial", model.Where(it => it.role_id == role_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RolePermissionGridViewPartialDelete(int id, int role_id)
        {
            ViewData["role_id"] = role_id;
            DbStoredProcedure.RolePermissionDelete(id);

            var model = db.TNtlRolePermissions;
            return PartialView("_RolePermissionGridViewPartial", model.Where(it => it.role_id == role_id).ToList());
        }
    }
}