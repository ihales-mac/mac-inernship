using CryptographyServices;
using Server.Utils.EventArguments;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ClientHandler
    {
        private readonly CryptographyService _cryptographyService;
        public char splitChar = '~';
        public event ClientLoggedInEventHandler ClientLoggedIn;
        public event ClientDisconnectEventHandler ClientDisconnect;
        public event ClientListRequestedEventHandler ClientListRequested;
        public event ClientToClientMessageEventHandler ClientToClientMessage;
        public TcpClient ClientSocket { get; set; }
        private readonly ServerUtils _utils;
        private string _clientNumber;
        public string Username { get; set; }
        private NetworkStream _networkStream;
        private string _clientPublicKey;
        public ClientHandler()
        {
            _cryptographyService = new CryptographyService();
            _utils = new ServerUtils();
        }

        public void HandshakeWithClient()
        {
            byte[] bytes = new byte[10025];
            int bytesRead = _networkStream.Read(bytes, 0, bytes.Length);
            _clientPublicKey = Encoding.ASCII.GetString(bytes, 0, bytesRead);

            byte[] msg = Encoding.ASCII.GetBytes(_cryptographyService.PublicKey);
            _networkStream.Write(msg, 0, msg.Length);
            _networkStream.Flush();

        }

        public void StartClient(TcpClient inClientSocket, string clientNo)
        {
            this.ClientSocket = inClientSocket;
            this._clientNumber = clientNo;
            _networkStream = ClientSocket.GetStream();
            HandshakeWithClient();
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
        protected void OnClientLoggedIn(ClientLoggedInEventArgs e)
        {
            if (ClientLoggedIn != null)
            {
                ClientLoggedIn(this, e);
            }
        }
        protected void OnClientClientListRequested(ClientListRequestedEventArgs e)
        {
            if (ClientListRequested != null)
            {
                ClientListRequested(this, e);
            }
        }
        protected void OnClientToClientMessage(ClientToClientMessageEventArgs e)
        {
            if (ClientToClientMessage != null)
            {
                ClientToClientMessage(this, e);
            }
        }

        public void SendMessageToClient(string response)
        {
            if (ClientSocket.Connected)
            {
                string serverResponse = _cryptographyService.EncryptText(_clientPublicKey, response);
                byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                _networkStream.Write(sendBytes, 0, sendBytes.Length);
                _networkStream.Flush();
                Console.WriteLine(" >> " + "Server to client (" + _clientNumber + "): " + serverResponse);
            }
        }

        public string ReadMessageFromClient()
        {
            byte[] bytesFrom = new byte[10025];
            int bytesRead = _networkStream.Read(bytesFrom, 0, bytesFrom.Length);
            _networkStream.Flush();
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom, 0, bytesRead);
            dataFromClient = _cryptographyService.DecryptData(dataFromClient);
            Console.WriteLine(" >> " + "From client( " + _clientNumber + "): " + dataFromClient);
            return dataFromClient;
        }

        private string Login(string dataFromClient)
        {
            dataFromClient = dataFromClient.Substring(2, dataFromClient.Length - 2);
            string[] data = dataFromClient.Split(splitChar);
            if (_utils.CheckCredentials(data[0], data[1]))
            {
                this.Username = data[0];
                return "Login succesful! Welcome, " + Username + "!";
            }
            return "Login failed! Wrong credentials";
        }

        private void SendChatMessage(string dataFromClient)
        {
            dataFromClient = dataFromClient.Substring(1, dataFromClient.Length - 1);
            string[] data = dataFromClient.Split(splitChar);
            ClientToClientMessageEventArgs e = new ClientToClientMessageEventArgs(data[0], data[1], data[2]);
            OnClientToClientMessage(e);

        }

        private void DoChat()
        {
            int requestCount = 0;
            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    string dataFromClient = ReadMessageFromClient();
                    switch (dataFromClient[0])
                    {
                        case 'l':
                            {
                                string loginResponse = Login(dataFromClient);
                                SendMessageToClient(loginResponse);
                                if (loginResponse.Contains("Login succesful!"))
                                    OnClientLoggedIn(new ClientLoggedInEventArgs(this.Username));
                                break;
                            }
                        case 'c':
                            StopClient();
                            break;
                        case 'o':
                            OnClientClientListRequested(new ClientListRequestedEventArgs());
                            break;
                        case 'm':
                            SendChatMessage(dataFromClient);
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
                    StopClient();
                    break;
                }
            }
        }
    }
}
