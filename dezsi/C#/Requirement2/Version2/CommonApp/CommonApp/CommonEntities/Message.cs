using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp.Model
{
    public class Message
    {
        public Message(string UsernameFrom, string UsernameTo, string Text, DateTime Date)
        {
            UserNameFrom = UsernameFrom;
            UserNameTo = UsernameTo;
            this.Text = Text;
            this.Date = Date;
        }

        public String UserNameFrom { get; }
        public String UserNameTo { get; }
        public String Text { get; }
        public DateTime Date { get; }





    }

}
