using DevExpress.Web.Mvc;
using NtlSystem.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class StockItemController : Controller
    {
        // GET: StockItem
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult StockItemGridViewPartial()
        {
            var model = db.TNtlStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialAddNew(NtlSystem.Models.TNtlStockItem item)
        {
            string username = "admin";

            var product = db.TNtlProducts.Where(it => it.id == item.product_id).FirstOrDefault();

            item.name = product.name;
            item.description = product.description;

            DbStoredProcedure.StockItemInsert(item, username);

            var file = Request.Files["stock_item_image"];

            int stock_item_id = DbStoredProcedure.GetID("TNtlStockItem");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/ProductImages")}\\Product_{item.product_id}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/ProductImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            var model = db.TNtlStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialUpdate(TNtlStockItem item)
        {
            string username = "admin";
            DbStoredProcedure.StockItemUpdate(item, username);

            var model = db.TNtlStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialDelete(int id)
        {
            DbStoredProcedure.StockItemDelete(id);
            var model = db.TNtlStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }
    }
}