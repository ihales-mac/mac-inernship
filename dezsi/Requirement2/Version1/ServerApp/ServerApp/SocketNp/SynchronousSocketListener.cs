
using CommonApp;
using CommonApp.Model;
using ServerApp.Model;
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
  
    public class SynchronousSocketListener : ICommunication<Socket>
    {
        private static Messages messages = new Messages();
        private RijndaelManaged rijndael;

        public SynchronousSocketListener() {

            GenerateKeyAndArray();
        }
        public string ResolveLogin(string username, string password) {
            string response;

            try
            {
                if (Users.UsersDict[username].Equals(password))
                {
                    //connected.Add(username);
                    response = "correct";
                }
                else
                    response = "wrong";
            }
            catch (Exception) {
                response = "wrong";
            }
            return response;

        }



        // Incoming data from the client.  
        public static string data = null;

        public void Send(Socket handler, string data) {
            while (data.Length % 16 != 0) {
                data += '\0';
            }
            handler.Send(CommonApp.RijndaelClass.EncryptStringToBytes(data, rijndael.Key, rijndael.IV));

        }


        public void UnpackAndSend(Socket handler, string rec, Header type) {
            switch (type)
            {

                case Header.Login:
                    JMessage jmess = JMessage.Deserialize(rec);
                    KeyValuePair<string, string> kv = jmess.ToValue<KeyValuePair<string, string>>();

                    string answ = ResolveLogin(kv.Key, kv.Value);

                    Send(handler, answ);

                    break;

                case Header.Message:
                    jmess = JMessage.Deserialize(rec);
                    //Console.WriteLine(Convert.FromBase64String(rec));
                    Message message = jmess.ToValue<Message>();
                    Messages.AddMessage(message);
                    Send(handler, JMessage.Serialize(JMessage.FromValue<IList<CommonApp.Model.Message>>(Messages.GetMessagesOfUser(message.UserNameFrom), Header.Message)));
                    break;

                case Header.Messages:
                    string user = JMessage.Deserialize(rec).ToValue<string>();
                    Send(handler, JMessage.Serialize(JMessage.FromValue<IList<CommonApp.Model.Message>>(Messages.GetMessagesOfUser(user), Header.Messages)));
                    break;

                case Header.Users:
                    jmess = JMessage.FromValue<Dictionary<string, string>>(Users.UsersDict, Header.Users);
                    Send(handler, JMessage.Serialize(jmess));
                    break;
                case Header.Handshake:
                    SendKeyAndArray(handler);
                    break;


            }
        }
        public void GenerateKeyAndArray() {
            rijndael = new RijndaelManaged();
            rijndael.GenerateKey();
            rijndael.GenerateIV();

        }
        public void SendKeyAndArray(Socket handler) {

           
            Console.WriteLine("Key {0} ", Convert.ToBase64String(rijndael.Key));
            Console.WriteLine("IV {0} ", Convert.ToBase64String(rijndael.IV));
            KeyValuePair<byte[],byte[]> msg = new KeyValuePair<byte[],byte[]>( rijndael.Key,rijndael.IV);
            string json = JMessage.Serialize(JMessage.FromValue<KeyValuePair<byte[], byte[]>>(msg, Header.Handshake));
            Console.WriteLine(json);
  
            handler.Send(Encoding.ASCII.GetBytes(json));
        }


        public void StartListening()
        {

            //Rijndael rijn = new RijndaelManaged();
            //byte[] key = rijn.Key;
            //byte[] arr = rijn.IV;
            //byte[] encr = CommonApp.RijndaelClass.EncryptStringToBytes("hey", key, arr);
            //string res = CommonApp.RijndaelClass.DecryptStringFromBytes(encr, key, arr);
            //Console.WriteLine(res);



            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];
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
                        //try to deserialize (in case it's not encrypted, it will do)
                        jmessage = CommonApp.JMessage.Deserialize(msg);
                        UnpackAndSend(handler, msg, jmessage.Type);
                        Console.WriteLine("Handshake done");
                    }
                    catch (Exception ) {
                        //encrypted, so decrypt and deserialize
                        Console.WriteLine("Encrypted message receive");
                        CommonApp.RijndaelClass.TruncateBytesArray(ref bytesNew);
                        Console.WriteLine("Truncated array {0}", Convert.ToBase64String(bytesNew));

                        string decr = CommonApp.RijndaelClass.DecryptStringFromBytes(bytesNew, rijndael.Key, rijndael.IV);
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

    }
}
