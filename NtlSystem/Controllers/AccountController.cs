using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NtlSystem.Models;
using System.Web.Security;
using System.Diagnostics;

namespace NtlSystem.Controllers
{
    public class AccountController : Controller
    {
        dbNtlSystemEntities db = new dbNtlSystemEntities();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Index", "JobOrderPostgres");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TNtlUser model)
        {
            string returnUrl = "/Homepage";

            string pwdHashed = SecurityHelper.HashPasswordFull(model.password);
            var dataItem = db.TNtlUsers.FirstOrDefault(it => it.username.Equals(model.username) && it.password.Equals(pwdHashed));

            // Get Permissions

            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(model.username, false);

                string[] role_arr = Roles.GetRolesForUser(model.username);

                if (!role_arr.Contains("Admin".ToLower()))
                {
                    return Content($"Error 403: You do not have authority to access this webpage");
                }

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Content(returnUrl);
                }

                return Content($"/Homepage/Index");
            }

            return Content($"Error 400: Invalid Username or Password");
        }

        [HttpPost]
        public bool CheckUsernameExist(TNtlUser model)
        {
            return db.TNtlUsers.Select(x => x.username).Contains(model.username);
        }

        [HttpPost]
        public bool CheckUsernameNotExist(TNtlUser model)
        {
            return !CheckUsernameExist(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(TNtlUser model)
        {
            var item = db.TNtlUsers.FirstOrDefault(it => it.username.Equals(model.username));
            item.password = SecurityHelper.HashPasswordFull(model.password);

            DbStoredProcedure.UserUpdate(item);

            return Content("/JobOrderPostgres/Index");
        }

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TNtlUser model)
        {
            // Create New User
            model.password = SecurityHelper.HashPasswordFull(model.password);
            DbStoredProcedure.UserInsert(model);

            int user_id = DbStoredProcedure.GetID("TNtlUser");

            // DbStoredProcedure.UserInsert(model);
            string role = GeneralFunc.TrimStr(Request.Form["role"]);
            int role_id = db.TNtlRoles.FirstOrDefault(it => it.name.ToLower().Equals(role.ToLower())).id;

            // Assign Role To User
            TNtlUserRole userRole = new TNtlUserRole();
            userRole.user_id = user_id;
            userRole.role_id = role_id;

            DbStoredProcedure.UserRoleInsert(userRole);

            return Content("/JobOrderPostgres/Index");
        }

        [Authorize]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}