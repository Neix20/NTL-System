using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class UserController : AdminController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult UserGridViewPartial()
        {
            var model = db.TNtlUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialAddNew(NtlSystem.Models.TNtlUser item)
        {
            item.password = SecurityHelper.HashPasswordFull(item.password);
            DbStoredProcedure.UserInsert(item);

            var model = db.TNtlUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialUpdate(NtlSystem.Models.TNtlUser item)
        {
            DbStoredProcedure.UserUpdate(item);

            var model = db.TNtlUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.UserDelete(id);

            var model = db.TNtlUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }
    }
}