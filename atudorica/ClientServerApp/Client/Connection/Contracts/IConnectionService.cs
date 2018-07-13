using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Connection.Contracts
{
    public interface IConnectionService
    {
        string Authenticate(string username, string password);
        void SendMessageToUser(string destination, string message);
        void CloseConnection();
        string SendMessage(string message);
        List<string> GetOnlineUsers();
    }
}
