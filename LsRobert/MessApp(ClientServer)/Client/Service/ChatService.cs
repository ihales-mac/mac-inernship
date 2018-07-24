using Client.Events;
using Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Client.Events.Handles;

namespace Client
{
  
    class ChatService:IChat
    {
        ICommunication communication;
        String messageToAllClients = "";
        public  String usernameOwner;

        public event EventHandler<SendMessageArgs> SendedMessage;
        public event EventHandler<ShowUsers> ShowUserInDataGrid;
        public event EventHandler<PrivateChatUser> ShowPrivateChat;
        public event EventHandler<SendMessageArgs> SendedMessagePrivateChat;

        public ChatService(ICommunication communication_) { this.communication = communication_; }

        public void alwaysRead()
        {
            while (true)
            {
                String message = Common.Cryption.DecryptMessage(communication.getMessage());
                interprate(message);
            }
        }
        #region Handlers
        public virtual void OnSendMessage()
        {
            if (SendedMessage != null)
            {
                SendedMessage(this, new SendMessageArgs() { message = messageToAllClients });
            }
        }

        public virtual void OnShow()
        {
            if (ShowUserInDataGrid != null)
            {
                ShowUserInDataGrid(this, new ShowUsers() { user = getListOfUsers() });
            }
        }

        public  virtual void OnShowPrivateChat()
        {
            if (ShowPrivateChat != null)
            {
                ShowPrivateChat(this, new PrivateChatUser() { user=usernameOwner});
            }
        }
        
        public virtual void OnSendedMessagePrivateChat()
        {
            if (SendedMessagePrivateChat != null)
            {
                SendedMessagePrivateChat(this, new SendMessageArgs() { message = messageToAllClients });
            }
        }


        #endregion
        private void interprate(String message)
        {

            string[] words = message.Split();
            usernameOwner = words[1];
            messageToAllClients = convertMessage(words);
           

            switch (words[0])
            {
                case "Send":
                    // getMessage(mesajCareSeTransmiteLaToti);
                   // mesajCareSeTransmiteLaToti = message;
                     OnSendMessage();
                    break;
                case "Show":
                    OnShow();
                    break;
                case "SendForOne":
                
                    OnShowPrivateChat();
                    OnSendedMessagePrivateChat();
                    break;
                case "SendForOnePrivate":
                    OnSendedMessagePrivateChat();
                    break;
                default:
                    break;
            }
        }


     
       
        #region FunctionForInterpretingTheMessage
        public void sendMessage(String s,String handler)
        {
            String toSend= handler + " " + s;
            String messageToSend = Common.Cryption.EncryptMessage(toSend);
            communication.SendMessage(messageToSend);
          
        }


       
        public List<String> getListOfUsers()
        {
            List<String> userList = new List<String>();
            String[] words;
          
          words = messageToAllClients.Split(' '); //In messageToAllClients are the users
            for(int i=0; i<words.Length;i++)
            {
                userList.Add(words[i]);
            }
         
            return userList;
        }


        //Make a new message without the first words(which is "Send","Show",etc)
        private string convertMessage(string[] words)
        {
            String message2 = "";
            for (int i = 1; i < words.Length - 1; i++)
                message2 += words[i] + " ";
            return message2;
        }

        #endregion

        
      

       
    }
}
