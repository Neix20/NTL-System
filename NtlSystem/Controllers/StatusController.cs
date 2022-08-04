using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class StatusController : AdminController
    {
        // GET: Status
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult StatusGridViewPartial()
        {
            var model = db.TNtlStatus;
            return PartialView("_StatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StatusGridViewPartialAddNew(NtlSystem.Models.TNtlStatu item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.StatusInsert(item, username);

            var model = db.TNtlStatus;
            return PartialView("_StatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StatusGridViewPartialUpdate(NtlSystem.Models.TNtlStatu item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.StatusUpdate(item, username);

            var model = db.TNtlStatus;
            return PartialView("_StatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StatusGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.StatusDelete(id);

            var model = db.TNtlStatus;
            return PartialView("_StatusGridViewPartial", model.ToList());
        }
    }
}