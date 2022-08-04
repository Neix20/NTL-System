using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class ProductComponentController : AdminController
    {
        // GET: ProductComponent
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartial(int product_id)
        {
            ViewData["product_id"] = product_id;

            var model = db.TNtlProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.Where(it => it.master_product_id == product_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialAddNew(TNtlProductComponent item, int product_id)
        {
            item.master_product_id = product_id;
            ViewData["product_id"] = item.master_product_id;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.ProductComponentInsert(item, username);

            var model = db.TNtlProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.Where(it => it.master_product_id == item.master_product_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialUpdate(TNtlProductComponent item, int product_id)
        {
            item.master_product_id = product_id;
            ViewData["product_id"] = item.master_product_id;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.ProductComponentUpdate(item, username);

            var model = db.TNtlProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.Where(it => it.master_product_id == item.master_product_id).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialDelete(int id, int product_id)
        {
            ViewData["product_id"] = product_id;
            DbStoredProcedure.ProductComponentDelete(id);

            var model = db.TNtlProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.Where(it => it.master_product_id == product_id).ToList());
        }
    }
}