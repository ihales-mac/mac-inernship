using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client.tcp
{
    public class TCPClient
    {
        public TcpClient _socket { get; set; }
        private string _host;
        private int _port;

        public TCPClient(string host,int port)
        {
            this._host = host;
            this._port = port;
        }

        public bool Send(Message request)
        {
            TcpClient socket = new TcpClient(_host, _port);
            NetworkStream stream = socket.GetStream();
            request.WriteTo(stream);

            Console.WriteLine("client -send : " + request);
            Message response = new Message();
            response.ReadFrom(stream);

            Console.WriteLine("Client receive: " + response.Body);

            if (response.Header.Equals("OK"))
            {
                _socket = socket;
                return true;
            }
            return false;

        }

        public NetworkStream getStream()
        {
            return _socket.GetStream();
        }
    }
}
