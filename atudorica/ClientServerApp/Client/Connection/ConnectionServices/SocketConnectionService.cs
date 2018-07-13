using Client.Connection.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Connection.ConnectionServices
{
    public class SocketConnectionService : IConnectionService
    {
        private TcpClient ClientSocket;
        private IPEndPoint remoteEP;
        private NetworkStream serverStream;
        public string Username;
        public void Connect()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[1];
                remoteEP = new IPEndPoint(ipAddress, 11100);
                ClientSocket = new TcpClient();
                ClientSocket.Connect(remoteEP);
                serverStream = ClientSocket.GetStream();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            SendMessage("c");
            ClientSocket.Close();
        }

        public List<string> GetOnlineUsers()
        {
            List<string> usernames=new List<string>();
            string response = SendMessage("o");
            string[] usernamesArray = response.Split('~');
            usernames = usernamesArray.ToList();
            return usernames;
        }

        public string Authenticate(string username, string password)
        {
            string result = SendMessage("l " + username + "/" + password);
            if (result.Contains("Login succesful!"))
                this.Username = username;
            return result;

        }

        public void SendMessageToUser(string destination,string message)
        {
            SendMessage("m" + Username + "~" + destination + "~" + message);
        }

        public string SendMessage(string message)
        {
            byte[] bytes = new byte[10025];
            string response = null;
            if (ClientSocket==null || !ClientSocket.Connected)
                try
                {
                    Connect();
                }
                catch (Exception ex)
                {
                    ClientSocket.Close();
                    return ex.ToString();
                }
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);
                serverStream.Write(msg, 0, msg.Length);
                serverStream.Flush();
                int bytesRead=serverStream.Read(bytes, 0, bytes.Length);
                response = Encoding.ASCII.GetString(bytes,0, bytesRead);
            }
            catch (ArgumentNullException ane)
            {
                ClientSocket.Close();
                return "ArgumentNullException :" + ane.ToString();
            }
            catch (SocketException se)
            {
                ClientSocket.Close();
                return "SocketException :  " + se.ToString();
            }
            catch (Exception ex)
            {
                ClientSocket.Close();
                return "Unexpected exception : " + ex.ToString();
            }
            return response;
        }
    }
}
