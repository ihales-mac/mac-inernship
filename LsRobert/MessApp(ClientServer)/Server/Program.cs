using System;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static Service service = new Service();
        static void Main(string[] args)
        {
          
         
           
           // myThread = new System.Threading.Thread(new System.Threading.ThreadStart();
            while (true)
            {
                TcpClient client = service.newClient();
                HandleClient handleClient = new HandleClient();
                handleClient.startClient(client,service.getClients());
               // service.recieve(client);
            }


              
        }
    }
}
