using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonApp;
using CommonApp.Model;

namespace ClientApp.SocketNp
{
    public abstract class SynchronousSocketClient : ICommunication<Socket>
    {
        protected string _username;
        public IList<Message> GetMessages()
        {
            return JMessage.Deserialize(this.SendAndReceiveMessage<object>(this._username, Header.Messages)).ToValue<IList<Message>>();
        }
        public Dictionary<string, string> GetUsers()
        {

            string users = this.SendAndReceiveMessage<object>("users", Header.Users);
            return JMessage.Deserialize(users).ToValue<Dictionary<string, string>>();
        }

        public bool Login(string Username, string Password)
        {
            this._username = Username;
            NewKey();

            KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>(Username, HashesClass.ComputeHash(Password, "MD5"));


            if (this.SendAndReceiveMessage(keyValue, Header.Login).Equals("correct"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract void NewKey();
        public virtual string SendAndReceiveMessage<T>(T obj, Header type)
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



                    //byte[] send = Encoding.ASCII.GetBytes(serialized);
                    // Send the data through the socket. 
                    int bytesSent, bytesRec;
                    string received;
                    if (type == Header.Handshake || type == Header.ExchangePKs)
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
                        Console.WriteLine("Going to send {0}", Convert.ToBase64String(EncryptMessage(serialized)));
                        bytesSent = sender.Send(EncryptMessage(serialized));
                        Console.WriteLine("Sent data");
                        bytesRec = sender.Receive(bytes);
                        Console.WriteLine("Received data");
                        CommonApp.RijndaelClass.TruncateBytesArray(ref bytes);
                        Console.WriteLine("Received, truncated and got {0}", Convert.ToBase64String(bytes));
                        received = DecryptMessage(bytes);
                        Console.WriteLine("Decrypted message and got {0}", received);

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

        internal abstract string DecryptMessage(byte[] byteMessage);

        internal abstract byte[] EncryptMessage(string serialized, Header type = Header.Unspecified);
        public abstract IList<Message> SendMessage(string user, string writeTo, string text, DateTime now);

        public void SetUsername(string name)
        {
            _username = name;
        }
    }
}
