using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LanguageCenter
{
    public class RouteConfig
    {
        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapRoute(
        //        name: "Default",
        //        url: "{controller}/{action}/{id}",
        //        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        //    );
        //}
        public static void RegisterRoutes(RouteCollection routes)
        {
            // routes are defined in the Areas folders.

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                           name: "Error",
                           url: "error",
                           defaults: new { controller = "Base", action = "Error" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).DataTokens = new RouteValueDictionary(new { area = "Home" });
        }
    }
}
