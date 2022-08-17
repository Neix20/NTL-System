using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtlSystem.Controllers
{
    public class LogController : Controller
    {
        string fileDir = $"D:\\ASP-dotNet-Projects\\SeleniumTest\\SeleniumTest\\bin\\Debug\\logs";

        // GET: Log
        public ActionResult Index()
        {
            List<string> fileLs = Directory.GetFiles(fileDir).ToList();
            fileLs = fileLs.Select(file => Path.GetFileName(file)).ToList();

            return View(fileLs);
        }
        
        public ActionResult GetLog(string filename)
        {
            filename = (filename.Equals("")) ? "log.txt" : filename;

            string filePath = $"{fileDir}\\{filename}";

            List<string> fileContent = System.IO.File.ReadAllLines(filePath).ToList();

            return View(fileContent);
        }

    }
}