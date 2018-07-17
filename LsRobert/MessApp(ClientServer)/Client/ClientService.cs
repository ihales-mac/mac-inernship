using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Client
{
    class ClientService
    {
       
        TcpClient clientSocket = new TcpClient();
        static NetworkStream stream;

        

      


       public ClientService()
        {
            clientSocket.Connect("127.0.0.1", 5432);
            Console.WriteLine("Client Socket Program - Server Connected ...");
        }
        public TcpClient getClient() { return clientSocket; }
       

        public  Boolean login(String username,String password)
        {
            
            stream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Login" + " " + username + " " +password + " " + "$");
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();

            byte[] bytes = new byte[clientSocket.ReceiveBufferSize];
            int toRead = clientSocket.GetStream().Read(bytes, 0, clientSocket.ReceiveBufferSize);
            string messageFromServer=ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
            if (messageFromServer == "true") 
                return true;
            return false;

        }
    }
}
