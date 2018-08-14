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
    }
}
