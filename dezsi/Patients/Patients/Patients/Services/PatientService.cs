using Patients.Data;
using Patients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patients.Services
{
    public class PatientService : IPatientService
    {
        public IEnumerable<Patient> GetPatients() {
            using (PatientContext ctx = new PatientContext()) {

                return ctx.Patients.AsEnumerable();

            }
        }

    }
}