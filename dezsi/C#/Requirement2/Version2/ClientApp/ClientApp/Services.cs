using CommonApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;


namespace ClientApp
{
    class Services
    {
        public Services() {
            

        }
       
        internal ICommunication<Socket> Handler { get; set; }
        public string User { get; set; }
        public string WriteTo { get; set; }
        public Dictionary<string, string> Users { get; set; }
        public IList<Message> Messages { get; set; }

        internal void NewHandler(string s)
        {
            if (s.Equals("sym"))
            Handler = new SocketNp.SynchronousSocketClientSym(User);
            else
                 Handler = new SocketNp.SynchronousSocketClientAsym(User);
        }

        internal void NewKey()
        {
            Handler.NewKey();
        }

        internal bool Login(string user, string pass)
        {
            User = user;
            
            Handler.SetUsername(user);
            return Handler.Login(user, pass);
        }

        internal void SetUp()
        {

            SetUsers();
            SetMessages();
        }


        internal void SetMessages()
        {
            Messages = Handler.GetMessages();
        }

        internal void SetUsers()
        {
            Users = Handler.GetUsers();
        }

        internal string GetUsername()
        {
            return User;
        }

        internal IList<Message> GetFilteredMessagesUser()
        {
            return  Messages.Where(m => m.UserNameFrom.Equals(User) || m.UserNameTo.Equals(User)).ToList<Message>();
        }
        internal IList<Message> GetFilteredMessagesUsers()
        {
            return Messages.Where(m => (m.UserNameFrom.Equals(User) && m.UserNameTo.Equals(WriteTo)) || (m.UserNameFrom.Equals(WriteTo) && m.UserNameTo.Equals(User))).ToList<Message>();

        }

        internal IList<Message> SendMessage(string text,  DateTime now)
        {
            Messages = Handler.SendMessage(User, WriteTo, text, now);
            return Messages;
        }
        public void SetUsername(string name)
        {
            User = name;
            Handler.SetUsername(name);
          
        }
    }
}
