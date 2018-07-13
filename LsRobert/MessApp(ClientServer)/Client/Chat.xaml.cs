using Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        public TcpClient clientSocket;
        IChat chatService;
        public void SendedMessage(object source, SendMessageArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                String oldMessage = textboxMessage.Text.ToString();
                
                textboxMessage.Text = oldMessage + e.message + "\n";
               
            });
        }
       

        public Chat(TcpClient client)
        {
            InitializeComponent();
            clientSocket = client;
            chatService = new ChatService(clientSocket);
            chatService.SendedMessage += SendedMessage;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(mesajCareSeTransmiteLaToti);
          
            String mesaj = textboxSendMessage.Text;
            chatService.sendMessage(mesaj);
          


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           filligDataGrid (chatService.getListOfUsers());
        }
        public void filligDataGrid(List<String> userList)
        {
            DataTable dt = new DataTable();
            DataColumn user = new DataColumn("Users online", typeof(string));

            dt.Columns.Add(user);

            for (int i= 0; i < userList.Count; i++){
                DataRow dataRow = dt.NewRow();
                dataRow[0] = userList[i];

                dt.Rows.Add(dataRow);
            }

            dataGrid.ItemsSource = dt.DefaultView;
        }
    }
}
