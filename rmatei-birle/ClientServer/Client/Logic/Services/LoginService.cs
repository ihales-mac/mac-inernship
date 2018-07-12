using Client.Communication.Contracts;
using Client.Communication.Services;
using Client.Logic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Services
{
    class LoginService : ILoginService
    {
        ICommunication Communication;
        //public LoginService(ICommunication comm)
        //{
        //    this.Communication = comm;
        //}

        public LoginService()
        {
            this.Communication = SocketServices.Instance;
        }

        public string Login(string Username, string Password, string IP, string Port)
        {
            string response = Communication.Connect(IP, Port);

            if (response != "connected")
            {
                return response;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("$$LOGIN$$UN=");
            sb.Append(Username);
            sb.Append("$$PW=");
            sb.Append(Password);

            Communication.SendMessage(sb.ToString());

            response = Communication.ListenOnce();
            //$$REJECTED$$REASON=why_rejected
            //$$ACCEPTED$$IC=identification_code

            string[] SplitResponse = response.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);

            if (SplitResponse[0] == "REJECTED")
            {
                return SplitResponse[1].Split('=')[1];
            }

            if(SplitResponse[0] == "ACCEPTED")
            {
                string IC = SplitResponse[1].Split('=')[1];
                Communication.SetIC(IC);
                return "success";
            }

            return null;
        }
    }
}
