using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// The Namespace Ex3 Of The Project.
/// </summary>
namespace Ex3
{
    /// <summary>
    /// RouteConfig class
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// The Function RegisterRoutes Registers The Routes In The RouteCollection routes which
        /// it gets as a parameter.
        /// </summary>
        /// <param name="routes">RouteCollection routes in which we register.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("display", "display/{ip}/{port}/{time}",
            defaults: new { controller = "Display", action = "display" , time = UrlParameter.Optional});

            routes.MapRoute("save", "save/{ip}/{port}/{time}/{timer}/{filename}",
            defaults: new { controller = "Save", action = "save" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
