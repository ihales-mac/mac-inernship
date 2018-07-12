using Client.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Contracts
{
    public interface IChatService
    {
        event MessageToViewEventHandler MessageToView;
        event CommandToViewEventHandler CommandToView;
        void SendMessage(string message, string user);
        List<Tuple<string, string>> GetMessages(string user);
        List<string> GetOnlineUsers();
        void Logout();
    }
}
