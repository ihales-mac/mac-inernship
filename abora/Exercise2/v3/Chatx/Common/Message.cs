using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Common
{
    public class Message
    {
        public string Header { get; set; }
        public string Body { get; set; }
        private string LINE_SEPARATOR = ";";

        public void WriteTo(NetworkStream stream)
        {
            string msg = Header + LINE_SEPARATOR + Body + LINE_SEPARATOR;

            byte[] data = Encoding.ASCII.GetBytes(msg);
            //encrypt data using public key

            int count = Encoding.ASCII.GetByteCount(msg);
            stream.Write(data, 0, count);
        }


        public void WriteTo(NetworkStream stream, EncryptionRSA rsa)
        {
            string msg = Header + LINE_SEPARATOR + Body + LINE_SEPARATOR;

            byte[] data = rsa.Encrypt(Encoding.ASCII.GetBytes(msg));
            //encrypt data using public key
            stream.Write(data, 0, data.Length);
        }



        public void ReadFrom(NetworkStream stream)
        {
            byte[] bytes = new byte[10024];
            string msg = null;
            int count = stream.Read(bytes, 0, 10024);

            msg = Encoding.ASCII.GetString(bytes, 0, count);

            this.Header = msg.Split(';')[0];
            this.Body = msg.Split(';')[1];
        }

        public void ReadFrom(NetworkStream stream, EncryptionRSA rsa)
        {
            byte[] bytes = new byte[10024];
            int count = stream.Read(bytes, 0, 10024);

            byte[] descripted = new byte[count];
            for (int i = 0; i < count; i++)
            {
                descripted[i] = bytes[i];
            }

            string msg = rsa.Decrypt(descripted);

            this.Header = msg.Split(';')[0];
            this.Body = msg.Split(';')[1];
        }
    }
}
