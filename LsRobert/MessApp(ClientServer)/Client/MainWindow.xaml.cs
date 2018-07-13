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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientService ClientService = new ClientService();

        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String username = textbox1.Text.ToString();
            String password = textbox2.Text.ToString();
            if (ClientService.login(username, password) == false)
            {
                MessageBox.Show("Nume sau parola gresita");
            }
            else
            {
                Chat win2 = new Chat(ClientService.getClient());
                win2.Show();
                this.Close();


            }
        }
    }
}
