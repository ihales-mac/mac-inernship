using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
    class Program
    {
        private static TcpClient _clientSocket;
        private static TcpListener _serverSocket;
        private static readonly List<ClientHandler> Clients = new List<ClientHandler>();
        private  static readonly char splitChar='~';
        private static void ClientConnection()
        {
            while (true)
            {
                _clientSocket = _serverSocket.AcceptTcpClient();

                Console.WriteLine(" >> " + "Client No:" + (Clients.Count + 1).ToString() + " started!");
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.ClientListRequested += ClientHandler_clientListRequested;
                clientHandler.ClientDisconnect += ClientHandler_clientDisconnect;
                clientHandler.ClientToClientMessage += ClientHandler_clientToClientMessage;
                clientHandler.ClientLoggedIn += ClientHandler_ClientLoggedIn;
                clientHandler.StartClient(_clientSocket, (Clients.Count+1).ToString());
                Clients.Add(clientHandler);
            }
            _clientSocket.Close();
            _serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
        }


        public static void NotifyUsersClientListChanged(string exception)
        {
            string list = null;
            foreach (ClientHandler c in Clients)
                list += ((c.Username + "~"));
            if (list != null)
            {
                list = list.Substring(0, list.Length - 1);
                foreach (ClientHandler c in Clients)
                    if (exception == null ||  c.Username != exception)
                        c.SendMessageToClient("lc~" + list);
            }
        }

        private static void ClientHandler_clientDisconnect(object sender, Utils.EventArguments.ClientDisconnectEventArgs e)
        {
            Clients.Remove((ClientHandler)sender);
            NotifyUsersClientListChanged(null);
        }

        private static void ClientHandler_ClientLoggedIn(object sender, Utils.EventArguments.ClientLoggedInEventArgs e)
        { 
            NotifyUsersClientListChanged(e.Username);
        }

        private static void ClientHandler_clientListRequested(object sender, Utils.EventArguments.ClientListRequestedEventArgs e)
        {
            string result = null;
            foreach (ClientHandler c in Clients)
                result+=((c.Username+"~"));
            result = result.Substring(0, result.Length - 1);
            ((ClientHandler)sender).SendMessageToClient(result);
        }

        private static void ClientHandler_clientToClientMessage(object sender,
            Utils.EventArguments.ClientToClientMessageEventArgs e)
        {
            ClientHandler client = Clients.Find(o=> o.Username == e.receiver);
            client.SendMessageToClient("m~"+e.sender+splitChar+e.message);
        }

        static void Main(string[] args)
        {

            _serverSocket = new TcpListener(11100);
            _clientSocket = default(TcpClient);
            _serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");
            Thread clientConnectionThread = new Thread(ClientConnection);
            clientConnectionThread.Start();
            while (true)
            {
                string command = Console.ReadLine();
                if (command == "list clients")
                foreach (ClientHandler c in Clients)
                    Console.WriteLine(c.Username);
            }
        }
    }
}
