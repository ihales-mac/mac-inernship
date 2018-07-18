using Client.Connection.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Utils.EventArguments;
using Client.Utils;
using CryptographyServices;

namespace Client.Connection.ConnectionServices
{
    public class SocketConnectionService : IConnectionService
    {
        private CryptographyService _cryptographyService;
        public event ClientListChangedEventHandler ClientListChanged;
        public event MessageReceivedEventHandler MessageReceived;
        public char splitChar = '~';
        private volatile static bool _isLoggedIn;
        private TcpClient _clientSocket;
        private IPEndPoint _remoteEp;
        private NetworkStream _serverStream;
        private Thread _receiveMesagesThread;
        private string _serverPublicKey;
        public string Username;

        public void HandshakeWithServer()
        {
            byte[] msg = Encoding.ASCII.GetBytes(_cryptographyService.PublicKey);
            _serverStream.Write(msg, 0, msg.Length);
            _serverStream.Flush();
            byte[] bytes = new byte[10025];

            int bytesRead = _serverStream.Read(bytes, 0, bytes.Length);
            _serverPublicKey = Encoding.ASCII.GetString(bytes, 0, bytesRead);
        }

        public void Connect()
        {
            try
            {
                _cryptographyService = new CryptographyService();
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[1];
                _remoteEp = new IPEndPoint(ipAddress, 11100);
                _clientSocket = new TcpClient();
                _clientSocket.Connect(_remoteEp);
                _serverStream = _clientSocket.GetStream();
                HandshakeWithServer();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            SendMessage("c");
            _clientSocket.Close();
        }

        public List<string> GetOnlineUsers()
        {

            List<string> usernames = new List<string>();
            string response = SendMessage("o");
            string[] usernamesArray = response.Split(splitChar);
            usernames = usernamesArray.ToList();
            return usernames;
        }

        public void ReceiveMessagesFromServer()
        {
            byte[] bytes = new byte[10025];
            while (true)
            {
                if (_isLoggedIn == true)
                {

                    int bytesRead = _serverStream.Read(bytes, 0, bytes.Length);
                    string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                    message = _cryptographyService.DecryptData(message);
                    string[] messageContent = message.Split(splitChar);
                    if (messageContent[0] == "m")
                    {
                        MessageReceivedEventArgs e = new MessageReceivedEventArgs(new Message(messageContent[1], "me", messageContent[2]));
                        OnMessageReceived(e);
                    }

                    if (messageContent[0] == "lc")
                    {
                        List<string> usernames = new List<string>();
                        for(int i=1;i<messageContent.Length;i++)
                            usernames.Add(messageContent[i]);
                        ClientListChangedEventArgs e = new ClientListChangedEventArgs(new List<string>(usernames));
                        OnClientListChanged(e);
                    }
                }
            }
        }

        protected void OnMessageReceived(MessageReceivedEventArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, e);
            }
        }

        protected void OnClientListChanged(ClientListChangedEventArgs e)
        {
            if (ClientListChanged != null)
            {
                ClientListChanged(this, e);
            }
        }

        public string Authenticate(string username, string password)
        {
            string result = SendMessage("l " + username + splitChar + password);
            if (result.Contains("Login succesful!"))
            {
                this.Username = username;
                _isLoggedIn = true;
            }
            else
            {
                _isLoggedIn = false;
            }
            return result;

        }

        public void SendMessageToUser(string receiver, string message)
        {
            string messageToEncrypt = "m" + Username + splitChar + receiver + splitChar + message;
            byte[] msg = Encoding.ASCII.GetBytes(_cryptographyService.EncryptText(_serverPublicKey,messageToEncrypt));
            _serverStream.Write(msg, 0, msg.Length);
            _serverStream.Flush();
        }

        public string SendMessage(string message)
        {
            if (_isLoggedIn && _receiveMesagesThread != null)
            {
                _receiveMesagesThread.Abort();
            }


            byte[] bytes = new byte[10025];
            string response = null;
            if (_clientSocket == null || !_clientSocket.Connected)
                try
                {
                    Connect();
                }
                catch (Exception ex)
                {
                    _clientSocket.Close();
                    return ex.ToString();
                }
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(_cryptographyService.EncryptText(_serverPublicKey, message));
                _serverStream.Write(msg, 0, msg.Length);
                _serverStream.Flush();
                int bytesRead = _serverStream.Read(bytes, 0, bytes.Length);
                response = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            }
            catch (ArgumentNullException ane)
            {
                _clientSocket.Close();
                return "ArgumentNullException :" + ane.ToString();
            }
            catch (SocketException se)
            {
                _clientSocket.Close();
                return "SocketException :  " + se.ToString();
            }
            catch (Exception ex)
            {
                _clientSocket.Close();
                return "Unexpected exception : " + ex.ToString();
            }
            if (_isLoggedIn)
            {
                _receiveMesagesThread = new Thread(ReceiveMessagesFromServer);
                _receiveMesagesThread.Start();
            }
            return response;
        }
    }
}
