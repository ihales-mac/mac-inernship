using Client.Communication.Services;
using Client.Logic.Contracts;
using Client.Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    /// 
    public partial class LoginView : Window
    {
        private readonly ILoginService _loginService;
        private IMessageService _messageService = new MessageService();


        public LoginView()
        {
            lock (this)
            {
                InitializeComponent();
                _loginService = new LoginService();
                _messageService.LoginEvent += _messageService_LoginEvent;
            }
        }

        private void _messageService_LoginEvent(object sender, EventArguments.LoginEventArgs e)
        {
            string response = e.Response;
            if (response != "success")
            {
                if (response == null)
                {
                    StatusText.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        StatusText.Text = "Unknown error!";
                    }));
                }
                else
                {
                    StatusText.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        StatusText.Text = response;
                    }));
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ChatView chat = new ChatView();
                    this.Hide();
                    chat.Closed += Chat_Closed;
                    chat.ShowDialog();
                });
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string ip = IPText.Text;
            string port = PortText.Text;
            string username = UsernameText.Text;
            string password = PasswordText.Password;

            _loginService.Login(username, password, ip, port);
        }

        private void Chat_Closed(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                this.Close();
            });
        }
    }
}
