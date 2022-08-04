using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class PlatformController : AdminController
    {
        // GET: Platform
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult PlatformGridViewPartial()
        {
            var model = db.TNtlPlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialAddNew(TNtlPlatform item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.PlatformInsert(item, username);

            var model = db.TNtlPlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialUpdate(TNtlPlatform item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.PlatformUpdate(item, username);

            var model = db.TNtlPlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialDelete(int id)
        {
            DbStoredProcedure.PlatformDelete(id);

            var model = db.TNtlPlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }
    }
}