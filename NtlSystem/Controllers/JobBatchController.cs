using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NtlSystem.Models;

namespace NtlSystem.Controllers
{
    public class JobBatchController : AdminController
    {
        // GET: JobBatch
        public ActionResult Index()
        {
            return View();
        }

        dbNtlSystemEntities db = new dbNtlSystemEntities();

        [ValidateInput(false)]
        public ActionResult JobBatchGridViewPartial()
        {
            var model = db.TNtlJobBatches;
            return PartialView("_JobBatchGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobBatchGridViewPartialAddNew(TNtlJobBatch item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            item.status_id = DbStoredProcedure.GetStatusID("incomplete", "Job Batch");
            DbStoredProcedure.JobBatchInsert(item, username);

            var model = db.TNtlJobBatches;
            return PartialView("_JobBatchGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobBatchGridViewPartialUpdate(TNtlJobBatch item)
        {
            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";
            DbStoredProcedure.JobBatchUpdate(item, username);

            var model = db.TNtlJobBatches;
            return PartialView("_JobBatchGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult JobBatchGridViewPartialDelete(int id)
        {
            DbStoredProcedure.JobBatchDelete(id);

            var model = db.TNtlJobBatches;
            return PartialView("_JobBatchGridViewPartial", model.ToList());
        }

        [HttpPost]
        public void CompleteBatch(int batch_id)
        {
            // Set Job Batch Status to Complete
            int c_sta_id = DbStoredProcedure.GetStatusID("complete", "Job Batch");

            var item = db.TNtlJobBatches.Find(batch_id);
            item.status_id = c_sta_id;

            string username = (User.Identity.IsAuthenticated) ? User.Identity.Name : "Admin";

            // Complete All Job Order within Job Batch
            int joc_sta_id = DbStoredProcedure.GetStatusID("complete", "Job Order");

            JobOrderController jcController = new JobOrderController();

            db.TNtlJobOrders.Where(it => it.batch_id == item.id).ToList().ForEach(it => {
                it.status_id = joc_sta_id;
                jcController.CompleteOrder(it.id);
                DbStoredProcedure.JobOrderUpdate(it, username);
            });

            DbStoredProcedure.JobBatchUpdate(item, username);
        }
    }
}