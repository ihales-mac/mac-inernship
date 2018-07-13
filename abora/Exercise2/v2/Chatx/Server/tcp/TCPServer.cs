using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Server
{
    class TCPServer
    {
        private string hOST;
        private int pORT;
        private TcpListener TcpListener;
        public List<Tuple<string, TcpClient>> ActiveClients = new List<Tuple<string, TcpClient>>();
        private LoginServiceServer LoginServiceServer;

        IDictionary<string,Func<Message,Message>> handlers = new Dictionary<string, Func<Message, Message>>();

        public TCPServer(string hOST, int pORT, LoginServiceServer loginServiceServer)
        {

            this.hOST = hOST;
            this.pORT = pORT;
            LoginServiceServer = loginServiceServer;
        }

        public void addHandler(string methodName, Func<Message, Message> function)
        {
            handlers.Add(methodName, function);
        }

        public TcpClient GetTCPClient(string username)
        {

            foreach (var client in ActiveClients)
            {
                if (client.Item1.Equals(username))
                {
                    return client.Item2;
                }
            }
            return null;
        }


        public void Start()
        {
            //start the server
            IPAddress iPAddress = Dns.GetHostEntry(hOST).AddressList[0];
            TcpListener = new TcpListener(iPAddress, pORT);
            
            
            try
            {
                TcpListener.Start();
                Console.WriteLine("Server started...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                //handle login

                Message message = new Message();
                message.ReadFrom(tcpClient.GetStream());
                string username = message.Body.Split(';')[0];
                Console.WriteLine("username : " + username);

                string password = message.Body.Split(';')[1];
                Console.WriteLine("password : " + password);

                if(LoginServiceServer.CheckLogin(username, password)){
                    Console.WriteLine("Client authenticated is connected...");
                    RegisterClient(username, tcpClient);
                    Message response = new Message();

                    response.Body = username;
                    handlers["IS_AUTHENTICATED"].Invoke(response);
                    Thread thread = new Thread(() => HandleRequest(tcpClient));
                    thread.Start();
                }
                else
                {
                    Message response = new Message();
                    response.Header = "404";
                    response.Body = "Username of password is not valid";
                    handlers["AUTHENTICATION_FAILED"].Invoke(response);
                }
            }

        }

        private void RegisterClient(string username, TcpClient tcpClient)
        {
            ActiveClients.Add(Tuple.Create(username, tcpClient));
        }

        private void HandleRequest(TcpClient tcpClient)
        {
            while (true)
            {
                Thread.Sleep(10);
                Message message = new Message();
                message.ReadFrom(tcpClient.GetStream());
                Console.WriteLine("Client send: body" + message.Body + "header" + message.Header);
                Message response = handlers[message.Header].Invoke(message);
                
            }    
 
        
        }
    }
}
