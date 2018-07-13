using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthentificationService auth;
        public MainWindow()
        {
            InitializeComponent();
            auth = new AuthentificationService();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            auth.CloseConnection();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password;
            string username = txtUsername.Text;
            string results = auth.Authenticate(username, password);
            if (results.Contains("Login succesful!"))
            {
                Chat chat = new Chat(auth.cs);
                chat.Closed += Chat_Closed;
                chat.Show();
                this.Hide();
            }
            else
                auth.CloseConnection();
            lblLoginStatus.Text = results;
        }

        private void Chat_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
