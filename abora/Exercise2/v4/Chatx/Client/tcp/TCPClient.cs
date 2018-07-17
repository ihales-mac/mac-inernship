using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common;
using System.Threading;

namespace Client.tcp
{
    public class TCPClient
    {
        public TcpClient _socket { get; set; }
        private string _host;
        private int _port;
        private string PublicKeyToSend;
        private string PrivateKey;
        private string PublicKey;

        public TCPClient(string host, int port)
        {
            this._host = host;
            this._port = port;
            EncryptionRSA RSA = new EncryptionRSA();
            PublicKeyToSend = RSA.GetPublicKey();
            PrivateKey = RSA.GetPrivateKey();
            _socket = new TcpClient(_host, _port);
            GetKeyFromServer(_socket);
            SendPublicKeyToServer(PublicKeyToSend);
        }

        internal void ReadOnly(Message message)
        {
            var rsa = new EncryptionRSA();
            rsa.SetPrivateKey(PrivateKey);
            message.ReadFrom(_socket.GetStream(), rsa);
            
        }

        private void SendPublicKeyToServer(string publicKeyToSend)
        {
            Message sendKeyMsg = new Message();
            sendKeyMsg.Header = "PUBLIC_KEY_CLIENT";
            sendKeyMsg.Body = publicKeyToSend;
            Console.WriteLine("Client send public key: " + publicKeyToSend);
            sendKeyMsg.WriteTo(_socket.GetStream());
        }

        internal void Close(Message closeMsg)
        {
            _socket.Close();
        }

        private void GetKeyFromServer(TcpClient socket)
        {
            Message requestedKey = new Message();
            requestedKey.ReadFrom(socket.GetStream());

            string key = requestedKey.Body;
            PublicKey = key;


            Console.WriteLine("Key send to client: " + key);
        }

        internal void SendOnly(Message message)
        {
            var rsa = new EncryptionRSA();
            rsa.SetPublicKey(PublicKey);
            message.WriteTo(_socket.GetStream(), rsa);
        }

        public bool Send(Message request)
        {
            var rsa = new EncryptionRSA();
            rsa.SetPublicKey(PublicKey);

            request.WriteTo(_socket.GetStream(), rsa);
            Console.WriteLine("client -send : " + request.Body);
            Message response = new Message();
            rsa = new EncryptionRSA();
            rsa.SetPrivateKey(PrivateKey);
            response.ReadFrom(_socket.GetStream(),rsa);
         
            Console.WriteLine("Client receive: " + response.Body);

            if (response.Header.Equals("OK"))
            {
                return true;
            }
            return false;

        }

        public NetworkStream getStream()
        {
            return this._socket.GetStream();
        }

    }
}
