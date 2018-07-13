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


namespace Client
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private string _username;
        private MessageService _messageService;

        public ChatWindow(string username,MessageService msgService)
        {
            InitializeComponent();
            _username = username;
            _messageService = msgService;

            lblGreeting.Content = "Hello " + username;
            Thread thread = new Thread(getMessage);
            thread.Start();
        }


       
        private void getMessage()
        {
         
            
            this._messageService.ReadMessages(this.txbChat);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string msg = txbMessage.Text;
            _messageService.WriteMessage(_username + " : " + msg);
        }
    }
}
