using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace RainbowEF
{
    class RainbowContext : DbContext
    {
        public RainbowContext() : base("name=RainbowDBString")
        {
            Database.SetInitializer<RainbowContext>(new DropCreateDatabaseIfModelChanges<RainbowContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RainbowItem> RainbowItems { get; set; }
    }
}
