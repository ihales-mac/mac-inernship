using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;

namespace Server
{
    class TCPServer
    {
        private string hOST;
        private int pORT;
        private TcpListener TcpListener;
        public List<Tuple<string, string, TcpClient>> ActiveClients = new List<Tuple<string, string, TcpClient>>();
        //private LoginServiceServer LoginServiceServer;
        private EncryptionRSA RSA;
        private string PublicKeyToSend;
        private string PrivateKey;
        IDictionary<string, Func<Message, Message>> handlers = new Dictionary<string, Func<Message, Message>>();

        public TCPServer(string hOST, int pORT)
        {

            this.hOST = hOST;
            this.pORT = pORT;
            RSA = new EncryptionRSA();
            PublicKeyToSend = RSA.GetPublicKey();
            PrivateKey = RSA.GetPrivateKey();
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
                    return client.Item3;
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
                //hand shake
                //Server send public key to client
                SendPublicKey(tcpClient);
                //Server read public key from client
                string publicKeyFromClient = ReadPublicKey(tcpClient);
                //handle login
                Message request = new Message();
                var rsa = new EncryptionRSA();
                rsa.SetPrivateKey(PrivateKey);
                request.ReadFrom(tcpClient.GetStream(), rsa);

                Message response = handlers[request.Header].Invoke(request);

                if (response.Header.Equals("OK"))
                {
                    Console.WriteLine("Client authenticated is connected...");
                    Console.WriteLine("server sends head: " + response.Header + " body " + response.Body);
                    var rsa2 = new EncryptionRSA();
                    rsa2.SetPublicKey(publicKeyFromClient);
                    response.WriteTo(tcpClient.GetStream(),rsa2);
                    RegisterClient(response.Body, publicKeyFromClient, tcpClient);
                    Thread thread = new Thread(() => HandleRequest(tcpClient));
                    thread.Start();
                }
                else
                {
                    var rsa2 = new EncryptionRSA();
                    rsa2.SetPublicKey(publicKeyFromClient);

                    response.WriteTo(tcpClient.GetStream(),rsa2);
                    Console.WriteLine("Unable to log in");
                    
                }
                

            }

        }

        private string ReadPublicKey(TcpClient tcpClient)
        {
            Message msgKey = new Message();
            msgKey.ReadFrom(tcpClient.GetStream());
            Console.WriteLine("Server receive from client Public key " + msgKey.Body);
            return msgKey.Body;
        }

        private void SendPublicKey(TcpClient tcpClient)
        {
            Message msgKey = new Message();
            msgKey.Header = "PUBLIC_KEY";
            msgKey.Body = PublicKeyToSend;
            Console.WriteLine("Server send key: " + msgKey.Body);
            msgKey.WriteTo(tcpClient.GetStream());
        }

        private void RegisterClient(string username, string key, TcpClient tcpClient)
        {

            ActiveClients.Add(Tuple.Create(username, key, tcpClient));
        }

        private void HandleRequest(TcpClient tcpClient)
        {

            while (true)
            {
                Thread.Sleep(10);
                Message message = new Message();
                var rsa = new EncryptionRSA();
                rsa.SetPrivateKey(PrivateKey);
                try
                {
                    message.ReadFrom(tcpClient.GetStream(), rsa);
                    Console.WriteLine("Client send: body" + message.Body + "header" + message.Header);
                    handlers[message.Header].Invoke(message);
                    broadcast(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Thread stop listening");
                    break;
                    
                }


            }


        }

        private void broadcast(Message request)
        {
            foreach (Tuple<string, string, TcpClient> client in ActiveClients)
            {
                string publicKey = client.Item2;
                var rsa = new EncryptionRSA();
                rsa.SetPublicKey(publicKey);

                request.WriteTo(client.Item3.GetStream(), rsa);
            }
        }
    }
}
