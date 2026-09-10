//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;

//namespace Aarogyabhumi
//{
//    public class RouteConfig
//    {
//        public static void RegisterRoutes(RouteCollection routes)
//        {
//            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//            routes.MapRoute(
//                name: "Default",
//                url: "{controller}/{action}/{id}",
//                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
//            );
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Aarogyabhumi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ✅ Yeh PEHLE add karo — SSO route
            routes.MapRoute(
                name: "HubbleSSO",
                url: "sso",
                defaults: new { controller = "Hubble", action = "Sso" }
            );
            // ✅ Coins balance endpoint
            routes.MapRoute(
                name: "HubbleBalance",
                url: "balance",
                defaults: new { controller = "Hubble", action = "Balance" }
            );

            // ✅ Coins debit endpoint
            routes.MapRoute(
                name: "HubbleDebit",
                url: "debit",
                defaults: new { controller = "Hubble", action = "Debit" }
            );

            // ✅ Coins reverse endpoint
            routes.MapRoute(
                name: "HubbleReverse",
                url: "reverse",
                defaults: new { controller = "Hubble", action = "Reverse" }
            );

            // Tumhara existing default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}