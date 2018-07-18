using Client.Connection.ConnectionServices;
using Client.Connection.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Utils;
using Client.Utils.EventArguments;

namespace Client.Services
{
    class ChatServices
    {
        public event UpdateTxtChatBoxEventHandler UpdateChatBox;
        public IConnectionService cs;
        private static List<Message> _messages;
        public ChatServices()
        {
            cs = new SocketConnectionService();
            _messages = new List<Message>();
            cs.MessageReceived += ConnectionService_messageReceived;
        }

        public ChatServices(IConnectionService cs)
        {
            this.cs = cs;
            _messages = new List<Message>();
            cs.MessageReceived += ConnectionService_messageReceived;
        }

        public string getChatText(string conversation)
        {
            string result=null;
            foreach (Message m in _messages)
            {
                if (m.Sender == conversation || m.Receiver == conversation)
                    result += m.Sender + ": " + m.Content + "\n";
            }
            return result;
        }

        public void SendMessage(string sendTo, string message)
        {
            Message m =new Message("me",sendTo,message);
            _messages.Add(m);
            cs.SendMessageToUser(sendTo, message);
            OnUpdateTxtChatBox(new UpdateTxtChatBoxEventArgs(getChatText(sendTo),sendTo));
        }

        private void ConnectionService_messageReceived(object sender,
            Utils.EventArguments.MessageReceivedEventArgs e)
        {
            _messages.Add(e.m);
            OnUpdateTxtChatBox(new UpdateTxtChatBoxEventArgs(getChatText(e.m.Sender),e.m.Sender));
        }

        public List<string> GetOnlineUsers()
        {
            return cs.GetOnlineUsers();
        }

        protected  void OnUpdateTxtChatBox(UpdateTxtChatBoxEventArgs e)
        {
            if (UpdateChatBox != null)
            {
                UpdateChatBox(this, e);
            }
        }
    }
}
