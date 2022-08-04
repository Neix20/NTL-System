using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class WindowController : Controller
    {
        // GET: Window
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult WindowGridViewPartial()
        {
            var model = db.TNtlWindows;
            return PartialView("_WindowGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WindowGridViewPartialAddNew(TNtlWindow item)
        {
            item.length = 0;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.WindowInsert(item, username);

            var model = db.TNtlWindows;
            return PartialView("_WindowGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WindowGridViewPartialUpdate(TNtlWindow item)
        {
            item.length = 0;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.WindowUpdate(item, username);
            
            var model = db.TNtlWindows;
            return PartialView("_WindowGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WindowGridViewPartialDelete(int id)
        {
            DbStoredProcedure.WindowDelete(id);

            var model = db.TNtlWindows;
            return PartialView("_WindowGridViewPartial", model.ToList());
        }
    }
}