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
        event CommandAddToViewEventHandler CommandAddToView;
        event CommandRemoveToViewEventHandler CommandRemoveToView;
        void SendMessage(string message, string user);
        List<Tuple<string, string>> GetMessages(string user);
        void Logout();
    }
}
