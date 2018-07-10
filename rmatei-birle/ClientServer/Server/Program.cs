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
        static private bool IsRunning = true;
        static private Thread listeningThread;
        static private string IP;
        static private int Port;
        static private TcpListener listener;
        static private List<ClientHandler> Handlers = new List<ClientHandler>();

        static private bool SymmetricEncryption { get; set; }
        static private bool AsymmetricEncryption { get; set; }


        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
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
                        case "getencryption":
                            Console.WriteLine("Symmetric encryption: " + SymmetricEncryption);
                            Console.WriteLine("Asymmetric encryption: " + AsymmetricEncryption);
                            Console.WriteLine("");
                            break;
                        case "setsymmetric":
                            ModifyEncryption(true, true);
                            break;
                        case "clrsymmetric":
                            ModifyEncryption(true, false);
                            break;
                        case "setasymmetric":
                            ModifyEncryption(false, true);
                            break;
                        case "clrasymmetric":
                            ModifyEncryption(false, false);
                            break;
                        case "echo":
                            StringBuilder sb = new StringBuilder();
                            for (int i = 1; i < SplitCommand.Length; i++)
                            {
                                sb.Append(SplitCommand[i]);
                            }
                            SendAll(sb.ToString());
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

        private static void PrintHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("quit - Quit server and disconnect clients");
            Console.WriteLine("getconn - Get connection details");
            Console.WriteLine("getencryption - Get encryption settings");
            Console.WriteLine("setsymmetric - Turn on symmetric encryption");
            Console.WriteLine("clrsymmetric - Turn off symmetric encryption");
            Console.WriteLine("setasymmetric - Turn on asymmetric encryption");
            Console.WriteLine("clrasymmetric - Turn off asymmetric encryption");
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
                    handler.ID = Handlers.Count;
                    handler.Start();
                    Handlers.Add(handler);
                }
                catch (SocketException e)
                {
                    if(e.SocketErrorCode == SocketError.Interrupted)
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

        private static void SendAll(string message)
        {
            foreach (ClientHandler ch in Handlers)
            {
                ch.SendMessage("SERVER: " + message);
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
    }
}
