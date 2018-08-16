using PatientApp.Data.Models.Command;
using PatientApp.Data.Models.Query;
using PatientApp.Logic.Services;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PacientApp.Controllers
{
    public class PatientController : ApiController
    {
        IPatientLogic _pl;
        public PatientController(IPatientLogic patientLogic)
        {
            _pl = patientLogic;
        }

        // GET: api/Patient
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            List<PatientQ> patients = _pl.GetAll();
            List<string> response = new List<string>();
            patients.ForEach(p => { response.Add(p.ToString()); });
            return response;
        }

        // POST: api/Patient
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public void Post([FromBody]PatientC patient)
        {
            _pl.AddPatient(patient);
        }
    }
}
