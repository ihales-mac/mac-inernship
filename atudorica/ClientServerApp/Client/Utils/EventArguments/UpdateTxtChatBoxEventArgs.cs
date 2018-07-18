using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utils.EventArguments
{
    public delegate void UpdateTxtChatBoxEventHandler(object sender, UpdateTxtChatBoxEventArgs e);
    public class UpdateTxtChatBoxEventArgs : EventArgs
    {
        public string conversation;
        public string content;
        public UpdateTxtChatBoxEventArgs(string cont,string conv)
        {
            this.content = cont;
            this.conversation = conv;
        }
    }
}
