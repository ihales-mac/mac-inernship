using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utils.EventArguments
{
    public delegate void ClientListChangedEventHandler(object sender, ClientListChangedEventArgs e);
    public class ClientListChangedEventArgs : EventArgs
    {
        public List<string> UsernamesList;
        public ClientListChangedEventArgs(List<string> ul)
        {
            this.UsernamesList = ul;
        }
    }
}
