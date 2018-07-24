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



    public class SynchronousSocketClientSym : SynchronousSocketClient
    {

        private static byte[] _key, _IV;
        public void SetUserName(string user) {
            _username = user;
        }
        public string GetUserName() {
            return _username;
        }

        public SynchronousSocketClientSym(String client){
            
            _username = client;


         }
        
        public T GetMessage<T>(string message) {
            JMessage mess = JMessage.Deserialize(message);
            return mess.ToValue<T>();

        }



     

        public string GetKey() {

            return Convert.ToBase64String(_key);
        }

        public string GetIV() {
            return Convert.ToBase64String(_IV);
        }



        internal override string DecryptMessage(byte[] byteMessage)
        {
            Console.WriteLine("key {0}, array {1}", Convert.ToBase64String(SynchronousSocketClientSym._key), Convert.ToBase64String(SynchronousSocketClientSym._IV));


            return CommonApp.RijndaelClass.DecryptStringFromBytes(byteMessage, _key, _IV);
        }

        internal override byte[] EncryptMessage(string msg, Header type = Header.Unspecified)
        {
            while (msg.Length % 16 != 0)
            {
                msg += '\0';

            }
            Console.WriteLine("key {0}, array {1}", Convert.ToBase64String(SynchronousSocketClientSym._key), Convert.ToBase64String(SynchronousSocketClientSym._IV));
            //byte[] bytes = 
            return CommonApp.RijndaelClass.EncryptStringToBytes(msg, _key, _IV);
        }

        public override void NewKey()
        {
            string msg = this.SendAndReceiveMessage("", Header.Handshake);
            KeyValuePair<byte[], byte[]> pair = JMessage.Deserialize(msg).ToValue<KeyValuePair<byte[], byte[]>>();
            _key = pair.Key;
            _IV = pair.Value;
        }

        public override IList<Message> SendMessage(string user, string writeTo, string text, DateTime now)
        {
            Message m = new Message(user, writeTo, text, now);
            string messages = this.SendAndReceiveMessage(m, Header.Message);
            return JMessage.Deserialize(messages).ToValue<IList<Message>>();
        }
    }
}
