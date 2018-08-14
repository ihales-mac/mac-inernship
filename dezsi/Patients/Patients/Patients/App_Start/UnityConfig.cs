using Patients.Controllers;
using Patients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Unity;

namespace Patients
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType(typeof(IPatientService), typeof(PatientService),"patientService",null);
            container.Resolve<PatientService>();
            container.Resolve<PatientsController>();

      
        }
    }
}