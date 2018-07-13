using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Events
{
   public  class SendMessageArgs:EventArgs
    {
        public string message { get; set; }
    }
}
