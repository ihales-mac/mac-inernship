using Client.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication.Contracts
{
    public interface ICommunication
    {
        event IncomingMessageEventHandler IncomingMessage;
        string Connect(string IP, string Port);
        string ListenOnce();
        void ListenContinuously();
        void StopListeningContinuously();
        void Disconnect();
        void SendMessage(string message);
        void SetIC(string IC);
    }
}
