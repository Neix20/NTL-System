using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class HomepageController : AdminController
    {
        // GET: Homepage
        public ActionResult Index()
        {
            return View();
        }
    }
}