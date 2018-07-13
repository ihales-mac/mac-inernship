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
using System.Net.Sockets;
using System.Threading;


namespace Clinet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TcpClient TcpClient;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Int32 port = 8080;
            TcpClient TcpClient = new TcpClient("localhost", port);

            string username = txbUsername.Text;
            string password = txbPassword.Text;

            byte[] usernameBytes;
            byte[] passwordBytes;
            byte[] separator;

            usernameBytes = Encoding.ASCII.GetBytes(username);
            passwordBytes = Encoding.ASCII.GetBytes(password);
            separator = Encoding.ASCII.GetBytes(";");
            byte[] result = new byte[usernameBytes.Length + passwordBytes.Length + separator.Length];

            int i = 0;
            foreach(byte b in usernameBytes)
            {
                result[i++] = b;
            }
            result[i++] = separator[0];

            foreach (byte b in passwordBytes)
            {
                result[i++] = b;
            }


            //get a client stream
            NetworkStream stream = TcpClient.GetStream();

            //write to stream
            stream.Write(result, 0, result.Length);

            //read response from server
            byte[] response = new byte[256];

            stream.Read(response, 0, 256);
            string responseStr = Encoding.ASCII.GetString(response);

            int size = responseStr.IndexOf('\0');
            responseStr = responseStr.Substring(0, size);
            Console.WriteLine(responseStr);
            if (responseStr.Equals("success"))
            {
                
                CharWindow charWindow = new CharWindow(TcpClient,username);
                this.Close();
                charWindow.Show();

            }
            else
            {
                lblError.Content = "Username or password is not correct";
            }


        }

    }
}
