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
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (txtMessageBox.Text != null && lstContacts.SelectedItem!=null)
            {
                txtChatBox.Text += "\nme: " + txtMessageBox.Text;
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
                    txtChatBox.Text += "\nme: " + txtMessageBox.Text;
                    cs.SendMessage(lstContacts.SelectedItem.ToString(), txtMessageBox.Text);
                    txtMessageBox.Text = "";
                }
            }
        }
    }
}
