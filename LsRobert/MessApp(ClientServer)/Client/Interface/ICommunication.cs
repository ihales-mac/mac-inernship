using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interface
{
   public  interface ICommunication
    {
        void SendMessage(string message);
        String getMessage();
        TcpClient getClient();
       
    }
}
