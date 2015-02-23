using Microsoft.AspNet.Identity.EntityFramework;

namespace MinimalOwinWebApiSelfHost.Models
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string email):base(email)
        {
            UserName = email;
        }
    }
}