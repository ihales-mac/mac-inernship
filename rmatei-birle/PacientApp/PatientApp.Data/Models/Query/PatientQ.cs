using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Data.Models.Query
{
    public class PatientQ
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Detail3 { get; set; }

        public override string ToString()
        {
            return "{\"ID\":" + this.ID 
                + ",\"FirstName\":\"" + this.FirstName 
                + "\",\"LastName\":\"" + this.LastName 
                + "\",\"Address\":\"" + this.Address 
                + "\",\"Detail1\":\"" + this.Detail1
                + "\",\"Detail2\":\"" + this.Detail2
                + "\",\"Detail3\":\"" + this.Detail3 
                + "\"}";
        }
    }
}
