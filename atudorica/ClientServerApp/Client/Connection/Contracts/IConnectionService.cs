using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Utils.EventArguments;

namespace Client.Connection.Contracts
{
    public interface IConnectionService
    {
        event ClientListChangedEventHandler ClientListChanged;
        event MessageReceivedEventHandler MessageReceived;
        string Authenticate(string username, string password);
        void SendMessageToUser(string destination, string message);
        void CloseConnection();
        string SendMessage(string message);
        List<string> GetOnlineUsers();
    }
}
