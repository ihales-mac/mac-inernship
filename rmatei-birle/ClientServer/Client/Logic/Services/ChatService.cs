using Client.Communication.Contracts;
using Client.Communication.Services;
using Client.EventArguments;
using Client.Logic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Logic.Services
{
    class ChatService : IChatService
    {
        public event MessageToViewEventHandler MessageToView;
        public event CommandAddToViewEventHandler CommandAddToView;
        public event CommandRemoveToViewEventHandler CommandRemoveToView;
        private readonly ICommunication _communication;
        private List<Tuple<string, string, string>> _messages; //item1 = whose chat, item2 = whose message, item3 = message

        private IMessageService _messageService = new MessageService();

        public ChatService()
        {
            _communication = SocketServices.Instance;

            _messageService.OnlineUserEvent += _messageService_OnlineUserEvent;
            _messageService.MessageEvent += _messageService_MessageEvent;

            _messages = new List<Tuple<string, string, string>>();
            _communication.SendMessage("$$GETUSERS");
        }

        private void _messageService_MessageEvent(object sender, MessageEventArgs e)
        {
            _messages.Add(e.Message);
            MessageToView?.Invoke(this, new MessageToViewEventArgs());
        }

        private void _messageService_OnlineUserEvent(object sender, OnlineUserEventArgs e)
        {
            if (e.Action == "ADD")
            {
                new Thread(() =>
                    {
                        CommandAddToView?.Invoke(this, new CommandAddToViewEventArgs(e.User));
                    }
                ).Start();
            }
            if (e.Action == "REM")
            {
                new Thread(() =>
                    {
                        CommandRemoveToView?.Invoke(this, new CommandRemoveToViewEventArgs(e.User));
                    }
                ).Start();
            }
        }

        public List<Tuple<string, string>> GetMessages(string user)
        {
            List<Tuple<string, string>> messages = new List<Tuple<string, string>>();

            foreach (Tuple<string, string, string> msg in _messages)
            {
                if (msg.Item1 == user)
                {
                    messages.Add(new Tuple<string, string>(msg.Item2, msg.Item3));
                }
            }

            return messages;
        }

        public void Logout()
        {
            _communication.SendMessage("$$LOGOUT");
        }

        public void SendMessage(string message, string user)
        {
            _messages.Add(new Tuple<string, string, string>(user, "ME", message));
            StringBuilder sb = new StringBuilder();

            sb.Append("$$CHAT=");
            sb.Append(user);
            sb.Append("$$MSG=");
            sb.Append(message);
            _communication.SendMessage(sb.ToString());
        }
    }
}
