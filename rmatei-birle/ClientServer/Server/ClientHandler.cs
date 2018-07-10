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
    public class ClientHandler
    {
        private static List<Tuple<string, string>> Users = new List<Tuple<string,string>>();


        private Thread thread { get; set; }
        private bool IsRunning = true;
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;

        public int ID { get; set; }
        public bool SymmetricEncryption { get; set; }
        public bool AsymmetricEncryption { get; set; }

        public ClientHandler(object client)
        {
            PopulateUsers();

            SymmetricEncryption = false;
            AsymmetricEncryption = false;
            this.client = (TcpClient)client;
            writer = new StreamWriter(this.client.GetStream(), Encoding.ASCII);
            reader = new StreamReader(this.client.GetStream(), Encoding.ASCII);
        }

        private void Communicating()
        {
            while (IsRunning)
            {
                string Incoming = reader.ReadLine();
                if(Incoming != null)
                {
                    this.SendMessage("ECHO " + Incoming);
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
        }

        public void SendMessage(string message)
        {
            //tbi
        }

        private static void PopulateUsers()
        {
            Users.Add(new Tuple<string, string>("user1", "user"));
            Users.Add(new Tuple<string, string>("user2", "user"));
        }

    }
}
