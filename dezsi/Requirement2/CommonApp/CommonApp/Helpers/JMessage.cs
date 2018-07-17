using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    public enum Header
    {
        Unspecified,
        Login,
        Users,
        Messages,
        Message,
        Handshake,
        ExchangePKs,
        MessagePKPack


    }
    public class JMessage
    {
        public Header Type { get; set; }
        public JToken Value { get; set; }
        public T ToValue<T>() {
            return this.Value.ToObject<T>();
        }
        public static JMessage FromValue<T>(T value, Header type)
        {
            return new JMessage { Type = type, Value = JToken.FromObject(value) };
        }
     
        public static string Serialize(JMessage message)
        {
            return JToken.FromObject(message).ToString();
        }

        public static JMessage Deserialize(string data)
        {
            return JToken.Parse(data).ToObject<JMessage>();
            
        }
    }
}
