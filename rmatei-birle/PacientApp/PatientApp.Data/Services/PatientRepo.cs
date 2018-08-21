using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientApp.Data.Mappers;
using PatientApp.Data.Models.Command;
using PatientApp.Data.Models.Query;

namespace PatientApp.Data.Services
{
    public class PatientRepo : Contracts.IPatientRepo
    {
        private PatientMapper _mapper = new PatientMapper();

        public void Create(PatientC patient)
        {
            var pt = _mapper.Map(patient);
            Patient p = pt.Item1;
            PatientDetail pd = pt.Item2;
            p.PatientDetail = pd;

            using (var _dbContext = new Model1())
            {
                _dbContext.Patients.Add(p);
                _dbContext.SaveChanges();
            }

        }

        public List<PatientQ> Get()
        {
            List<Patient> lp;
            List<PatientDetail> lpd;

            using (var _dbContext = new Model1())
            {
                lp = _dbContext.Patients.Include(x => x.);
                lpd = _dbContext.PatientDetails.ToList();
            }
            List<PatientQ> patients = new List<PatientQ>();

            lp.ForEach(p =>
            {
                int dID = (int)p.details;
                lpd.ForEach(pd =>
                {
                    if (pd.ID == dID)
                    {
                        patients.Add(_mapper.Map(p, pd));
                    }
                });
            });
            return patients;
        }
    }
}
