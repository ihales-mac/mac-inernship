using Patients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patients.Services
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetPatients();
        Patient GetPatient(int id);
        void Add(Patient p);
        void Delete(int id);
        void Put(int id, Patient value);
    }
}
