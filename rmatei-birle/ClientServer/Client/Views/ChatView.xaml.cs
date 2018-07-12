using Client.Logic.Contracts;
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
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        IChatService ChatService;
        string CurrentUser;

        public ChatView(IChatService cs)
        {
            this.ChatService = cs;
            this.ChatService.MessageToView += ChatService_MessageToView;
            this.ChatService.CommandToView += ChatService_CommandToView;
            InitializeComponent();
            DataGridTextColumn col = new DataGridTextColumn();
            col.Header = "Online Users";
        }

        private void ChatService_CommandToView(object sender, EventArguments.CommandToViewEventArgs e)
        {
            PopulateOnlineUsers();
        }

        private void ChatService_MessageToView(object sender, EventArguments.MessageToViewEventArgs e)
        {
            PopulateChat();
        }

        private void PopulateOnlineUsers()
        {
            List<string> online = ChatService.GetOnlineUsers();

            OnlineUsers.Items.Add(online.First());
        }

        private void PopulateChat()
        {
            List<Tuple<string, string>> messages = ChatService.GetMessages(this.CurrentUser);
            StringBuilder sb = new StringBuilder();

            foreach (Tuple<string, string> msg in messages)
            {
                sb.Append(">>");
                sb.Append(msg.Item1);
                sb.Append(":");
                sb.Append(msg.Item2);
                sb.Append("\n");
            }
            ChatTextBox.Text = sb.ToString();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                ChatService.SendMessage(UserInputText.Text, this.CurrentUser);
            }
        }

        private void OnlineUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentUser = OnlineUsers.SelectedValue.ToString();
            PopulateChat();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            ChatService.Logout();
            this.Close();
        }
    }
}
