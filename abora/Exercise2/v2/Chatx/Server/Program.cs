using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const string HOST = "localhost";
            const int PORT = 8080;
            
            LoginServiceServer loginService = new LoginServiceServer();
            TCPServer tCPServer = new TCPServer(HOST, PORT, loginService);
            MessageServiceServer messageService = new MessageServiceServer();


            tCPServer.addHandler("CHAT", delegate (Message request)
             {
                 Message message = new Message();
                 foreach(Tuple<string, TcpClient> client in tCPServer.ActiveClients)
                 {
                     request.WriteTo(client.Item2.GetStream());
                 }

                 message.Header = "OK2";
                 message.Body = request.Body;
                 return message;
             });

            tCPServer.addHandler("IS_AUTHENTICATED", delegate (Message response)
             {
                 TcpClient client = tCPServer.GetTCPClient(response.Body); 
                 response.Header = "OK";
                 response.WriteTo(client.GetStream());

                 return response;
             });


            tCPServer.addHandler("AUTHENTICATION_FAILED", delegate (Message response)
            {
                TcpClient client = tCPServer.GetTCPClient(response.Body);
                response.WriteTo(client.GetStream());

                return response;
            });

            tCPServer.Start();
        }
    }
}
