using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EServices_1._0.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Js").Include(
                "~/Content/js/jquery.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/js/jquery.scrollTo.min.js",
                "~/Content/js/jquery.nicescroll.js",
                "~/Content/js/scripts.js",
                "~/Scripts/printThis.js"

                ));
            bundles.Add(new ScriptBundle("~/DataTableJs").Include(
                 "~/Content/datatables.net/js/jquery.dataTables.min.js",
                "~/Content/datatables.net-bs/js/dataTables.bootstrap.min.js"
                
                ));
            bundles.Add(new StyleBundle("~/Css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/css/style.css",
                "~/Content/css/style-responsive.css"

                ));

            bundles.Add(new StyleBundle("~/DataTableCss").Include(
               "~/Content/datatables.net-bs/css/dataTables.bootstrap.min.css"
              
               ));
       

            ////for parent area
            //bundles.Add(new ScriptBundle("~/ParentAreaJs").Include(
            //  "~/Areas/JavaScriptFiles/jquery-3.1.1.min.js",
            //  "~/Areas/JavaScriptFiles/jquery.signalR-2.4.1.min.js",
            //  "~/Areas/JavaScriptFiles/notification-tab.js",
            //  "~/Areas/JavaScriptFiles/MyJs.js"
            //  ));
            //bundles.Add(new StyleBundle("~/ParentAreaCss").Include(
            // "~/Areas/layoutDesign/bootstrap/css/bootstrap.css",
            // "~/Areas/layoutDesign/Nofication.css",
            // "~/Areas/layoutDesign/style.css"
            // ));


            BundleTable.EnableOptimizations = true;
        }
    }
}