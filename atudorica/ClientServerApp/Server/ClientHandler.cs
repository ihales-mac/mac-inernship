using Server.Utils.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ClientHandler
    {
        public event ClientDisconnectEventHandler ClientDisconnect;
        public event ClientListRequestedEventHandler ClientListRequested;
        public TcpClient ClientSocket { get; set; }
        private readonly ServerUtils _utils;
        private string _clientNumber;
        public string Username { get; set; }        
        private NetworkStream _networkStream;
        public ClientHandler()
        {
            _utils = new ServerUtils();
        }

        public void StartClient(TcpClient inClientSocket, string clientNo)
        {
            this.ClientSocket = inClientSocket;
            this._clientNumber = clientNo;
            Thread ctThread = new Thread(DoChat);
            ctThread.Start();
        }

        public void StopClient()
        {
            ClientSocket.Close();
            Console.WriteLine("Client " + this._clientNumber + " has disconnected!");
            OnClientDisconnect(new ClientDisconnectEventArgs());
            Thread.CurrentThread.Join();
        }

        protected void OnClientDisconnect(ClientDisconnectEventArgs e)
        {
            if (ClientDisconnect != null)
            {
                ClientDisconnect(this, e);
            }
        }

        protected void OnClientClientListRequested(ClientListRequestedEventArgs e)
        {
            if (ClientListRequested != null)
            {
                ClientListRequested(this, e);
            }
        }

        public void SendMessageToClient(string response)
        {
            if (ClientSocket.Connected)
            {
                string serverResponse = response;
                byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                _networkStream.Write(sendBytes, 0, sendBytes.Length);
                _networkStream.Flush();
                Console.WriteLine(" >> " + "Server to client (" + _clientNumber + "): " + serverResponse);
            }
        }

        public string ReadMessageFromClient()
        {
            byte[] bytesFrom = new byte[10025];
            _networkStream = ClientSocket.GetStream();
            int bytesRead = _networkStream.Read(bytesFrom, 0, bytesFrom.Length);
            _networkStream.Flush();
            string dataFromClient= System.Text.Encoding.ASCII.GetString(bytesFrom, 0, bytesRead);
            Console.WriteLine(" >> " + "From client( " + _clientNumber + "): " + dataFromClient);
            return dataFromClient;
        }

        private string Login(string dataFromClient)
        {
            dataFromClient = dataFromClient.Substring(2, dataFromClient.Length - 2);
            string[] data = dataFromClient.Split('/');
            if (_utils.CheckCredentials(data[0],data[1]))
            {
                this.Username = data[0];
                return "Login succesful! Welcome, " + Username + "!"; 
            }
            return "Login failed! Wrong credentials";
        }

        

        private void DoChat()
        {
            int requestCount = 0;
            string dataFromClient = null;
            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    dataFromClient = ReadMessageFromClient();
                    switch (dataFromClient[0])
                    {
                        case 'l':
                            SendMessageToClient(Login(dataFromClient));
                            break;
                        case 'c':
                            StopClient();
                            break;
                        case 'o':
                            OnClientClientListRequested(new ClientListRequestedEventArgs());
                            break;
                        default:
                            {
                                SendMessageToClient("Invalid command!");
                                break;
                            }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }
    }
}
