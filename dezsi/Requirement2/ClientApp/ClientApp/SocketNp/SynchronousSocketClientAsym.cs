using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClientApp.View;
using CommonApp;
using CommonApp.Model;

namespace ClientApp.SocketNp
{
    class SynchronousSocketClientAsym : ICommunication<Socket>
    {
        private string _username;
        private static RSACryptoServiceProvider RSA;
        private static RSAParameters ServerPublicKey;
        public void SetUserName(string user)
        {
            _username = user;
        }
        public string GetUserName()
        {
            return _username;
        }

        public void NewKey() {
            RSA = new RSACryptoServiceProvider(8192);
        }

        public SynchronousSocketClientAsym(String client)
        {

            _username = client;



        }

        public T GetMessage<T>(string message)
        {
            JMessage mess = JMessage.Deserialize(message);
            return mess.ToValue<T>();

        }

        public byte[] EncryptMessage(string msg, Header type)
        {
            KeyValuePair<string, string> messageAnduser = new KeyValuePair<string, string>(_username, msg);

            JMessage jmess = JMessage.FromValue<KeyValuePair<string, string>>(messageAnduser,type );
            string serialize = JMessage.Serialize(jmess);
            byte[] msgbytes = Encoding.Unicode.GetBytes(serialize);
            return CommonApp.RSAClass.RSAEncrypt(msgbytes, ServerPublicKey, false);
        }

        public string DecryptMessage(byte[] msg)
        {

            return Encoding.Unicode.GetString(CommonApp.RSAClass.RSADecrypt(msg, RSA.ExportParameters(true), false));
        }

        public string SendAndReceiveMessage<T>(T obj, Header type)
        {

            byte[] bytes = new byte[2048];
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



                    byte[] send = Encoding.ASCII.GetBytes(serialized);
                    // Send the data through the socket. 
                    int bytesSent, bytesRec;
                    string received;
                    if (type == Header.ExchangePKs)
                    {
                        bytesSent = sender.Send(Encoding.ASCII.GetBytes(serialized));
                        bytesRec = sender.Receive(bytes);
                        received = Encoding.ASCII.GetString(bytes);
                        var cleaned = received.Replace("\0", string.Empty);
                        Console.WriteLine("Echoed = {0}", cleaned);
                        received = cleaned;
                    }
                    else
                    {
                        Console.WriteLine("Going to send {0}", Convert.ToBase64String(EncryptMessage(serialized, type)));
                        bytesSent = sender.Send(EncryptMessage(serialized, type));
                        Console.WriteLine("Sent data");
                        bytesRec = sender.Receive(bytes);
                        Console.WriteLine("Received data");
                      //  CommonApp.RijndaelClass.TruncateBytesArray(ref bytes);
                      //  Console.WriteLine("Received, truncated and got {0}", Convert.ToBase64String(bytes));
                        received = DecryptMessage(bytes);
                        //Console.WriteLine("Decrypted message and got {0}", received);

                        received = received.TrimEnd('\0');
                        Console.WriteLine("Trimmed {0}", received);

                    }

                    sender.Close();


                    return received;


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

        public void GetKey()
        {

            KeyValuePair<string, RSAParameters> toSend = new KeyValuePair<string, RSAParameters > (this._username, RSA.ExportParameters(false));

            string msg = this.SendAndReceiveMessage < KeyValuePair<string, RSAParameters>>(toSend, Header.ExchangePKs);
            RSAParameters ret = JMessage.Deserialize(msg).ToValue<RSAParameters>();
            SynchronousSocketClientAsym.ServerPublicKey = ret;
            


        }


        public IList<Message> SendMessage(string UsernameTo, string message, DateTime time)
        {
            Message m = new Message(this._username, UsernameTo, message, time);
            string messages = this.SendAndReceiveMessage(m, Header.Message);
            return JMessage.Deserialize(messages).ToValue<IList<Message>>();


        }

        public IList<Message> GetMessages()
        {
            return JMessage.Deserialize(this.SendAndReceiveMessage<object>(this._username, Header.Messages)).ToValue<IList<Message>>();
        }
        public Dictionary<string, string> GetUsers()
        {

            string users = this.SendAndReceiveMessage<object>("users", Header.Users);
            return JMessage.Deserialize(users).ToValue<Dictionary<string, string>>();
        }


        public void Login(string Username, string Password)
        {
            this._username = Username;
            GetKey();
          
            KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>(Username, HashesClass.ComputeHash(Password, "MD5"));


            if (this.SendAndReceiveMessage(keyValue, Header.Login).Equals("correct"))
            {

                ChatWindow chat = new ChatWindow();
                chat.SetUserName(Username);
                chat.Show();

            }
            else
            {
                MessageBox.Show("Sorry, incorrect username or password. Try again.");


            }



        }
       
        

    }
}
