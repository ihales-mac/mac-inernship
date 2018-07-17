using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp.Model
{
    public class Message
    {
        private string _UsernameFrom;
        private string _UsernameTo;
        private string _Text;
        private DateTime _Date;

        public Message(string UsernameFrom, string UsernameTo, string Text, DateTime Date)
        {
            _UsernameFrom = UsernameFrom;
            _UsernameTo = UsernameTo;
            _Text = Text;
            _Date = Date;
        }

        public String UserNameFrom { get { return _UsernameFrom; } }
        public String UserNameTo { get { return _UsernameTo; } }
        public String Text { get { return _Text; } }
        public DateTime Date { get { return _Date; } }

        


   
    }

}
