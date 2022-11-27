using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace randevuProje
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Odalar",
                url: "Odalar/{adi}-{odaid}",
                defaults: new { controller = "Home", action = "Odalar", odaid = UrlParameter.Optional, adi = UrlParameter.Optional }
            );


            routes.MapRoute(
               name: "Ozellik",
               url: "Ozellik/{oisim}/{id}",
               defaults: new { controller = "Admin", action = "Ozellik", id = UrlParameter.Optional, oisim = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "ResimEkle",
               url: "ResimEkle/{bisim}/{id}",
               defaults: new { controller = "Admin", action = "ResimEkle", id = UrlParameter.Optional, bisim = UrlParameter.Optional }
           );


            routes.MapRoute(
              name: "Hata",
              url: "hata/{kod}",
              defaults: new { controller = "Error", action = "Page404", kod = UrlParameter.Optional }
          );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
