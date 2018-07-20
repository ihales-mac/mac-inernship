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
    class SynchronousSocketClientAsym : SynchronousSocketClient
    {

        private static RSACryptoServiceProvider RSA;
        private static RSAParameters ServerPublicKey;


        public SynchronousSocketClientAsym(String client)
        {
            base._username = client;
        }


       
        public string GetUserName()
        {
            return _username;
        }



        public override void NewKey()
        {
            RSA = new RSACryptoServiceProvider(8192);
            KeyValuePair<string, RSAParameters> toSend = new KeyValuePair<string, RSAParameters>(this._username, RSA.ExportParameters(false));
            if (_username == null)
                throw new Exception("username null");
            string msg = this.SendAndReceiveMessage<KeyValuePair<string, RSAParameters>>(toSend, Header.ExchangePKs);
            RSAParameters ret = JMessage.Deserialize(msg).ToValue<RSAParameters>();
            SynchronousSocketClientAsym.ServerPublicKey = ret;
        }




 

        internal override string DecryptMessage(byte[] byteMessage)
        {
            return Encoding.Unicode.GetString(CommonApp.RSAClass.RSADecrypt(byteMessage, RSA.ExportParameters(true), false));
        }

        internal override byte[] EncryptMessage(string serialized, Header type)
        {
            //Add a new layer of information: the username for each message so that the server can identify the sender
            KeyValuePair<string, string> messageAnduser = new KeyValuePair<string, string>(_username, serialized);

            JMessage jmess = JMessage.FromValue<KeyValuePair<string, string>>(messageAnduser, type);
            string serialize = JMessage.Serialize(jmess);
            byte[] msgbytes = Encoding.Unicode.GetBytes(serialize);
            return CommonApp.RSAClass.RSAEncrypt(msgbytes, ServerPublicKey, false);
        }

        public override IList<Message> SendMessage(string user, string writeTo, string text, DateTime now)
        {
            Message m = new Message(user, writeTo, text, now);
            string messages = this.SendAndReceiveMessage(m, Header.Message);
            return JMessage.Deserialize(messages).ToValue<IList<Message>>();

        }
        public override string SendAndReceiveMessage<T>(T obj, Header type)
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
                        received = DecryptMessage(bytes);


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
    }
}
