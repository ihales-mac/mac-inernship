using Client.Connection.ConnectionServices;
using Client.Connection.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    class ChatServices
    {
        IConnectionService cs;
        public ChatServices()
        {
            cs=new SocketConnectionService();
        }
        public ChatServices(IConnectionService cs)
        {
            this.cs = cs;
        }

        public void SendMessage(string sendTo, string message)
        {
            cs.SendMessageToUser( sendTo,  message);
        }

        public List<string> GetOnlineUsers()
        {
            return cs.GetOnlineUsers();
        }
    }
}
