using ClientApp.SocketNp;
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
        private ICommunication<Socket> client;
        public ICommand ButtonCommand { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            // ButtonCommand = new RelayCommand(ShowMessage, param => true);
           // ThreadedClient.Run();
        }

        private void ShowMessage(object sender)
        {
            MessageBox.Show(sender.ToString());
        }


        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {

            client = new SocketNp.SynchronousSocketClient2("generic");
            client.NewKey();
            client.Login(this.TextBoxName.Text, this.TextBoxPassword.Text);
            

        }
    }
}
