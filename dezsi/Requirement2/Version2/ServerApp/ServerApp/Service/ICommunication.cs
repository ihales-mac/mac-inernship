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

        void SendMessage();
        string Users();
        string Messages();
        string ResolveLogin(String username, String Password);
        void StartListening();
        
 
      
    }
}
