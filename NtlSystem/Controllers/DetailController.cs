using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class DetailController : AdminController
    {
        // GET: Detail
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult DetailGridViewPartial()
        {
            var model = db.TNtlDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialAddNew(NtlSystem.Models.TNtlDetail item)
        {
            string username = "admin";
            DbStoredProcedure.DetailInsert(item.name, item.remark, username, username);

            var model = db.TNtlDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialUpdate(NtlSystem.Models.TNtlDetail item)
        {
            string username = "admin";
            DbStoredProcedure.DetailUpdate(item.id, item.name, item.remark, item.created_by, item.created_date, username);

            var model = db.TNtlDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.DetailDelete(id);

            var model = db.TNtlDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }
    }
}