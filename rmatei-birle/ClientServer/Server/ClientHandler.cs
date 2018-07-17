using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public delegate void ClientHandlerStopEventHandler(object sender, EventArgs e);
    public class ClientHandler
    {
        public event ClientHandlerStopEventHandler ClientHandlerStop;
        private Thread thread { get; set; }
        private bool IsRunning = true;
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;

        public int ID { get; set; }
        private User CurrentUser;
        public bool SymmetricEncryption { get; set; }
        public bool AsymmetricEncryption { get; set; }

        public ClientHandler(object client)
        {
            SymmetricEncryption = false;
            AsymmetricEncryption = false;
            this.client = (TcpClient)client;
            writer = new StreamWriter(this.client.GetStream(), Encoding.ASCII);
            writer.AutoFlush = true;
            reader = new StreamReader(this.client.GetStream(), Encoding.ASCII);
        }

        public string GetUsername()
        {
            return CurrentUser.GetUsername();
        }

        private void Communicating()
        {
            while (IsRunning)
            {
                string Incoming = reader.ReadLine();
                if (Incoming != null)
                {
                    Console.WriteLine("Client " + this.ID + ":" + Incoming);

                    //incomming: 
                    //$$LOGIN$$UN=username$$PW=password
                    //$$IC=identification_code$$LOGOUT
                    //$$IC=identification_code&&CHAT=to_whom&&MSG=message

                    string[] IncomingSplit = Incoming.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (IncomingSplit[0] == "LOGIN")
                    {
                        //$$REJECTED$$REASON=why_rejected
                        //$$ACCEPTED$$IC=identification_code

                        string username = IncomingSplit[1].Split('=')[1];
                        string password = IncomingSplit[2].Split('=')[1];

                        CurrentUser = Program.GetUser(username, password);

                        if (CurrentUser == null)
                        {
                            this.SendMessage("$$REJECTED$$REASON=Incorrect username or password");
                        }
                        else
                        {
                            CurrentUser.IC = Guid.NewGuid().ToString();
                            this.SendMessage("$$ACCEPTED$$IC=" + CurrentUser.IC);
                            Program.Broadcast("$$CMD$$ACTION=ADD$$VALUE=" + this.GetUsername(), this.GetUsername());
                        }
                    }

                    if (IncomingSplit[0].Contains("IC"))
                    {
                        //$$IC=identification_code$$LOGOUT
                        //$$IC=identification_code&&CHAT=to_whom&&MSG=message
                        string ic = IncomingSplit[0].Split('=')[1];
                        if (CurrentUser.IC == ic)
                        {
                            if (IncomingSplit[1] == "LOGOUT")
                            {
                                this.Stop();
                            }
                            if (IncomingSplit[1].Contains("CHAT"))
                            {
                                string chat = IncomingSplit[1].Split('=')[1];
                                string message = IncomingSplit[2].Split('=')[1];
                                try
                                {
                                    Program.GetHandler(chat).SendMessage("$$CHAT=" + this.CurrentUser.GetUsername() + "$$MSG=" + message);
                                }
                                catch (NullReferenceException) { }
                                catch (ArgumentNullException) { }
                            }

                            if (IncomingSplit[1] == "GETUSERS")
                            {
                                foreach (ClientHandler ch in Program.Handlers)
                                {
                                    string msg = "$$CMD$$ACTION=ADD$$VALUE=" + ch.GetUsername();
                                    SendMessage(msg);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Start()
        {
            this.IsRunning = true;
            this.thread = new Thread(Communicating);
            this.thread.Start();
        }

        public void Stop()
        {
            this.IsRunning = false;
            ClientHandlerStop?.Invoke(this, new EventArgs());
            this.thread.Abort();
        }

        public void SendMessage(string message)
        {
            Console.WriteLine("To C" + this.ID + ": " + message);
            writer.WriteLine(message);
        }
    }
}
