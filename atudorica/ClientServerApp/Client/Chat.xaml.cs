using Client.Connection.Contracts;
using Client.Services;
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
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {

        ChatServices cs;
        public Chat(IConnectionService connectionService)
        {
            InitializeComponent();
            cs = new ChatServices(connectionService);
            lstContacts.ItemsSource = cs.GetOnlineUsers();
            cs.UpdateChatBox += txtChatBox_refresh;
            connectionService.ClientListChanged += lstContacts_refresh;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstContacts_refresh(object sender, Utils.EventArguments.ClientListChangedEventArgs e)
        {
            lstContacts.Dispatcher.BeginInvoke((Action)(() => lstContacts.ItemsSource = e.UsernamesList.ToArray()));
        }

        private void txtChatBox_refresh(object sender, Utils.EventArguments.UpdateTxtChatBoxEventArgs e)
        {
            lstContacts.Dispatcher.BeginInvoke((Action)(() =>
            {
            if (lstContacts.SelectedItem!=null && lstContacts.SelectedItem.ToString() == e.conversation)
            {
                txtChatBox.Dispatcher.BeginInvoke((Action) (() => txtChatBox.Text = e.content));
            }
            }));
        }

        private void lstContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = lstContacts.SelectedItem;
            txtChatBox.Dispatcher.BeginInvoke((Action) (() => txtChatBox.Text = cs.getChatText(item.ToString())));
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (txtMessageBox.Text != null && lstContacts.SelectedItem!=null)
            {
                cs.SendMessage(lstContacts.SelectedItem.ToString(), txtMessageBox.Text);
                txtMessageBox.Text = "";
            }

        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (txtMessageBox.Text != null && lstContacts.SelectedItem != null)
                {
                    cs.SendMessage(lstContacts.SelectedItem.ToString(), txtMessageBox.Text);
                    txtMessageBox.Text = "";
                }
            }
        }

    }
}
