namespace PatientApp.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PatientDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PatientDetail()
        {
            Patients = new HashSet<Patient>();
        }

        public int ID { get; set; }

        [StringLength(200)]
        public string address { get; set; }

        [StringLength(200)]
        public string detail1 { get; set; }

        [StringLength(200)]
        public string detail2 { get; set; }

        [StringLength(200)]
        public string detail3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
