using CommonApp.Model;
using System;
using System.Collections.Generic;

namespace ServerApp
{
    public class Messages
    {
        public static IList<Message> messages = new List<Message> { new Message("user", "mary", "heyyy", new DateTime(2018,7,12,2,52,4,1,new System.Globalization.GregorianCalendar(),DateTimeKind.Local)), new Message("mary", "user", "hellooo", new DateTime(2018, 7, 12, 2, 52, 4, 1, new System.Globalization.GregorianCalendar(), DateTimeKind.Local)) };

        public static void AddMessage(Message message)
        {
            messages.Add(message);
            Console.WriteLine(messages.Count);
        }
        public static IList<Message> GetMessagesOfUser(String of)
        {
            IList<Message> list = new List<Message>();
            foreach (Message m in messages)
            {
                if (m.UserNameFrom.Equals(of) || m.UserNameTo.Equals(of))
                    list.Add(m);
            }
            return list;
        }
        public static IList<Message> GetMessagesBetweenUsers(String one, String another)
        {
            IList<Message> list = new List<Message>();
            foreach (Message m in messages)
            {
                if ((m.UserNameFrom.Equals(one) && m.UserNameTo.Equals(another))||((m.UserNameFrom.Equals(another) && m.UserNameTo.Equals(one))))
                    list.Add(m);
            }
            return list;
        }

    }
}
