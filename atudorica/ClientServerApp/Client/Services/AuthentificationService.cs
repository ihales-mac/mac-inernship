using Client.Connection.ConnectionServices;
using Client.Connection.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    class AuthentificationService
    {
        public IConnectionService cs;
        public AuthentificationService()
        {
            cs = new SocketConnectionService();
        }

        public void CloseConnection()
        {
            cs.CloseConnection();
        }

        public string Authenticate(string username, string password)
        {
            return cs.Authenticate( username,  password);
        }
    }
}
