using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class UserRoleController : AdminController
    {
        // GET: UserRole
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult UserRoleGridViewPartial(int user_id)
        {
            ViewData["user_id"] = user_id;
            
            var model = db.TNtlUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.Where(it => it.user_id == user_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserRoleGridViewPartialAddNew(TNtlUserRole item, int user_id)
        {
            ViewData["user_id"] = user_id;
            item.user_id = user_id;

            DbStoredProcedure.UserRoleInsert(item);
            
            var model = db.TNtlUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.Where(it => it.user_id == user_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserRoleGridViewPartialUpdate(TNtlUserRole item, int user_id)
        {
            ViewData["user_id"] = user_id;
            item.user_id = user_id;

            DbStoredProcedure.UserRoleUpdate(item);

            var model = db.TNtlUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.Where(it => it.user_id == user_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserRoleGridViewPartialDelete(int id, int user_id)
        {
            ViewData["user_id"] = user_id;
            DbStoredProcedure.UserRoleDelete(id);

            var model = db.TNtlUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.Where(it => it.user_id == user_id).ToList());
        }
    }
}