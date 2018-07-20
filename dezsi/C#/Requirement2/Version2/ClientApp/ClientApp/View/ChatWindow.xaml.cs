using CommonApp;
using CommonApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.View
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow :  Window
    {
        Services _client;

        public ChatWindow()
        {

            InitializeComponent();

        }
      
        public void SetUpUser() {

            
            _client.SetUp();

            this.messagesFrom.ItemsSource = _client.Users.Keys.ToDictionary(e => e);
            this.ChatBox.ItemsSource = _client.GetFilteredMessagesUser();
        }

        internal void AddService(Services client)
        {
            _client = client;
        }

        //a second setup based on user data
        public void SetUserName(string username) {

            _client.User = username;
            
         
        }

        private void messagesFrom_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
             
            string getUser = ((KeyValuePair<string, string>)(this.messagesFrom.SelectedItem)).Value;
            _client.WriteTo = getUser;
          
            this.ChatLabel.Content = " You can now text " + _client.WriteTo;
            _client.SetMessages();
            IList<Message> msg = _client.GetFilteredMessagesUsers();
            this.ChatBox.ItemsSource = msg;

        }


        //Discussions between the two
        

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            _client.SendMessage(this.messageText.Text, DateTime.Now);
            _client.SetMessages();
            this.ChatBox.ItemsSource = (IEnumerable<Message>)_client.GetFilteredMessagesUsers();


        }

    }
}
