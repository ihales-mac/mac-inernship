using Client.Events;
using Client.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
       

        ICommunication communication;
        IChat chatService;
        Thread listen;
        string usernameWindow3Owner;
        public void SendedMessage(object source, SendMessageArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                String oldMessage = textboxMessage.Text.ToString();
                
                textboxMessage.Text = oldMessage + e.message + "\n";
               
            });
        }

        public void ShowUsersInDataGrid(object source, ShowUsers e)
        {
            this.Dispatcher.Invoke(() =>
            {
                filligDataGrid(e.user);
            });
        }

         public void ShowPrivateChat(object source, PrivateChatUser e)
         {
             this.Dispatcher.Invoke(() =>
             {
                 //Console.WriteLine(usernameWindow3Owner);
                 PrivateChat win3 = new PrivateChat(communication, chatService,e.user);
                 win3.Show();
             });
         }
         

       

        public  Chat(ICommunication communication_,String username)    
        {
            this.communication = communication_;
            InitializeComponent();
           // clientSocket = client;
            chatService = new ChatService(communication);
            chatService.SendedMessage += SendedMessage;
            chatService.ShowUserInDataGrid += ShowUsersInDataGrid;
           chatService.ShowPrivateChat += ShowPrivateChat;
            label.Content = username;
            usernameWindow3Owner = username;
            listen = new Thread(messageFromServer);
            listen.Start();

        }

        private void messageFromServer()
        {
            chatService.alwaysRead();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            String mesaj = textboxSendMessage.Text;
            chatService.sendMessage(mesaj, "Send");



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            chatService.sendMessage("", "Show");
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            chatService.sendMessage("","Logout");
            listen.Abort();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine(usernameWindow3Owner);
                object item = dataGrid.SelectedItem;
                string username = (dataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                string message = textboxSendMessage.Text;
                string messageToSend = username + " " + message;
                chatService.sendMessage(messageToSend, "SendForOne");

                 PrivateChat win3 = new PrivateChat(communication, chatService, username);
                win3.Show();
               



            }
            catch (Exception ex)
            {
                MessageBox.Show("Selecteaza un utilizator cu care sa vorbesti");
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }
    }
}
