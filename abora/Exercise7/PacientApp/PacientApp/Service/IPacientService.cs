using System.Collections.Generic;
using PacientApp.Models;

namespace PacientApp.Service
{
    public interface IPacientService
    {
        Pacient AddPacient(Pacient pacient);
        IEnumerable<Pacient> GetAllPacients();
        Pacient GetPacient(int id);
        Pacient RemovePacient(int id);
        Pacient UpdatePacient(int id, Pacient pacient);
    }
}