using Client.Communication.Contracts;
using Client.EventArguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Communication.Services
{
    public class SocketServices : ICommunication
    {
        private static SocketServices instance;

        private SocketServices() { }

        public static ICommunication Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SocketServices();
                }
                return instance;
            }
        }

        public event IncomingMessageEventHandler IncomingMessage;

        private TcpClient client;
        private StreamReader Reader;
        private StreamWriter Writer;
        private Thread ListeningThread;
        private string IdentificationCode;

        private bool IsConnected = false;
        private bool IsListening = false;

        public string Connect(string IP, string Port)
        {
            int port;
            if (!Int32.TryParse(Port, out port))
            {
                return "Error: Invalid port";
            }

            try
            {
                IPAddress.Parse(IP);
            }
            catch (FormatException)
            {
                return "Error: Invalid IP format";
            }


            if (!IsConnected)
            {
                client = new TcpClient();
                client.Connect(IP, port);
                Reader = new StreamReader(client.GetStream(), Encoding.ASCII);
                Writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
                IsConnected = true;

                //get and send keys;
            }

            return "connected";
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                Reader.Close();
                Reader.Dispose();
                Writer.Close();
                Writer.Dispose();
                client.GetStream().Close();
                client.Close();
                client.Dispose();
                IsConnected = false;
            }
        }

        public void ListenContinuously()
        {
            if (!IsListening)
            {
                IsListening = true;
                //do thread here
                ListeningThread = new Thread(() =>
                {
                    while (IsListening && IsConnected)
                    {
                        string FromServer = Reader.ReadLine();
                        //to event stuff here;
                        if(FromServer != null)
                        {
                            //raise event
                            OnIncomingMessage(new IncomingMessageEventArgs(FromServer));
                        }
                    }
                });

                ListeningThread.Start();
            }
        }
        
        protected void OnIncomingMessage(IncomingMessageEventArgs e)
        {
            if (IncomingMessage != null)
                IncomingMessage(this, e);
        }

        public string ListenOnce()
        {
            if (IsListening)
            {
                IsListening = false;
                if(ListeningThread != null)
                {
                    ListeningThread.Abort();
                    ListeningThread = null;
                }
            }
            
            return Reader.ReadLine();
        }

        public void StopListeningContinuously()
        {
            if (IsListening)
            {
                IsListening = false;
                if (ListeningThread != null)
                {
                    ListeningThread.Abort();
                    ListeningThread = null;
                }
            }
        }

        public void SendMessage(string message)
        {
            //encrypt message
            //add identification code
            if(this.IdentificationCode != null)
            {
                Writer.WriteLine("$$IC=" + IdentificationCode + "" + message);
            }
            Writer.WriteLine(message);
            Writer.Flush();
        }

        public void SetIC(string IC)
        {
            this.IdentificationCode = IC;
        }
    }
}
