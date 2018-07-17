using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Communication.Contracts;
using Client.Communication.Services;
using Client.EventArguments;
using Client.Logic.Contracts;

namespace Client.Logic.Services
{
    class MessageService : IMessageService
    {
        public event LoginEventHandler LoginEvent;
        public event OnlineUserEventHandler OnlineUserEvent;
        public event MessageEventHandler MessageEvent;

        private readonly ICommunication _communication = SocketServices.Instance;

        public MessageService()
        {
            this._communication.IncomingMessage += Communication_IncomingMessage;
        }

        private void Communication_IncomingMessage(object sender, IncomingMessageEventArgs e)
        {
            //message arrives in format: $$CHAT=from_whom$$MSG=incoming_message or $$CMD$$ACTION=what_action$$VALUE=what_value
            //$$REJECTED$$REASON=why_rejected
            //$$ACCEPTED$$IC=identification_code
            string[] SplitResponse = e.Message.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);

            if (SplitResponse[0] == "REJECTED")
            {
                LoginEvent?.Invoke(this, new LoginEventArgs(SplitResponse[1].Split('=')[1]));
            }

            if (SplitResponse[0] == "ACCEPTED")
            {
                string IC = SplitResponse[1].Split('=')[1];
                _communication.SetIc(IC);
                LoginEvent?.Invoke(this, new LoginEventArgs("success"));
            }

            if (SplitResponse[0] == "CMD")
            {
                //if command deal with command
                string action = SplitResponse[1].Split('=')[1];
                string value = SplitResponse[2].Split('=')[1];
                if (action == "ADD")
                {
                    new Thread(() =>
                    {
                        OnlineUserEvent?.Invoke(this, new OnlineUserEventArgs("ADD", value));
                    }
                    ).Start();
                }

                if (action == "REMOVE")
                {
                    new Thread(() =>
                    {
                        OnlineUserEvent?.Invoke(this, new OnlineUserEventArgs("REM", value));
                    }
                    ).Start();
                }
            }

            if (SplitResponse[0].Contains("CHAT"))
            {
                //if not command then chat
                string fromWho = SplitResponse[0].Split('=')[1];
                string msg = SplitResponse[1].Split('=')[1];

                MessageEvent?.Invoke(this,
                    new MessageEventArgs(new Tuple<string, string, string>(fromWho, fromWho, msg)));
            }
        }
    }
}