<<<<<<< HEAD
﻿
using Patients.BusinessLogic.Services;
using Patients.DataAccessLayer.Models;
=======
﻿using Patients.Models;
using Patients.Services;
>>>>>>> 4e1ba54c757d4af9d344447da1a281360b5966c9
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
    public class PatientsController : ApiController
    {
        private IPatientService _patientService;

        public PatientsController(IPatientService serv) { this._patientService = serv; }
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