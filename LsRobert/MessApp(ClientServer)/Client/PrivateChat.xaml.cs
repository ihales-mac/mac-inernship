using Client.Events;
using Client.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for PrivateChat.xaml
    /// </summary>
    public partial class PrivateChat : Window
    {

        public void SendedMessagePrivateChat(object source, SendMessageArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                String oldMessage = textbox1.Text.ToString();

                textbox1.Text = oldMessage + e.message + "\n";
                textbox2.Clear();

            });
        }

       
        

        ICommunication communication;
        IChat chatService;
        string username;
        public PrivateChat(ICommunication communication_, IChat chatServervice_,string username_)
        {
            InitializeComponent();
            this.communication = communication_;
            this.chatService = chatServervice_;
            this.username = username_;
            chatService.SendedMessagePrivateChat += SendedMessagePrivateChat;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = textbox2.Text;
            string messageToSend = username + " " + message;
            chatService.sendMessage(messageToSend, "SendForOnePrivate");
        }
    }
}
