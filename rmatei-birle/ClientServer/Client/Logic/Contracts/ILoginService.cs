using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Contracts
{
    public interface ILoginService
    {
        string Login(string Username, string Password, string IP, string Port);
    }
}
