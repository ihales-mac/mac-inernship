using Client.tcp;
using Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginService _loginService;
        private MessageService _messageService;
        private TCPClient _tCPClient;


        public MainWindow()
        {
            InitializeComponent();
            _tCPClient = new TCPClient("localhost", 8080);
            _loginService = new LoginService(_tCPClient);
            _messageService = new MessageService(_tCPClient);

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassword.Text;

            bool isAccepted = this._loginService.CheckLogin(username, password);
            if (isAccepted)
            {
           
                ChatWindow chatWindow = new ChatWindow(username, _messageService);
                this.Close();
                chatWindow.Show();
            }
            else
            {
                Error error = new Error();
                this.Close();
                error.Show();
            }
        }
    }
}
