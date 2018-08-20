using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PacientApp.Models;
using PacientApp.Service;
using Swashbuckle.Swagger.Annotations;

namespace PacientApp.Controllers
{
    public class PacientController : ApiController
    {
        private IPacientService _pacientService;

        public PacientController(IPacientService pacientService)
        {
            this._pacientService = pacientService;
        }
        // GET api/values
        [SwaggerOperation("GetAll")]
        public IEnumerable<Pacient> Get()
        {
            return this._pacientService.GetAllPacients();
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Pacient pacient = this._pacientService.GetPacient(id);
                return Request.CreateResponse(HttpStatusCode.OK, pacient);
            }catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Pacient with id = " + id + " not found");
            }
            
            
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post([FromBody]Pacient pacient)
        {
            try
            {
                using (PacientContext ctx = new PacientContext())
                {
                    Pacient newPacient = _pacientService.AddPacient(pacient);
                    //return 201 Created
                    var msg = Request.CreateResponse(HttpStatusCode.Created, pacient);
                    //included location 
                    msg.Headers.Location = new Uri(Request.RequestUri + pacient.PacientId.ToString());
                    return msg;
                }
            }catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // DELETE api/values/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Pacient pacient = this._pacientService.RemovePacient(id);
                var msg = Request.CreateResponse(HttpStatusCode.OK,pacient);
                return msg;

            }catch(System.ArgumentNullException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Pacient with id = " + id + " is not found");
            }catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            
        }

        // PUT api/values/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public HttpResponseMessage Put(int id, [FromBody]Pacient pacient)
        {
            try
            {
                Pacient newPacient = this._pacientService.UpdatePacient(id, pacient);
                return Request.CreateResponse(HttpStatusCode.OK, newPacient);

            }catch(System.ArgumentNullException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Pacient with id = " + id + " is not found");
            }
        }
    }
}
