using Client.tcp;
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
using System.Net.Sockets;
using System.Threading;
using Common;

namespace Client
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private string _username;
        private MessageService _messageService;
        Thread ListenThread;

        public ChatWindow(string username,MessageService msgService)
        {
            InitializeComponent();
            _username = username;
            _messageService = msgService;

            msgService.MessageReceived += this.OnMessageReceived;

            lblGreeting.Content = "Hello " + username;
            ListenThread = new Thread(getMessage);
            ListenThread.Start();
        }

        public void OnMessageReceived(object source,MessageEventArgs e)
        {
            Message message = e.Message;
            this.txbChat.Dispatcher.BeginInvoke((Action)(() =>
            {
                this.txbChat.Text += message.Body;
                this.txbChat.Text += "\n";
            }
            ));
        }


        private void getMessage()
        {
            
            this._messageService.ReadMessages();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string msg = txbMessage.Text;
            _messageService.WriteMessage(_username + " : " + msg);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            
            _messageService.Close(_username);
            ListenThread.Abort();
            this.Close();
        }
    }
}
