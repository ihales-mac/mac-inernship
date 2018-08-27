namespace TestAccountApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using TestAccountApp.Database;
    using TestAccountApp.Models;
    using TestAccountApp.UserManagement;
    internal sealed class Configuration : DbMigrationsConfiguration<TestAccountApp.Database.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TestAccountApp.Database.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var ctx = new ApplicationDbContext();
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

 
            var user = new ApplicationUser()
            {
                UserName = "SuperPowerUser",
                Email = "taiseer.joudeh@mymail.com",
                EmailConfirmed = false,
                FirstName = "Taiseer",
                LastName = "Joudeh",
                JoinDate = DateTime.Now.AddYears(-3),
            

            };
            manager.Create(user, "MySuperP@ssword!");

            var details = new AspNetUserDetail()
            {
                DOB = DateTime.Now.AddYears(-32).AddMonths(+4),
                UserId = user.Id

            };

            ctx.AspNetUserDetails.Add(details);
            ctx.SaveChanges();

            if (roleManager.Roles.Count() == 0)
                {

                    roleManager.Create(new IdentityRole { Name = "Physician" });
                    roleManager.Create(new IdentityRole { Name = "Patient" });
                    roleManager.Create(new IdentityRole { Name = "Pharmacist" });
                    roleManager.Create(new IdentityRole { Name = "Admin" });

                }

                var adminUser = manager.FindByName("SuperPowerUser");

                manager.AddToRoles(adminUser.Id, new string[] { "Admin" });
                manager.AddClaim(adminUser.Id, new Claim("role", "Admin"));
            }
            
           
        
    }
}
