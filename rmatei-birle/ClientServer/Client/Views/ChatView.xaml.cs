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
using Client.Logic.Services;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        private readonly IChatService _chatService;
        private string _currentUser;
        private List<string> _onlineUsers;
        private string _mostRecentUnreadChat = null;
        public string Me;

        public ChatView()
        {
            lock (this)
            {
                InitializeComponent();
                _onlineUsers = new List<string>();
                Closing += ChatView_Closing;
                _chatService = new ChatService();
                _chatService.MessageToView += ChatService_MessageToView;
                _chatService.CommandAddToView += _chatService_CommandAddToView;
                _chatService.CommandRemoveToView += _chatService_CommandRemoveToView;
            }
        }

        private void _chatService_CommandRemoveToView(object sender, EventArguments.CommandRemoveToViewEventArgs e)
        {
            OnlineUsers.Dispatcher.BeginInvoke((Action)(() =>
            {
                _onlineUsers.Remove(e.User);
                OnlineUsers.Items.Clear();
                foreach (string usr in _onlineUsers)
                {
                    OnlineUsers.Items.Add(new { User = usr });
                }

            }));
        }

        private void _chatService_CommandAddToView(object sender, EventArguments.CommandAddToViewEventArgs e)
        {
            OnlineUsers.Dispatcher.BeginInvoke((Action)(() =>
            {
                _onlineUsers.Add(e.User);
                OnlineUsers.Items.Clear();
                foreach (string usr in _onlineUsers)
                {
                    OnlineUsers.Items.Add(new { User = usr });
                }
            }));
        }

        private void ChatView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _chatService.Logout();
        }

        private void ChatService_MessageToView(object sender, EventArguments.MessageToViewEventArgs e)
        {
            ChatTextBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        this._mostRecentUnreadChat = e.Username;
                        this.Title += " - New message from: " + this._mostRecentUnreadChat;
                        GetChat();
                    }
                ));
        }

        private void GetChat()
        {
            List<Tuple<string, string>> messages = _chatService.GetMessages(_currentUser);
            StringBuilder sb = new StringBuilder();

            foreach (Tuple<string, string> msg in messages)
            {
                sb.Append(">> ");
                sb.Append(msg.Item1);
                sb.Append(": ");
                sb.Append(msg.Item2);
                sb.Append("\n");
            }
            ChatTextBox.Text = sb.ToString();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                if (UserInputText.Text != "")
                {
                    _chatService.SendMessage(UserInputText.Text, _currentUser);
                    UserInputText.Text = "";
                    GetChat();
                }
            }
        }

        private void OnlineUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _currentUser = OnlineUsers.SelectedValue.ToString().Split('=')[1].Split(' ')[1];

                if (_currentUser == _mostRecentUnreadChat)
                {
                    this.Title = this.Me + "'s Chat";
                }

                if (_currentUser != null)
                {
                    GetChat();
                }
            }
            catch { }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _chatService.Logout();
            this.Close();
        }
    }
}
