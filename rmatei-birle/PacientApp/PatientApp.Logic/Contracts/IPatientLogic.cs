using PatientApp.Data.Models.Command;
using PatientApp.Data.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Logic.Contracts
{
    public interface IPatientLogic
    {
        List<PatientQ> GetAll();
        void AddPatient(PatientC patient);
    }
}
