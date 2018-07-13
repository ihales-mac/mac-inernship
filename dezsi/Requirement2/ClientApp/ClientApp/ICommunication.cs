using CommonApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    interface ICommunication<T>
    {
        string SendAndReceiveMessage<Obj>(Obj obj, Header type = Header.Unspecified);
        string GetKey();
        string GetIV();
        IList<CommonApp.Model.Message> SendMessage(string to, string message, DateTime time);
        IList<CommonApp.Model.Message> GetMessages();
        Dictionary<string, string> GetUsers();
        void Login(string user, string pwd);
        string GetUserName();
        void SetUserName(string user);
    }
}
