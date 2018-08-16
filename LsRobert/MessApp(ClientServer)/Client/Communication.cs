using Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
     class Communication:ICommunication
    {

        TcpClient clientSocket = new TcpClient();
        static NetworkStream stream;

        
        

        public Communication()
        {
            clientSocket.Connect("127.0.0.1", 5432);
            Console.WriteLine("Client Socket Program - Server Connected ...");
        }

        public void SendMessage(string message)
        {
            stream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();
        }

        public String getMessage()
        {
            byte[] bytes = new byte[clientSocket.ReceiveBufferSize];
            int toRead = clientSocket.GetStream().Read(bytes, 0, clientSocket.ReceiveBufferSize);
            string messageFromServer = ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
            return messageFromServer; 
        }

        public TcpClient getClient()
        {
            return clientSocket;
        }
    }
}
