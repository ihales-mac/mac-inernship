using Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Client.Events.Handles;

namespace Client
{
  
    class ChatService:IChat
    {

        public event EventHandler <SendMessageArgs> SendedMessage; //meesage is the argument


        public TcpClient client;
        static NetworkStream stream;
        public ChatService(TcpClient _client)
        {
            client = _client;

        }

        String mesajCareSeTransmiteLaToti = "";

        public virtual void OnSendMessage()
        {
            if(SendedMessage != null)
            {
                SendedMessage(this, new SendMessageArgs(){ message=mesajCareSeTransmiteLaToti});
            }
        }
        public void sendMessage(String s)
        {
            stream = client.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Send"+ " " + s +  " " + "$");
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();

              Thread ctThread = new Thread(() => getMessage());
              ctThread.Start();
          
        }


        private void getMessage()
        {
           
            while (true)
            {
                byte[] bytes = new byte[client.ReceiveBufferSize];
                int toRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                string returndata = ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
                mesajCareSeTransmiteLaToti = "" + returndata;
                msg();
            }
        }
      
        public String msg()
        {

            Console.WriteLine(mesajCareSeTransmiteLaToti);
            OnSendMessage();
            return mesajCareSeTransmiteLaToti;
            
        }

        public List<String> getListOfUsers()
        {
            List<String> userList = new List<String>();

            stream = client.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Show" + " "  + "$");
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();

            byte[] bytes2 = new byte[client.ReceiveBufferSize];
            int toRead2 = client.GetStream().Read(bytes2, 0, client.ReceiveBufferSize);
            string numberOfElements = ASCIIEncoding.ASCII.GetString(bytes2, 0, toRead2);
           

            for (int i = 0; i < Int32.Parse(numberOfElements); i++)
            {
                byte[] bytes = new byte[client.ReceiveBufferSize];
                int toRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                string messageFromServer = ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
                userList.Add(messageFromServer);

                Byte[] sendBytes = Encoding.ASCII.GetBytes("Gata");
                stream.Write(sendBytes, 0, sendBytes.Length);
                stream.Flush();
            }

            return userList;
        }
    }
}
