using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NtlSystem.Models;

namespace NtlSystem.Controllers
{
    public class PlatformMessageController : Controller
    {
        dbNtlSystemEntities db = new dbNtlSystemEntities();

        // GET: PlatformMessage
        public ActionResult Index()
        {
            var customerIdList = db.TNtlCustomerChats.Select(it => (int)it.customer_id).Distinct();

            List<mCustomerMsg> model = new List<mCustomerMsg>();

            int unr_sta_id = DbStoredProcedure.GetStatusID("unread", "Platform Message");

            foreach(var id in customerIdList){
                var customer = db.TNtlCustomers.Find(id);
                var chatList = db.TNtlCustomerChats.Where(it => it.customer_id == id && it.status_id == unr_sta_id).ToList();

                int notification = chatList.Count;

                mCustomerMsg customerMsg = new mCustomerMsg(notification, customer);
                model.Add(customerMsg);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult MessengerPartial(int customer_id)
        {
            var model = db.TNtlCustomerChats.Where(it => it.customer_id == customer_id).ToList();
            return PartialView("_MessagePartial", model);
        }

        [HttpPost]
        public ActionResult AddMessage(TNtlCustomerChat item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            var user = db.TNtlUsers.FirstOrDefault(it => it.username.Equals(username));
            int user_id = (user != null) ? user.id : 0;

            int sta_id = DbStoredProcedure.GetStatusID("unsend", "Platform Message");
            int platform_id = DbStoredProcedure.GetPlatformID("shopee");
            item.user_id = user_id;
            item.status_id = sta_id;
            item.platform_id = platform_id;

            DbStoredProcedure.CustomerChatInsert(item);

            return PartialView("_OwnMessagePartial", item);
        }

        [HttpPost]
        public ActionResult UpdateNotificationStatus(int customer_id)
        {
            var chatList = db.TNtlCustomerChats.Where(it => it.customer_id == customer_id).ToList();

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            var user = db.TNtlUsers.FirstOrDefault(it => it.username.Equals(username));
            int user_id = (user != null) ? user.id : 0;

            int sta_id = DbStoredProcedure.GetStatusID("send", "Platform Message");

            chatList.ForEach(obj =>
            {
                // Set Read Status To Complete
                obj.status_id = sta_id;
                obj.user_id = user_id;

                DbStoredProcedure.CustomerChatUpdate(obj);
            });

            Response.StatusCode = 200;
            return Json("Successfully updated status!");
        }

        [HttpPost]
        public ActionResult ReverseUpdateNotificationStatus(int customer_id)
        {
            var chatList = db.TNtlCustomerChats.Where(it => it.customer_id == customer_id).ToList();

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            var user = db.TNtlUsers.FirstOrDefault(it => it.username.Equals(username));
            int user_id = (user != null) ? user.id : 0;

            int sta_id = DbStoredProcedure.GetStatusID("unread", "Platform Message");

            chatList.ForEach(obj =>
            {
                // Set Read Status To Complete
                obj.status_id = sta_id;
                obj.user_id = user_id;

                DbStoredProcedure.CustomerChatUpdate(obj);
            });

            Response.StatusCode = 200;
            return Json("Successfully reverse complete status to incomplete status!");
        }
    }
}