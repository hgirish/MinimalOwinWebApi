using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MinimalOwinWebApiSelfHost.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("MyDatabase")
        {
            
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IDbSet<Company> Companies { get; set; }
       
    }
}