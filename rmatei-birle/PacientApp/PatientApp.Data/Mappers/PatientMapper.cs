using PatientApp.Data.Models.Command;
using PatientApp.Data.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Data.Mappers
{
    class PatientMapper
    {
        public PatientQ Map(Patient p, PatientDetail pd)
        {
            if(p == null)
            {
                p = new Patient();
            }

            if(pd == null)
            {
                pd = new PatientDetail();
            }

            PatientQ pat = new PatientQ();
            pat.ID = p.ID;
            pat.FirstName = p.firstname;
            pat.LastName = p.lastname;
            pat.Address = pd.address;
            pat.Detail1 = pd.detail1;
            pat.Detail2 = pd.detail2;
            pat.Detail3 = pd.detail3;

            return pat;
        }

        public Tuple<Patient, PatientDetail> Map(PatientC pc)
        {
            PatientDetail pd = new PatientDetail();
            Patient p = new Patient();

            if(pc == null)
            {
                pc = new PatientC();
            }

            p.firstname = pc.FirstName;
            p.lastname = pc.LastName;
            pd.address = pc.Address;
            pd.detail1 = pc.Detail1;
            pd.detail2 = pc.Detail2;
            pd.detail3 = pc.Detail3;

            return new Tuple<Patient, PatientDetail>(p, pd);
        }
    }
}
