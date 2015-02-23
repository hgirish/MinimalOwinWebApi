using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using MinimalOwinWebApiSelfHost.Models;

namespace MinimalOwinWebApiSelfHost.OAuthServerProvider
{
    public class ApplicationOAuthServerProvider
        : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // This call is required...
            // but we're not using client authentication, so validate and move on...
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var manager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await manager.FindAsync(context.UserName, context.Password);

           
            if (user == null )
            {
                 context.SetError(
                    "Invalid_grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }
            
            var identity =
                new ClaimsIdentity(context.Options.AuthenticationType);
            foreach (var userClaim in user.Claims)
            {
                identity.AddClaim(new Claim(userClaim.ClaimType,userClaim.ClaimValue));
            }

             context.Validated(identity);
           
        }
    }
}