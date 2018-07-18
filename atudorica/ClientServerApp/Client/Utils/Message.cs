using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utils
{
    public class Message
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }

        public Message(string sender,string receiver, string content)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Content = content;
        }
    }
}
