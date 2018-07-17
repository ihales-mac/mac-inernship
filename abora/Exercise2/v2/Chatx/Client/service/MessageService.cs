using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Client.tcp;
using System.Windows.Controls;

namespace Client
{
    public class MessageService
    {
        public TCPClient _tCPClient { get; set; }

        public MessageService(TCPClient tcpClient)
        {
            this._tCPClient = tcpClient;
        }

        internal void ReadMessages(TextBox textBox)
        {
            while (true)
            {
                Thread.Sleep(5);
                Message message = new Message();
                message.ReadFrom(_tCPClient.getStream());
         
                Console.WriteLine(message.Body);
                textBox.Dispatcher.BeginInvoke((Action)(() =>
                {
                    textBox.Text += message.Body;
                    textBox.Text += "\n";
                }
                ));
            }
        }

        public void WriteMessage(string msg)
        {
            Message message = new Message();
            message.Header = "CHAT";
            message.Body = msg;
            message.WriteTo(_tCPClient.getStream());
        }
    }
}
