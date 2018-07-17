using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Events
{
    class Handles
    {
        public delegate void SendEventHandle(object source, SendMessageArgs args);

    }
}
