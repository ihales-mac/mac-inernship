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


        IDictionary<string,Func<Message,Message>> handlers = new Dictionary<string, Func<Message, Message>>();

        public TCPServer()
        {

        }

        public TCPServer(string hOST, int pORT)
        {
            this.hOST = hOST;
            this.pORT = pORT;
        }

        public void addHandler(string methodName, Func<Message, Message> function)
        {
            handlers.Add(methodName, function);
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
                
                Console.WriteLine("Client connected...");
                Thread thread = new Thread(() => HandleRequest(tcpClient));
                thread.Start();
            }

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
                if (response.Header.Equals("OK"))
                {
                    ActiveClients.Add(Tuple.Create(response.Body, tcpClient));
                    Console.WriteLine("Client was successfully");
                    response.WriteTo(tcpClient.GetStream());
                }
                
                
            }    
 
        
        }
    }
}
