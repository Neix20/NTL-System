using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Optimization;

namespace NtlSystem.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.6.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsPDF").Include("~/Scripts/jspdf.min.js", "~/Scripts/html2PDF.js"));

            bundles.Add(new ScriptBundle("~/bundles/auto-table").Include("~/Scripts/auto-tables.js"));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include("~/Scripts/chart.js", "~/Scripts/chartjs-plugin-datalabels.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontAwesome").Include("~/Scripts/fontawesome/all.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/layout").Include("~/Content/ample-layout.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include("~/Content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/fontAwesome").Include("~/Content/all.min.css"));
        }

        internal static void RegisterBundles(object bundles)
        {
            throw new NotImplementedException();
        }
    }
}