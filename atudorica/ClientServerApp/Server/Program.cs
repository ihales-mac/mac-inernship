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
        private static TcpClient clientSocket;
        private static TcpListener serverSocket;
        private static List<ClientHandler> clients = new List<ClientHandler>();

        private static void ClientConnection()
        {
            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();

                Console.WriteLine(" >> " + "Client No:" + (clients.Count + 1).ToString() + " started!");
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.ClientListRequested += ClientHandler_clientListRequested;
                clientHandler.ClientDisconnect += ClientHandler_clientDisconnect;
                clientHandler.StartClient(clientSocket, (clients.Count+1).ToString());
                clients.Add(clientHandler);
            }
            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
        }

        private static void ClientHandler_clientDisconnect(object sender, Utils.EventArguments.ClientDisconnectEventArgs e)
        {
            clients.Remove((ClientHandler)sender);
        }

        private static void ClientHandler_clientListRequested(object sender, Utils.EventArguments.ClientListRequestedEventArgs e)
        {
            string result = null;
            foreach (ClientHandler c in clients)
                result+=((c.Username+"~"));
            ((ClientHandler)sender).SendMessageToClient(result);
        }

        static void Main(string[] args)
        {

            serverSocket = new TcpListener(11100);
            clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");
            Thread clientConnectionThread = new Thread(ClientConnection);
            clientConnectionThread.Start();
            while (true)
            {
                string command = Console.ReadLine();
                if (command == "list clients")
                foreach (ClientHandler c in clients)
                    Console.WriteLine(c.Username);
            }

        }
    }
}
