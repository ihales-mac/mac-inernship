using Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Service
{
      class LoginService
    {
        public   ICommunication communication;
     
        public LoginService(ICommunication communication_)
        {
            this.communication = communication_;
        }
        public void Login(string username, string password)
        {
           
            String message = "";
            message += "Login" + " " + username + " " + password;
            String messageToSend = Common.Cryption.EncryptMessage(message);

            communication.SendMessage(messageToSend);
        }

        public Boolean CheckUsernamePassword()
        {
            if (communication.getMessage() == "true")
                return true;
            return false;
        }
       
    }
}
