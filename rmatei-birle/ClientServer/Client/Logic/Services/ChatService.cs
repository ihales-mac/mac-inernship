using Client.Communication.Contracts;
using Client.Communication.Services;
using Client.EventArguments;
using Client.Logic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Services
{
    class ChatService : IChatService
    {
        public event MessageToViewEventHandler MessageToView;
        public event CommandToViewEventHandler CommandToView;
        ICommunication Communication;
        private List<Tuple<string, string, string>> Messages; //item1 = whose chat, item2 = whose message, item3 = message
        private List<string> OnlineUsers;

        //public ChatService(ICommunication comm)
        //{
        //    this.Communication = comm;
        //    this.Communication.IncomingMessage += Communication_IncomingMessage;
        //    Messages = new List<Tuple<string, string, string>>();
        //    OnlineUsers = new List<string>();
        //}

        public ChatService()
        {
            this.Communication = SocketServices.Instance;
            this.Communication.IncomingMessage += Communication_IncomingMessage;
            Messages = new List<Tuple<string, string, string>>();
            OnlineUsers = new List<string>();
        }

        public List<string> GetOnlineUsers()
        {
            return this.OnlineUsers;
        }

        public List<Tuple<string, string>> GetMessages(string user)
        {
            List<Tuple<string, string>> messages = new List<Tuple<string, string>>();

            foreach(Tuple<string,string,string> msg in this.Messages)
            {
                if(msg.Item1 == user)
                {
                    messages.Add(new Tuple<string, string>(msg.Item2, msg.Item3));
                }
            }

            return messages;
        }

        public void Logout()
        {
            Communication.SendMessage("$$LOGOUT");
        }

        public void SendMessage(string message, string user)
        {
            Messages.Add(new Tuple<string, string, string>(user, "ME", message));
            OnMessageToView(new MessageToViewEventArgs());
            StringBuilder sb = new StringBuilder();

            sb.Append("$$CHAT=");
            sb.Append(user);
            sb.Append("$$MSG=");
            sb.Append(message);
            Communication.SendMessage(sb.ToString());
        }

        private void Communication_IncomingMessage(object sender, EventArguments.IncomingMessageEventArgs e)
        {
            //message arrives in format: $$CHAT=from_whom$$MSG=incoming_message or $$CMD$$ACTION=what_action$$VALUE=what_value

            string[] SplitInput = e.Message.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);

            if (SplitInput[0] == "CMD")
            {
                //if command deal with command
                string action = SplitInput[1].Split('=')[1];
                string value = SplitInput[2].Split('=')[1];
                if (action == "ADD")
                {
                    OnlineUsers.Add(value);
                    OnCommandToView(new CommandToViewEventArgs());
                }
                if(action == "REMOVE")
                {
                    OnlineUsers.Remove(value);
                    OnCommandToView(new CommandToViewEventArgs());
                }
            }
            else
            {
                //if not command then chat
                string fromWho = SplitInput[0].Split('=')[1];
                string msg = SplitInput[1].Split('=')[1];

                Messages.Add(new Tuple<string, string, string>(fromWho, fromWho, msg)); // chat, whose message, message
                OnMessageToView(new MessageToViewEventArgs());
            }
        }

        protected void OnMessageToView(MessageToViewEventArgs e)
        {
            if (MessageToView != null)
                MessageToView(this, e);
        }

        protected void OnCommandToView(CommandToViewEventArgs e)
        {
            if (CommandToView != null)
                CommandToView(this, e);
        }
    }
}
