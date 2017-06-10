using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jeylabs.AJG.PickeringsForm
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              "Confirmation",                                           // Route name
              "Confirmation/{id}",                            // URL with parameters
              new { controller = "Pickerings", action = "Acknowledgement", id = UrlParameter.Optional }  // Parameter defaults
          );

            routes.MapRoute(
             "Error",                                           // Route name
             "Error/{id}",                            // URL with parameters
             new { controller = "Pickerings", action = "Error", id = UrlParameter.Optional }  // Parameter defaults
         );
            routes.MapRoute(
                name: "Default",                
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Pickerings", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
