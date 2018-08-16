using Patients.Models;
using Patients.Services;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Unity.Attributes;

namespace Patients.Controllers
{
    public class PatientsController : ApiController, IPatientsController
    {
        private IPatientService _patientService = new PatientService();
        //private IPatientService patientService = new PatientService();
       

        public PatientsController(IPatientService patientService) {
            this._patientService = patientService;
        }
   
        /*
        public PatientsController()
        {
            _patientService = patientService;
        }
        */
        // GET api/patients
        [SwaggerOperation("GetAll")]
        public IEnumerable<Patient> Get()
        {
            return _patientService.GetPatients();

        }

        // GET api/patients/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Patient Get(int id)
        {
            return _patientService.GetPatient(id);
        }

        // POST api/patients
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public void Post([FromBody]Patient value)
        {

            _patientService.Add(value);
        }

        // PUT api/patients/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Put(int id, [FromBody]Patient value)
       
        {
            _patientService.Put(id, value);
        }

        // DELETE api/patients/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(int id)
        {
            _patientService.Delete(id);
        }
    }
}