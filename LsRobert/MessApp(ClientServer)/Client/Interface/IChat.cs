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
    public interface IChat
    {
        event EventHandler<SendMessageArgs> SendedMessage;
        event EventHandler<ShowUsers> ShowUserInDataGrid;
         event EventHandler<PrivateChatUser> ShowPrivateChat;
        event EventHandler<SendMessageArgs> SendedMessagePrivateChat;


        void sendMessage(String s, String handler);
        List<String> getListOfUsers();
      


        void alwaysRead();
    }
}
