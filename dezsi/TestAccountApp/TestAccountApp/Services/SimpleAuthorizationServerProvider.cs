using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TestAccountApp.Database;
using TestAccountApp.UserManagement;

namespace TestAccountApp.Services
{
   public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
 
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
 
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
 
            using (ApplicationUserManager _manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
            {

                IdentityUser user = await _manager.FindAsync(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }
 
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            var ticket = new AuthenticationTicket(identity, null);
            context.Validated(ticket);
        }
    }
}