using PacientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PacientApp.Service
{
    public class PacientService : IPacientService
    {
        public Pacient AddPacient(Pacient pacient)
        {
            using (PacientContext ctx = new PacientContext())
            {
                ctx.Pacients.Add(pacient);
                ctx.SaveChanges();

                return pacient;
            }
        }

        public Pacient GetPacient(int id)
        {
            using (PacientContext ctx = new PacientContext())
            {
                return ctx.Pacients.Where(p => p.PacientId == id).First();
            }
        }

        public IEnumerable<Pacient> GetAllPacients()
        {
            using (PacientContext ctx = new PacientContext())
            {
                return ctx.Pacients.ToList();
            }
        }

        public Pacient RemovePacient(int id)
        {
            using(PacientContext ctx = new PacientContext())
            {
                //trow exception if only one
                Pacient pacient = ctx.Pacients.Where(p => p.PacientId == id).First();
                ctx.Pacients.Remove(pacient);
                ctx.SaveChanges();

                return pacient;
            }
        }
        public Pacient UpdatePacient(int id, Pacient pacient)
        {
            using(PacientContext ctx = new PacientContext())
            {
                Pacient oldPacient = ctx.Pacients.Where(p => p.PacientId == pacient.PacientId).First();
                oldPacient.FirstName = pacient.FirstName;
                oldPacient.LastName = pacient.LastName;
                oldPacient.Ssn = pacient.Ssn;

                ctx.SaveChanges();
                return oldPacient;
            }
        }
    }
}