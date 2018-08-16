using Client.tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common;

namespace Client
{
    public class LoginService
    {
        private TCPClient _tCPClient;

        public LoginService(TCPClient tCPClient)
        {
            this._tCPClient = tCPClient;
        }

        public bool CheckLogin(string username, string password)
        {
            Message message = new Message();
            message.Header = "CHECK_LOGIN";
            message.Body = username + "?" + password ;

            return Task.Factory.StartNew(() => {
                 return _tCPClient.Send(message);
            }).Result;

        }


    }
}
