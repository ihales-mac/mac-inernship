using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const string HOST = "localhost";
            const int PORT = 8080;
            
            LoginServiceServer loginService = new LoginServiceServer();
            TCPServer tCPServer = new TCPServer(HOST, PORT);


            tCPServer.addHandler("CHAT", delegate (Message request)
             {
                 //save message into db

                 return null;
             });

            tCPServer.addHandler("CHECK_LOGIN", delegate (Message request)
            {
                string username = request.Body.Split('?')[0];
                Console.WriteLine("username : " + username);

                string password = request.Body.Split('?')[1];
                Console.WriteLine("password : " + password);
                Message response = new Message();
                if (loginService.CheckLogin(username, password))
                {
                    response.Header = "OK";
                    response.Body = username;
                }
                else
                {
                    response.Header = "404";
                    response.Body = "Username or password is invalid";
                }

                return response;

            });

            tCPServer.Start();
        }
    }
}
