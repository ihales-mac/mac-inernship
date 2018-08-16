using PatientApp.Data.Models.Command;
using PatientApp.Data.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Data.Contracts
{
    public interface IPatientRepo
    {
        List<PatientQ> Get();
        void Create(PatientC patient);
    }
}

