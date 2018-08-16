
using CommonApp;
using CommonApp.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ServerApp.SocketNp
{

    public class SynchronousSocketListenerAsym : ICommunication<Socket>
    {
        private static Messages _messages = new Messages();
        private RSACryptoServiceProvider _RSA;

        public SynchronousSocketListenerAsym()
        {

            GenerateKeys();
        }
        public string ResolveLogin(string username, string password)
        {
            string response;

            try
            {
                if (ServerApp.Users.UsersDict[username].Equals(password))
                {
                    //connected.Add(username);
                    response = "correct";
                }
                else
                    response = "wrong";
            }
            catch (Exception)
            {
                response = "wrong";
            }
            return response;

        }



        // Incoming data from the client.  
        public static string data = null;

        public void Send(Socket handler, string data, RSAParameters key)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            byte[] encrypted = CommonApp.RSAClass.RSAEncrypt(bytes, key, false);
            handler.Send(encrypted);

        }

        public void UnpackAndSend(Socket handler, string message, Header type) {
            JMessage jmess = JMessage.Deserialize(message);
            KeyValuePair<string, string> keyValuePair = jmess.ToValue<KeyValuePair<string, string>>();
            RSAParameters param = (RSAParameters)ServerApp.Users.GetUserKeys()[keyValuePair.Key];
            SendMessage(handler, keyValuePair.Value, type, param);
        }


        public void SendUnencrypted(Socket handler, string rec) {

            JMessage des = JMessage.Deserialize(rec);
            KeyValuePair<string, RSAParameters> kv2 = des.ToValue<KeyValuePair<string, RSAParameters>>();
            ServerApp.Users.SetUserKey(kv2.Key, kv2.Value);
            JMessage jmess;
            jmess = JMessage.FromValue<RSAParameters>(_RSA.ExportParameters(false), Header.ExchangePKs);
            handler.Send(Encoding.ASCII.GetBytes(JMessage.Serialize(jmess)));
        

        }

        public void SendMessage(Socket handler, string rec, Header type, RSAParameters key)
        {
            switch (type)
            {

                case Header.Login:
                    JMessage jmess = JMessage.Deserialize(rec);
                    KeyValuePair<string, string> kv = jmess.ToValue<KeyValuePair<string, string>>();

                    string answ = ResolveLogin(kv.Key, kv.Value);

                    Send(handler, answ, key);

                    break;

                case Header.Message:
                    jmess = JMessage.Deserialize(rec);
                    //Console.WriteLine(Convert.FromBase64String(rec));
                    Message message = jmess.ToValue<Message>();
                    ServerApp.Messages.AddMessage(message);
                    Send(handler, JMessage.Serialize(JMessage.FromValue<IList<CommonApp.Model.Message>>(ServerApp.Messages.GetMessagesOfUser(message.UserNameFrom), Header.Message)), key);
                    break;

                case Header.Messages:
                    string user = JMessage.Deserialize(rec).ToValue<string>();
                    Send(handler, JMessage.Serialize(JMessage.FromValue<IList<CommonApp.Model.Message>>(ServerApp.Messages.GetMessagesOfUser(user), Header.Messages)), key);
                    break;

                case Header.Users:
                    jmess = JMessage.FromValue<Dictionary<string, string>>(ServerApp.Users.UsersDict, Header.Users);
                    Send(handler, JMessage.Serialize(jmess), key);
                    break;




            }
        }
        public void GenerateKeys()
        {
            _RSA = new RSACryptoServiceProvider(8192);
           

        }
        public void SendKey(Socket handler)
        {


            //Console.WriteLine("Key {0} ", Convert.ToBase64String(rijndael.Key));
         
       
            string json = JMessage.Serialize(JMessage.FromValue<RSAParameters>(_RSA.ExportParameters(false), Header.ExchangePKs));
            Console.WriteLine(json);

            handler.Send(Encoding.ASCII.GetBytes(json));
        }


        public void StartListening()
        {




            // Data buffer for incoming data.  
            byte[] bytes = new Byte[2048];
            //  CommonApp.RijndaelClass.GenerateNewKey();
            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();

                    Console.WriteLine("Someone connected...");
                    // An incoming connection needs to be processed.  
                    int bytesRec;

                    bytesRec = handler.Receive(bytes);
                    byte[] bytesNew = new byte[bytesRec];
                    for (int i = 0; i < bytesRec; i++)
                        bytesNew[i] = bytes[i];

                    CommonApp.JMessage jmessage;
                    try
                    {
                        //try to deserialize (in case it's not encrypted, it will do)
                        string msg = Encoding.ASCII.GetString(bytesNew);
    
                        jmessage = CommonApp.JMessage.Deserialize(msg);

                        SendUnencrypted(handler, msg);
                        //UnpackAndSend(handler, msg, jmessage.Type);
                        Console.WriteLine("Handshake done");
                    }
                    catch (Exception)
                    {
                        //encrypted, so decrypt and deserialize
                        Console.WriteLine("Encrypted message receive");
                     //   CommonApp.RijndaelClass.TruncateBytesArray(ref bytesNew);
                     //   Console.WriteLine("Truncated array {0}", Convert.ToBase64String(bytesNew));

                        byte[] decrb=  CommonApp.RSAClass.RSADecrypt(bytesNew, _RSA.ExportParameters(true), false);

                        string decr = Encoding.Unicode.GetString(decrb);
                        Console.WriteLine("Decrypted message {0}", decr);

                        //  byte[] res = Convert.FromBase64String(decr);
                        //  CommonApp.RijndaelClass.TruncateBytesArray(ref res);
                        //  Console.WriteLine("Truncated array {0}", Convert.ToBase64String(res));
                        jmessage = CommonApp.JMessage.Deserialize(decr);
                        Console.WriteLine("Deserialized {0}", jmessage.Value.ToString());
                        UnpackAndSend(handler, decr, jmessage.Type);



                    }
                    //SendMessage(handler, jmessage.Value.ToObject<string>(), jmessage.Type);


                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void SendMessage()
        {
            throw new NotImplementedException();
        }

        public string Users()
        {
            throw new NotImplementedException();
        }

        public string Messages()
        {
            throw new NotImplementedException();
        }
    }
}
