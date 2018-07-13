using ClientApp.View;
using CommonApp;
using CommonApp.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace ClientApp.SocketNp
{



    public class SynchronousSocketClient : ICommunication<Socket>
    {
        
        private string Username;
        byte[] Key, IV;
        public void SetUserName(string user) {
            Username = user;
        }
        public string GetUserName() {
            return Username;
        }

        public SynchronousSocketClient(String client){
            
            Username = client;


         }
        
        public T GetMessage<T>(string message) {
            JMessage mess = JMessage.Deserialize(message);
            return mess.ToValue<T>();

        }

        public byte[] EncryptMessage(string msg) {
            Console.WriteLine("key {0}, array {1}", Convert.ToBase64String(this.Key), Convert.ToBase64String(this.IV));
            
            return CommonApp.RijndaelClass.EncryptStringToBytes(msg, Key, IV);
        }

        public string DecryptMessage(byte[] msg) {
            Console.WriteLine("key {0}, array {1}", Convert.ToBase64String(this.Key), Convert.ToBase64String(this.IV));
            return CommonApp.RijndaelClass.DecryptStringFromBytes(msg, Key, IV);
        }

        public string SendAndReceiveMessage<T>(T obj, Header type = Header.Unspecified)
        {

            byte[] bytes = new byte[1024];
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    JMessage msg = JMessage.FromValue<T>(obj, type);
                    string serialized = JMessage.Serialize(msg);



                    //byte[] send = Encoding.ASCII.GetBytes(serialized);
                    // Send the data through the socket. 
                    int bytesSent, bytesRec;
                    string received;
                    if (type == Header.Handshake)
                    {
                        bytesSent = sender.Send(Encoding.ASCII.GetBytes(serialized));
                        bytesRec = sender.Receive(bytes);
                        received = Encoding.ASCII.GetString(bytes);
                        

                    }
                    else
                    {
                        Console.WriteLine("Encrypyed {0}", Convert.ToBase64String(EncryptMessage(serialized)));
                        bytesSent = sender.Send(EncryptMessage(serialized));
                        bytesRec = sender.Receive(bytes);
                        received = DecryptMessage(bytes);

                    }
                 

                    var cleaned = received.Replace("\0", string.Empty);
                    Console.WriteLine("Echoed = {0}", cleaned);
                    sender.Close();

               

                    return cleaned;


                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }          
             return null;
        }

        public void GetKeyAndArray() {
            string msg = this.SendAndReceiveMessage("", Header.Handshake);
            KeyValuePair<byte[],byte[]> pair = JMessage.Deserialize(msg).ToValue<KeyValuePair<byte[], byte[]>>();
            Key = pair.Key;
            IV = pair.Value;


        }

        public string GetKey() {

            return Convert.ToBase64String(Key);
        }

        public string GetIV() {
            return Convert.ToBase64String(IV);
        }

        public IList<Message> SendMessage(string UsernameTo, string message, DateTime time) {
            Message m = new Message(this.Username,UsernameTo,message,time);
            string messages= this.SendAndReceiveMessage(m, Header.Message);
            return JMessage.Deserialize(messages).ToValue<IList<Message>>();
            

        }

        public IList<Message> GetMessages() {
            return JMessage.Deserialize(this.SendAndReceiveMessage<object>("", Header.Messages)).ToValue<IList<Message>>();
        }
        public  Dictionary<string, string> GetUsers() {
            return JMessage.Deserialize(this.SendAndReceiveMessage<object>("", Header.Users)).ToValue<Dictionary<string,string>>();
        }

       
        public void Login(string Username, string Password)
        {

            GetKeyAndArray();

            this.Username = Username;
            KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>(Username, HashesClass.computeHash(Password, "MD5"));


            if (this.SendAndReceiveMessage(keyValue, Header.Login).Equals("correct"))
            {

                ChatWindow chat = new ChatWindow();
                chat.SetUserName(Username);
                chat.Show();

            }
            else {
                MessageBox.Show("Sorry, incorrect username or password. Try again.");


            }



        }


    }
}
