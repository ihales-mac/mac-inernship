using CommonApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    interface ICommunication<T>
    {
        void SendMessage(T handler, String msg, Header type = Header.Unspecified);
        string ResolveLogin(String username, String Password);
        void StartListening();
        //Generic send method to be used
        void Send(T handler, string message);
       
       
      
    }
}
