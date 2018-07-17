using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server
{
    public class Service
    {

        Dictionary<String, String> listUser = new Dictionary<string, string>()
        {
            {"Robi","a" },
            {"Andrei","b" }
        };
        public List<String> users = new List<string>();

        int port;
         TcpListener listener;
        public   List<TcpClient> clients = new List<TcpClient>();
     //   public List<String> users = new List<string>();
        NetworkStream networkStream;

        public Service(int port = 5432)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
        }

        public  TcpClient newClient()
        {
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            return client;
        }

        public List<TcpClient> getClients()
        {
            return clients;
        }
   
    }
}

