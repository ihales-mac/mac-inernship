using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class HandleClient
    {
        public static List<String> users = new List<string>();
        String nume;
        Dictionary<String, String> listUser = new Dictionary<string, string>()
        {
            {"Robi","a" },
            {"Andrei","b" },
            {"Cata","c" }
        };
        NetworkStream networkStream;
        TcpClient client;
        string clNo;
        public List<TcpClient> cliensTcpList;

        public void startClient(TcpClient inClientSocket, List<TcpClient> clientsTcp)
        {
            this.client = inClientSocket;

            Thread ctThread = new Thread(doChat);
            ctThread.Start();
            cliensTcpList = clientsTcp;
        }

        private void doChat()
        {

            try
            {
                while (true)
                {

                    networkStream = client.GetStream();
                    byte[] bytes = new byte[client.ReceiveBufferSize];
                    int toRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                    string message2 = ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);

                    string message = Common.Cryption.DecryptMessage(message2);

                    string[] words = message.Split(" ");
                    if (words[0] == "Login")
                    {
                        Login(words[1], words[2]);
                        users.Add(words[1]);
                        nume = words[1];


                    }
                    else
                    if (words[0] == "Message")
                    {
                        Console.WriteLine(words[1]);
                    }
                    else
                        if (words[0] == "Show")
                        Show(words);
                    else
                    {
                        if (words[0] == "Send")
                        {
                            String mesaj = "";
                            for (int i = 1; i < words.Length; i++)
                                mesaj += words[i] + " ";
                            Send(mesaj);
                        }
                        else
                            if (words[0] == "Logout")
                            client.Close();
                        else
                            if (words[0] == "SendForOne")
                        {
                            String mesaj = "";
                            for (int i = 2; i < words.Length; i++)
                                mesaj += words[i] + " ";

                            int numberOfClient = 0;
                            for (int i = 0; i < users.Count; i++)
                                if (users[i] == words[1])
                                    break;
                                else
                                    numberOfClient++;

                            SendForOne(mesaj, numberOfClient);
                        }else
                            if(words[0] == "SendForOnePrivate")
                        {
                            String mesaj = "";
                            for (int i = 2; i < words.Length; i++)
                                mesaj += words[i] + " ";

                            int numberOfClient = 0;
                            for (int i = 0; i < users.Count; i++)
                                if (users[i] == words[1])
                                    break;
                                else
                                    numberOfClient++;

                            SendForOnePrivate(mesaj, numberOfClient);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No client");
            }
        }


     

        public void Login(String Username, String password)
        {


            if (listUser[Username] == password)
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes("true");
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
            }
            else
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes("false");
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
            }


        }
        public void Show(String[] words)
        {
            String listOfUsers = "";
            for (int i = 0; i < users.Count; i++)
            {
                listOfUsers += users[i] + " ";
            }
            string encryptedMessage = Common.Cryption.EncryptMessage("Show" + " " + listOfUsers);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(encryptedMessage);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
        }
        public void Send(String msg)
        {
            for (int i = 0; i < cliensTcpList.Count; i++)
            {
                TcpClient broadcastSocket;
                broadcastSocket = cliensTcpList[i];
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                string encryptedMessage = Common.Cryption.EncryptMessage("Send" + " " + nume + " says : " + msg);
                broadcastBytes = Encoding.ASCII.GetBytes(encryptedMessage);

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }

        public void SendForOne(String msg, int numberOfClient)
        {
            TcpClient broadcastSocket;
            broadcastSocket = cliensTcpList[numberOfClient];
            NetworkStream broadcastStream = broadcastSocket.GetStream();
            Byte[] broadcastBytes = null;

            string encryptedMessage = Common.Cryption.EncryptMessage("SendForOne" + " " + nume + " says : " + msg);
            broadcastBytes = Encoding.ASCII.GetBytes(encryptedMessage);
            broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            broadcastStream.Flush();

        }

        public void SendForOnePrivate(String msg, int numberOfClient)
        {
            TcpClient broadcastSocket;
            broadcastSocket = cliensTcpList[numberOfClient];
            NetworkStream broadcastStream = broadcastSocket.GetStream();
            Byte[] broadcastBytes = null;

            string encryptedMessage = Common.Cryption.EncryptMessage("SendForOnePrivate" + " " + nume + " says : " + msg);
            broadcastBytes = Encoding.ASCII.GetBytes(encryptedMessage);
            broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            broadcastStream.Flush();

        }


    }
}

