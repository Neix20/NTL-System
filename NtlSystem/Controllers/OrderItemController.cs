using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using NtlSystem.Models;

namespace NtlSystem.Controllers
{
    public class OrderItemController : AdminController
    {
        // GET: OrderItem
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult OrderItemGridViewPartial(int order_id)
        {
            ViewData["order_id"] = order_id;
            var model = db.TNtlOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialAddNew(TNtlOrderItem item, int order_id)
        {
            item.order_id = order_id;
            ViewData["order_id"] = order_id;

            var product = db.TNtlProducts.Find(item.product_id);

            // To Remove
            item.name = "";

            item.discount_fee = 0;
            item.tax_price = 0;

            item.unit_price = product.sell_price;
            item.uom_id = product.uom_id;

            // Update Total
            item.sub_total_price = item.unit_price * item.quantity;
            item.total_price = item.sub_total_price - item.discount_fee - item.tax_price;

            // Note
            string rgx = @"\[(.*)\] (([A-Za-z -]*)((\d+)cm)?)(\|(\d+)cm)?(\|(\d+))?";

            string _width = Regex.Replace(item.remark, rgx, "$5");
            string _height = Regex.Replace(item.remark, rgx, "$7");

            decimal width = (_width.Equals("")) ? 100 : Convert.ToDecimal(_width);
            decimal height = (_height.Equals("")) ? 100 : Convert.ToDecimal(_height);

            item.total_usage = width * height * item.quantity / 100 / 100;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.OrderItemInsert(item, username);

            // Update Price
            DbStoredProcedure.OrderUpdatePrice(order_id, item.total_price, username);

            var model = db.TNtlOrderItems.Where(it => it.order_id == order_id); 
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialUpdate(TNtlOrderItem item, int order_id)
        {
            item.order_id = order_id;
            item.name = "";
            ViewData["order_id"] = order_id;

            var product = db.TNtlProducts.Find(item.product_id);
            item.unit_price = product.sell_price;
            item.uom_id = product.uom_id;

            // Note
            string rgx = @"\[(.*)\] (([A-Za-z -]*)((\d+)cm)?)(\|(\d+)cm)?(\|(\d+))?";

            string _width = Regex.Replace(item.remark, rgx, "$5");
            string _height = Regex.Replace(item.remark, rgx, "$7");

            decimal width = (_width.Equals("")) ? 100 : Convert.ToDecimal(_width);
            decimal height = (_height.Equals("")) ? 100 : Convert.ToDecimal(_height);

            item.total_usage = width * height * item.quantity / 100 / 100;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.OrderItemUpdate(item, username);

            // Update Price
            DbStoredProcedure.OrderUpdatePrice(order_id, -1 * item.total_price, username);

            var model = db.TNtlOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialDelete(int id, int order_id)
        {
            ViewData["order_id"] = order_id;
            DbStoredProcedure.OrderItemDelete(id);

            var model = db.TNtlOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }
    }
}