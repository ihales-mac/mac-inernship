using System;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static Service service = new Service();
        static void Main(string[] args)
        {
            System.Threading.Thread myThread;
         
           
           // myThread = new System.Threading.Thread(new System.Threading.ThreadStart();
            while (true)
            {
                TcpClient client = service.newClient();
                HandleClient handleClient = new HandleClient();
                handleClient.startClient(client, "1",service.getClients());
               // service.recieve(client);
            }


                Console.ReadLine();
        }
    }
}
