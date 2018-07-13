using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class HandleClient
    {
        public static  List<String> users = new List<string>();
        String nume;
        Dictionary<String, String> listUser = new Dictionary<string, string>()
        {
            {"Robi","a" },
            {"Andrei","b" }
        };
        NetworkStream networkStream;
        TcpClient client;
        string clNo;
        public List<TcpClient> cliensTcpList;

        public void startClient(TcpClient inClientSocket, string clineNo,List<TcpClient> clientsTcp)
        {
            this.client = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
            cliensTcpList = clientsTcp;
        }

        private void doChat()
        {
            while (true)
            {
                networkStream = client.GetStream();
                byte[] bytes = new byte[client.ReceiveBufferSize];
                int toRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                string message = ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
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
                    if(words[0] == "Show")
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
                }
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
            Byte[] sendBytes2 = Encoding.ASCII.GetBytes(users.Count.ToString());
            networkStream.Write(sendBytes2, 0, sendBytes2.Length);
            networkStream.Flush();

            for (int i = 0; i < users.Count; i++)
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(users[i]);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();

                byte[] bytes = new byte[client.ReceiveBufferSize];
                int toRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                string messageFromClient = ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
            }
        }
        public void Send(String msg)
        {
            for(int i=0;i<cliensTcpList.Count;i++)
            {
                TcpClient broadcastSocket;
                broadcastSocket = cliensTcpList[i];
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;
                broadcastBytes = Encoding.ASCII.GetBytes(nume + " says : " + msg);
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
        }
    }

