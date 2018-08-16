namespace PatientApp.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=PatientDB")
        {
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientDetail> PatientDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientDetail>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.PatientDetail)
                .HasForeignKey(e => e.details);
        }
    }
}
