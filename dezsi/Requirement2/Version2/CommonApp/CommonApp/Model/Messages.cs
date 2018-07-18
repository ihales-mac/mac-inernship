using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{

    public class Message
    {
        private string _UsernameFrom;
        private string _UsernameTo;
        private string _Text;
        private DateTime _Date;

        public String UserNameFrom { get { return _UsernameFrom; } }
        public String UserNameTo { get { return _UsernameTo; } }
        public String Text { get { return _Text; } }
        public DateTime Date {get{return _Date;} }
        public Message(string UsernameFrom, string UsernameTo, string Text, DateTime Date)
        {
            _UsernameFrom = UsernameFrom;
            _UsernameTo = UsernameTo;
            _Text = Text;
            _Date = Date;
        }
    }
    public class Messages
    {
        public static IList<Message> messages = new List<Message> { new Message("user1", "usr2", "heyyy", new DateTime()), new Message("usr2", "user1", "k", new DateTime()) };

        public static void AddMessage(string usernameFrom, string usernameTo, string text, DateTime date) {
            messages.Add(new Message(usernameFrom, usernameTo, text, date));
        } 
        public static IList<Message> GetMessagesFromUserToUser(String from, String to) {
            IList<Message> list = new List<Message>();
            foreach (Message m in messages){
                if (m.UserNameFrom.Equals(from) && m.UserNameTo.Equals(to))
                    list.Add(m);
            }
            return list;
        }

    }
}
