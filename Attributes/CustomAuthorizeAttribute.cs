using System.Net;
using System.Web.Mvc;

namespace Asp_Net_FinalProject.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Errors/AccessDenied.cshtml"
            };
        }
    }

}
