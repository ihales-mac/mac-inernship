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
            TCPServer tCPServer = new TCPServer(HOST, PORT);
            LoginServiceServer loginService = new LoginServiceServer();
            MessageServiceServer messageService = new MessageServiceServer();

            tCPServer.addHandler("CHECK_LOGIN", delegate(Message request){
                Console.WriteLine("Handler called");
                Message response = new Message();
                string username = request.Body.Split(';')[0];
                Console.WriteLine("username : " + username);
               
                string password = request.Body.Split(';')[1];
                Console.WriteLine("password : " + password);

                if (loginService.CheckLogin(username, password))
                {
                    response.Header = "OK";
                    response.Body = username;
                }
                else
                {
                    response.Header = "404";
                    response.Body = "Username of password do not match";
                }

                return response;

            });

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

            tCPServer.Start();
        }
    }
}
