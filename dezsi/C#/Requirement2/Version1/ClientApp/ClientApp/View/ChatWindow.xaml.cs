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
        private string user;
        private string writeTo;
        private Dictionary<string,string> users;
        private IList<Message> messages;
        ICommunication<Socket> client;


        public ChatWindow()
        {

            InitializeComponent();

        }

        public void SetUpUser() {
            client = new SocketNp.SynchronousSocketClient2(user);
            GetUsers();
            GetMessages();
            this.messagesFrom.ItemsSource = users.Keys.ToDictionary(e => e);

            messages = filterMessages(messages);
            this.ChatBox.ItemsSource = messages;
        }
        //a second setup based on user data
        public void SetUserName(string username) {

            user = username;
            this.Title = username;
            SetUpUser();
        }

        private void messagesFrom_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
             
            string getUser = ((KeyValuePair<string, string>)(this.messagesFrom.SelectedItem)).Value;
            writeTo = getUser;
          
            this.ChatLabel.Content = " You can now text " + writeTo;
            GetMessages();
            IList<Message> msg= filterMessages(messages, user, writeTo);
            this.ChatBox.ItemsSource = msg;

        }
        //all messages of user
        private IList<Message> filterMessages(IList<Message>messages) {
            return messages.Where(m => m.UserNameFrom.Equals(user) || m.UserNameTo.Equals(user)).ToList<Message>();
        }

        //Discussions between the two
        private IList<Message> filterMessages(IList<Message> messages, String from, String to) {
            return messages.Where(m => (m.UserNameFrom.Equals(from) && m.UserNameTo.Equals(to))||(m.UserNameFrom.Equals(to) && m.UserNameTo.Equals(from))).ToList<Message>();

        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            messages = client.SendMessage(writeTo, this.messageText.Text, DateTime.Now);

            this.ChatBox.ItemsSource = filterMessages(messages, user, writeTo);


        }

        private void GetMessages() {
            messages = client.GetMessages();
        }

        private void GetUsers() {

            users = client.GetUsers();
        }

    }
}
