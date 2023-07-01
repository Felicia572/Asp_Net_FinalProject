using System.Web;
using System.Web.Mvc;

namespace Asp_Net_FinalProject.Attributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _adminUser;
        public AdminAuthorizeAttribute()
        {
            _adminUser = "admin@example.com";
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string dynamicUser = httpContext.User.Identity.Name;

            // Allow access only if the user is the admin user
            if (dynamicUser == _adminUser)
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Errors/AccessDenied.cshtml"
            };
        }
    }
}
