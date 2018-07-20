using ClientApp.SocketNp;
using ClientApp.View;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Services client;
        public MainWindow()
        {
            InitializeComponent();
            client = new Services();
        }

        private void ShowMessage(object sender)
        {
            MessageBox.Show(sender.ToString());
        }


        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {

            client.NewHandler("sym");
           
  
           
           
            bool auth = client.Login(this.TextBoxName.Text, this.TextBoxPassword.Text); //client knows about username

            if (auth)
            {
                ChatWindow chat = new ChatWindow();

                chat.AddService(client);
                chat.SetUpUser();
        
                chat.Title = this.TextBoxName.Text;
                chat.Show();

            }
            else {
                MessageBox.Show("Sorry, incorrect username or password. Try again.");
            }

        }
    }
}
