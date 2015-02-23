using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MinimalOwinWebApiSelfHost.Models
{
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override async void Seed(ApplicationDbContext context)
        {
            //base.Seed(context);
            context.Companies.Add(new Company { Name = "Microsoft" });
            context.Companies.Add(new Company { Name = "Google" });
            context.Companies.Add(new Company { Name = "Apple" });
            context.SaveChanges();

            var john = new ApplicationUser {Email = "john@example.com", UserName = "john@example.com"};
            var jimi = new ApplicationUser {Email = "jimi@example.com", UserName = "jimi@example.com"};

            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var result1 = await manager.CreateAsync(john, "JohnsPassword");
            var result2 = await manager.CreateAsync(jimi, "JimisPassword");

            await manager.AddClaimAsync(john.Id,
                new Claim(ClaimTypes.Role, "Admin"));

            await manager.AddClaimAsync(john.Id,
                new Claim(ClaimTypes.Name, john.Email));

            await manager.AddClaimAsync(jimi.Id,
                new Claim(ClaimTypes.Name, jimi.Email));
            await manager.AddClaimAsync(jimi.Id,
                new Claim(ClaimTypes.Role, "User"));

        }
    }
}