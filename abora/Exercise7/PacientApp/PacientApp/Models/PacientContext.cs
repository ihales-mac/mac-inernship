using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PacientApp.Models
{
    public class PacientContext: DbContext
    {
        public PacientContext():base("name=PacientDBConnectionString")
        {
            Database.SetInitializer<PacientContext>(new CreateDatabaseIfNotExists<PacientContext>());
        }
        public DbSet<Pacient> Pacients { get; set; }
    }
}