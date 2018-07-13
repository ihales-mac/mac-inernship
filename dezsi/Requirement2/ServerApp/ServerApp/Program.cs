using ServerApp.SocketNp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {


            ICommunication<Socket> server = new SynchronousSocketListener();
          
            server.StartListening();

            //ProgramThreaded.Run();
        }
    }
}

