using Client.Interface;
using Client.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Client
{
     class ClientService
    {
        public  ICommunication communication;
        LoginService loginService;
        public ClientService(ICommunication communication_)
        {
            this.communication = communication_;
            loginService = new LoginService(communication);
        }
      

   

        public  Boolean login(String username,String password) { 
        
            loginService.Login(username, password);
            return loginService.CheckUsernamePassword();

          

           
        }
    }
}
