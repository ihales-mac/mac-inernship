using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Message
    {
        public string Header { get; set; }
        public string Body { get; set; }
        private string LINE_SEPARATOR = "\n";

        public void WriteTo(NetworkStream stream)
        {
            string msg = Header + LINE_SEPARATOR + Body + LINE_SEPARATOR;
            byte[] data = Encoding.ASCII.GetBytes(msg);
            int count = Encoding.ASCII.GetByteCount(msg);
            stream.Write(data, 0, count);
        }

        public void ReadFrom(NetworkStream stream)
        {
            byte[] bytes = new byte[1024];
            string msg = null;
            int count = stream.Read(bytes, 0, 1024);


            msg = Encoding.ASCII.GetString(bytes,0,count);

            this.Header = msg.Split('\n')[0];
            this.Body = msg.Split('\n')[1];
        }


    }
}
