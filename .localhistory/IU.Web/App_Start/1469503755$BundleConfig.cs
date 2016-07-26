using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Optimization;

namespace IU.Web
{
    [ExcludeFromCodeCoverage]
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Clear();
            //bundles.ResetAll();

            BundleTable.EnableOptimizations = false;

            Bundle jqueryval = new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*");
            jqueryval.Transforms.Clear();
            bundles.Add(jqueryval);

            //Angular
            Bundle angular = new ScriptBundle("~/bundles/angular").Include(
                    "~/AngularApp/1.2.6/sweet-alert.min.js",
                     "~/AngularApp/1.2.6/angular.min.js",
                     "~/AngularApp/1.2.6/angular-route.min.js",
                     "~/AngularApp/1.2.6/angular-resource.min.js",
                     "~/AngularApp/1.2.6/angular-animate.min.js",
                     "~/AngularApp/1.2.6/ngSweetAlert.js");
            angular.Transforms.Clear();
            bundles.Add(angular);

            Bundle myapp = new ScriptBundle("~/bundles/myapp").Include(
                        "~/AngularApp/myapp/initial/app.js");
            myapp.Transforms.Clear();
            bundles.Add(myapp);

            
            Bundle controllers = new ScriptBundle("~/bundles/controllers").IncludeDirectory(
                        "~/AngularApp/myapp/controllers/", "*.js");
            controllers.Transforms.Clear();
            bundles.Add(controllers);

            
            Bundle factories = new ScriptBundle("~/bundles/services").IncludeDirectory(
                        "~/AngularApp/myapp/services/", "*.js");
            factories.Transforms.Clear();
            bundles.Add(factories);

            Bundle signalr_core = new ScriptBundle("~/bundles/signalr_core").Include(
                        "~/Scripts/jquery.signalR-2.2.0.min.js");
            factories.Transforms.Clear();
            bundles.Add(signalr_core);
                      

        }
    }
}
