using Patients.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Patients.Data
{
    public class PatientContext : DbContext
    {
        public PatientContext() : base(@"data source = desktop - 4koh9rj; initial catalog = Patients; integrated security = True; multipleactiveresultsets=True;application name = EntityFramework") { }
        public DbSet<Patient> Patients { get; set; }

    }
}
