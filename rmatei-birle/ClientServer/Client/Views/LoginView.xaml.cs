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

namespace Client
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    /// 
    public partial class LoginView : Window
    {
        public ILoginService LoginService;

        public IChatService ChatService;

        //public LoginView(ILoginService ls, IChatService cs)
        //{
        //    LoginService = ls;
        //    ChatService = cs;
        //    InitializeComponent();
        //}

        public LoginView()
        {
            LoginService = new LoginService();
            ChatService = new ChatService();
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string ip = IPText.Text;
            string port = PortText.Text;
            string username = UsernameText.Text;
            string password = PasswordText.Password;

            string response = LoginService.Login(username, password, ip, port);

            if(response != "success")
            {
                if(response == null)
                {
                    StatusText.Text = "Unknown error!";
                }
                else
                {
                    StatusText.Text = response;
                }
            }
            else
            {
                ChatView chat = new ChatView(ChatService);
                chat.Owner = this;
                this.Hide();
                chat.ShowDialog();
            }
        }
    }
}
