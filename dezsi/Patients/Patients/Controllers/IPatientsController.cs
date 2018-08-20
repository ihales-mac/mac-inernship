using System.Collections.Generic;
using System.Web.Http;
using Patients.Models;

namespace Patients.Controllers
{
    public interface IPatientsController
    {
        void Delete(int id);
        IEnumerable<Patient> Get();
        Patient Get(int id);
        void Post([FromBody] Patient value);
        void Put(int id, [FromBody] Patient value);
    }
}