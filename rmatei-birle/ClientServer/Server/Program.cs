using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static List<User> AllUsers;

        private static bool IsRunning = true;
        private static Thread listeningThread;
        private static string IP;
        private static int Port;
        private static TcpListener listener;
        public static List<ClientHandler> Handlers = new List<ClientHandler>();
        private static int count = 0;

        private static bool SymmetricEncryption { get; set; }
        private static bool AsymmetricEncryption { get; set; }


        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            PopulateUsers();
            InterpretArgs(args);
            Console.WriteLine("IP: " + IP);
            Console.WriteLine("Port: " + Port);

            try
            {
                listener = new TcpListener(IPAddress.Parse(IP), Port);
                listener.Start();

                Console.WriteLine("The local End point is  :" + listener.LocalEndpoint);
                Console.WriteLine("");

                listeningThread = new Thread(AcceptConnections);
                listeningThread.Start();

                while (IsRunning)
                {
                    string command = Console.ReadLine();

                    string[] SplitCommand = command.Split(' ');


                    switch (SplitCommand[0].ToLower())
                    {
                        case "quit":
                            IsRunning = false;
                            listener.Stop();
                            break;
                        case "getconn":
                            Console.WriteLine("IP: " + IP);
                            Console.WriteLine("Port: " + Port);
                            Console.WriteLine("");
                            break;
                        default:
                            PrintHelp();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured. Message: " + e.ToString());
            }

            return;
        }

        public static User GetUser(string username, string password)
        {
            foreach (User usr in AllUsers)
            {
                if (usr.Equals(username, password))
                {
                    return usr;
                }
            }

            return null;
        }

        public static ClientHandler GetHandler(string username)
        {
            foreach (ClientHandler ch in Handlers)
            {
                if(ch.GetUsername() == username)
                {
                    return ch;
                }
            }
            return null;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("quit - Quit server and disconnect clients");
            Console.WriteLine("getconn - Get connection details");
            Console.WriteLine("");
        }

        private static void AcceptConnections()
        {
            Console.WriteLine("Accepting connections");
            Console.WriteLine("");

            while (IsRunning)
            {
                TcpClient IncomingClient;
                try
                {
                    IncomingClient = listener.AcceptTcpClient();
                    ClientHandler handler = new ClientHandler(IncomingClient);
                    handler.ClientHandlerStop += Handler_ClientHandlerStop;
                    handler.ID = count;
                    count++;
                    handler.Start();
                    Handlers.Add(handler);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        Console.WriteLine("Quitting...");
                    }
                    else
                    {
                        Console.WriteLine("Error occured: " + e.ToString());
                    }
                }
            }

            foreach (ClientHandler ch in Handlers)
            {
                ch.Stop();
            }
        }

        private static void Handler_ClientHandlerStop(object sender, EventArgs e)
        {
            Handlers.Remove((ClientHandler)sender);
            Broadcast("$$CMD$$ACTION=REMOVE$$VALUE=" + ((ClientHandler)sender).GetUsername());
        }

        private static void InterpretArgs(string[] args)
        {
            if (args.Contains("-a"))
            {
                int PosAdd = Array.FindIndex(args, a => a == "-a");
                IP = args[PosAdd + 1];
            }
            else
            {
                IP = "127.0.0.1";
            }

            if (args.Contains("-p"))
            {
                int PosPort = Array.FindIndex(args, a => a == "-p");
                if (!Int32.TryParse(args[PosPort + 1], out Port))
                {
                    Port = 8000;
                }
            }
            else
            {
                Port = 8000;
            }
        }

        public static void Broadcast(string message)
        {
            foreach (ClientHandler ch in Handlers)
            {
                ch.SendMessage(message);
            }
        }

        public static void Broadcast(string message, string ignore)
        {
            foreach (ClientHandler ch in Handlers)
            {
                if (ch.GetUsername() != ignore)
                {
                    ch.SendMessage(message);
                }
            }
        }

        private static void ModifyEncryption(bool symmetric, bool set)
        {
            if (symmetric)
            {
                SymmetricEncryption = set;

                foreach (ClientHandler ch in Handlers)
                {
                    ch.SymmetricEncryption = set;
                }
            }
            else
            {
                AsymmetricEncryption = set;

                foreach (ClientHandler ch in Handlers)
                {
                    ch.AsymmetricEncryption = set;
                }
            }
        }

        private static void PopulateUsers()
        {
            AllUsers = new List<User>();
            AllUsers.Add(new User("user1", "user"));
            AllUsers.Add(new User("user2", "user"));
            AllUsers.Add(new User("user3", "user"));
            AllUsers.Add(new User("user4", "user"));
            AllUsers.Add(new User("user5", "user"));
            AllUsers.Add(new User("user6", "user"));
        }

    }
}
