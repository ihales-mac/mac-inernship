using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace TestAccountApp.UserManagement
{
    public class AuthorizeAdminAttribute : System.Web.Http.AuthorizeAttribute
    {
        
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {


            var authorized = base.IsAuthorized(actionContext);
            if (!authorized)
            {
                // The user is not authenticated
                return false;
            }

            ClaimsPrincipal principal = (ClaimsPrincipal)actionContext.RequestContext.Principal;

            var user = ClaimsPrincipal.Current;

            if (user.Identity.Name == null || user.Identity.Name == "")
                throw new Exception("user not found");

            var username = user.Identity.Name;
            if (user.IsInRole("Admin"))
            {
                // Administrator => let him in
                return true;
            }


            return false;
        }

       
    }
}