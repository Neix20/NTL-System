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
    public class StockWarehouseController : Controller
    {
        // GET: StockWarehouse
        public ActionResult Index()
        {
            return View();
        }

        NtlSystem.Models.dbNtlSystemEntities db = new NtlSystem.Models.dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartial()
        {
            var model = db.TNtlStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialAddNew(NtlSystem.Models.TNtlStockWarehouse item)
        {
            item.address_line_1 = (item.address_line_1 == null) ? "address_line_1" : item.address_line_1;
            item.address_line_1 = item.address_line_1.Replace(",", String.Empty);

            item.address_line_2 = (item.address_line_2 == null) ? "address_line_2" : item.address_line_2;
            item.address_line_2 = item.address_line_2.Replace(",", String.Empty);

            string username = "Admin";
            DbStoredProcedure.StockWarehouseInsert(item, username);

            var file = Request.Files["warehouse_image"];

            int stock_warehouse_id = DbStoredProcedure.GetID("TNtlStockWarehouse");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/StockWarehouseImages")}\\{stock_warehouse_id}_{item.name}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/StockWarehouseImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap) Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            var model = db.TNtlStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialUpdate(NtlSystem.Models.TNtlStockWarehouse item)
        {
            string address = Request.Form["Address"];
            string[] address_arr = GeneralFunc.FormatAddress(address);
            item.address_line_1 = address_arr[0];
            item.address_line_2 = address_arr[1];
            item.city = address_arr[2];
            item.zip_code = int.Parse(address_arr[3]);
            item.state = address_arr[4];
            item.country = address_arr[5];

            string username = "admin";
            DbStoredProcedure.StockWarehouseUpdate(item, username);

            var model = db.TNtlStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialDelete(System.Int32 id)
        {
            DbStoredProcedure.StockWarehouseDelete(id);

            var model = db.TNtlStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }
    }
}