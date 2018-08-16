using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientApp.Data.Models.Command;
using PatientApp.Data.Models.Query;
using PatientApp.Data.Contracts;

namespace PatientApp.Logic.Services
{
    public class PatientLogic : Contracts.IPatientLogic
    {
        IPatientRepo _pr;

        public PatientLogic(IPatientRepo patientRepo)
        {
            _pr = patientRepo;
        }

        public void AddPatient(PatientC patient)
        {
            //validate patient
            _pr.Create(patient);
        }

        public List<PatientQ> GetAll()
        {
            return _pr.Get();
        }
    }
}
