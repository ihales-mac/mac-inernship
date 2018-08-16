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
        
    
        private List<Tuple<string, TcpClient>> ActiveClients = new List<Tuple<string, TcpClient>>();
        private List<Tuple<string, string>> Auth = new List<Tuple<string, string>>();
        TcpListener TcpListener;

        public TCPServer()
        {
            //create a mock user and password list
            Auth.Add(Tuple.Create("andrei", "1"));
            Auth.Add(Tuple.Create("goe", "2"));
            Auth.Add(Tuple.Create("mara", "3"));

            //start the server
            IPAddress iPAddress = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener = new TcpListener(iPAddress, 8080);

            try
            {
                TcpListener.Start();
                Console.WriteLine("Server started...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void broadcast(string msg, string name)
        {
            foreach (Tuple<string, TcpClient> t in ActiveClients)
            {
                TcpClient tcpClient = t.Item2;
                NetworkStream stream = tcpClient.GetStream();
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                int count = Encoding.ASCII.GetByteCount(msg);
                stream.Write(bytes, 0, count);
                stream.Flush();
            }
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Listening...");
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                bool isAuthenticate = false;

                byte[] bytes = new byte[1024];
                string clientCredentials = null;

                //read the name of the client
                NetworkStream networkStream = tcpClient.GetStream();
                networkStream.Read(bytes, 0, 1024);
                clientCredentials = Encoding.ASCII.GetString(bytes);

                int size = clientCredentials.IndexOf('\0');
                clientCredentials = clientCredentials.Substring(0, size);

                string username = clientCredentials.Split(';')[0];
                string password = clientCredentials.Split(';')[1];
                foreach (Tuple<string, string> userPass in Auth)
                {
                    if (userPass.Item1.Equals(username) && userPass.Item2.Equals(password))
                    {
                        isAuthenticate = true;
                    }
                }
                if (isAuthenticate)
                {
                    ActiveClients.Add(Tuple.Create(username, tcpClient));
                    Console.WriteLine(username + " joined the chat room");

                    //Write message back
                    WriteMsgToClient("success", networkStream);

                    //notify the rest that a user join the chat room
                    broadcast(username + " join the chat room", username);
                    Thread thread = new Thread(() => GetClientMessages(tcpClient,username));
                    thread.Start();
                }
                else
                {
                    //Write message back
                    WriteMsgToClient("failed", networkStream);
                    Console.WriteLine("Authentication failed");
                }

            }

        }

        private void GetClientMessages(TcpClient tcpClient,string username)
        {
            while (true)
            {
                Thread.Sleep(1);
                string msg = username + ": ";
                msg  += this.ReadFromClient(tcpClient.GetStream());
                
                broadcast(msg, username);
            }
        }

        private string ReadFromClient(NetworkStream networkStream)
        {
            byte[] bytes = new byte[1024];
            string msg = null;
            networkStream.Read(bytes, 0, 1024);
            msg = Encoding.ASCII.GetString(bytes);

            int size = msg.IndexOf('\0');
            msg = msg.Substring(0, size);

            return msg;
        }

        private void WriteMsgToClient(string msg,NetworkStream networkStream)
        {
            byte[] response = Encoding.ASCII.GetBytes(msg);
            int count = Encoding.ASCII.GetByteCount(msg);
            networkStream.Write(response, 0, count);
            networkStream.Flush();
        }
    }
}

