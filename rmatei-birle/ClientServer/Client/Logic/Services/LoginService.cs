using Client.Communication.Contracts;
using Client.Communication.Services;
using Client.Logic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.EventArguments;

namespace Client.Logic.Services
{
    class LoginService : ILoginService
    {
        private readonly ICommunication _communication;
        public LoginService()
        {
            _communication = SocketServices.Instance;
            string response = _communication.Connect("127.0.0.1", "8000");
            _communication.ListenContinuously();
        }

        public void Login(string username, string password, string ip, string port)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("$$LOGIN$$UN=");
            sb.Append(username);
            sb.Append("$$PW=");
            sb.Append(password);

            _communication.SendMessage(sb.ToString());
        }
    }
}
