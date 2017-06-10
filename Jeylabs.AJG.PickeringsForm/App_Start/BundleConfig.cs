using System.Web;
using System.Web.Optimization;

namespace Jeylabs.AJG.PickeringsForm
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/angular.min.js",
                         "~/Scripts/CustomAngular/Module.js",
                         "~/Scripts/CustomAngular/Factories/crud-factory.js",
                         "~/Scripts/CustomAngular/Controllers/PickeringsFormController.js",
                         "~/Scripts/CustomAngular/Directives/claims-advice.js",
                         "~/Scripts/CustomAngular/Directives/third-party-blame.js",
                          "~/Scripts/CustomAngular/Directives/witness-at-scene.js",
                          "~/Scripts/CustomAngular/Directives/claim-grid.js",
                          "~/Scripts/CustomAngular/Directives/third-party-file-upload.js",
                          "~/Scripts/CustomAngular/Directives/support-file-upload.js",
                          "~/Scripts/CustomAngular/Directives/enter-as-tab.js",
                          "~/Scripts/CustomAngular/Directives/scene-witness-directive.js",
                           "~/Scripts/moment.js ",
                          "~/Scripts/bootstrap-datetimepicker.min.js"                       
                      ));



            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-select.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       //"~/Content/site.css",
                       "~/Content/font-awesome.min.css",
                        "~/Content/FormStyle.css",
                        "~/Content/bootstrap-datetimepicker.min.css",
                        "~/Content/loader.min.css",
                        "~/Content/segment.min.css"
                      ));
        }
    }
}
