using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopinv.SiteExtension
{
    public class KycRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            // 🔹 Example: Read KYC status from Session
            bool isKycCompleted = true;

            if (httpContext.Session != null &&
                httpContext.Session["IsKycCompleted"] != null)
            {
                isKycCompleted = Convert.ToBoolean(httpContext.Session["IsKycCompleted"]);
            }

            // 🔹 Prevent infinite redirect
            var currentUrl = httpContext.Request.RawUrl.ToLower();
            if (currentUrl.Contains("kycwarning"))
                return;

            // 🔹 Redirect if KYC not completed
            if (!isKycCompleted)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            controller = "Home",
                            action = "KycWarning"
                        }
                    )
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}