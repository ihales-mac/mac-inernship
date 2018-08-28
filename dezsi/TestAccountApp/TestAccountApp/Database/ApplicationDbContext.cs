using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestAccountApp.Models;

namespace TestAccountApp.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = true;

        }

        public DbSet<AspNetUserDetail> AspNetUserDetails { get; set; }

        public object Options { get; internal set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}