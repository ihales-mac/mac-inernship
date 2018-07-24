using Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Client.ChatService;
using static Client.Events.Handles;

namespace Client
{
    interface IChat
    {
         void sendMessage(String s);
        List<String> getListOfUsers();

        event EventHandler<SendMessageArgs> SendedMessage;
    }
}
