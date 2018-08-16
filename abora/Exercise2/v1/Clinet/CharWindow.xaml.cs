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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Clinet
{
    /// <summary>
    /// Interaction logic for CharWindow.xaml
    /// </summary>
    public partial class CharWindow : Window
    {
        TcpClient TcpClient;
        public CharWindow(TcpClient tcpClient,string username)
        {
            this.TcpClient = tcpClient;
            InitializeComponent();
            this.lblGreeting.Content = "Hello " + username;
            Thread thread = new Thread(getMessage);
            thread.Start();
            //stream.Close();
            //tcpClient.Close();
        }

        private void getMessage()
        {
            while (true)
            {
                Thread.Sleep(5);
                NetworkStream stream = TcpClient.GetStream();
                byte[] data = new byte[1024];
                stream.Read(data, 0, 1024);
                string msg = Encoding.ASCII.GetString(data);
                msg = msg.Substring(0, msg.IndexOf('\0'));
                this.Dispatcher.Invoke(() =>
                {
                    txbChat.Text += (msg + "\n");
                });
                //txbChat.Invoke(new Action(() => txbChat.AppendText(msg + "\n")));
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string msgToSend = txbMessage.Text;
            byte[] data = Encoding.ASCII.GetBytes(msgToSend);
            int size = Encoding.ASCII.GetByteCount(msgToSend);
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(data,0,size);
        }
    }
}
