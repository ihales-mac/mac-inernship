using CommonApp;
using CommonApp.Model;
using System;
using System.Collections.Generic;


namespace ClientApp
{
    interface ICommunication<T>
    {
        void SetUsername(string name);
        string SendAndReceiveMessage<Obj>(Obj obj, Header type = Header.Unspecified);
        void NewKey();
        bool Login(string user, string pwd);
        IList<Message> GetMessages();
        Dictionary<string, string> GetUsers();
        IList<Message> SendMessage(string user, string writeTo, string text, DateTime now);
    }
}
