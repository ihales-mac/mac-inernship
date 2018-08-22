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
        public void Add(Patient p)
        {
            using (PatientContext ctx = new PatientContext())
            {
                ctx.Patients.Add(p);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<Patient> GetPatients()
        {

            using (PatientContext ctx = new PatientContext())
            {
                return ctx.Patients.ToList<Patient>();


            }
            
        }

        public Patient GetPatient(int id)
        {
            using (PatientContext ctx = new PatientContext())
            {
                var query = from p in ctx.Patients
                            where p.id == id
                            select p;

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                var patient = query.Single();
                return patient;


            }
        }

        public void Delete(int id)
        {
            using (PatientContext ctx = new PatientContext())
            {
                var patient = ctx.Patients.First(p => p.id == id);
                ctx.Patients.Remove(patient);
                ctx.SaveChanges();
            }
        }

        public void Put(int id, Patient value)
        {
            using (PatientContext ctx = new PatientContext())
            {


                var patient = ctx.Patients.First(p => p.id == id); ;
                if (patient != null)
                {
                    patient.last_name = value.last_name;
                    patient.first_name = value.first_name;
                    patient.phone_no = value.phone_no;
                    patient.sex = value.sex;

                    ctx.SaveChanges();
                }
            }
        }
    }
}