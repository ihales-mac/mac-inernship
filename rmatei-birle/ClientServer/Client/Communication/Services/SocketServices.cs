using Client.Communication.Contracts;
using Client.EventArguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Communication.Services
{
    public class SocketServices : ICommunication
    {
        private static SocketServices _instance;

        private SocketServices() { }

        public static ICommunication Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SocketServices();
                }
                return _instance;
            }
        }

        public event IncomingMessageEventHandler IncomingMessage;

        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private Thread _listeningThread;
        private string _identificationCode;

        private bool _isConnected = false;
        private bool _isListening = false;

        public string Connect(string ip, string port)
        {
            int portInt;

            if (!Int32.TryParse(port, out portInt))
            {
                return "Error: Invalid port";
            }

            try
            {
                IPAddress.Parse(ip);
            }
            catch (FormatException)
            {
                return "Error: Invalid IP format";
            }


            if (!_isConnected)
            {
                _client = new TcpClient();
                _client.Connect(ip, portInt);
                _reader = new StreamReader(_client.GetStream(), Encoding.ASCII);
                _writer = new StreamWriter(_client.GetStream(), Encoding.ASCII);
                _writer.AutoFlush = true;
                _isConnected = true;

                //get and send keys;
            }

            return "connected";
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                _reader.Close();
                _reader.Dispose();
                _writer.Close();
                _writer.Dispose();
                _client.Close();
                _client.Dispose();
                _isConnected = false;
            }
        }

        public void ListenContinuously()
        {
            if (!_isListening)
            {
                _isListening = true;
                //do thread here
                _listeningThread = new Thread(() =>
                {
                    while (_isListening && _isConnected)
                    {
                        string fromServer =_reader.ReadLine();
                        //to event stuff here;
                        if (fromServer != null)
                        {
                            //raise event
                            OnIncomingMessage(new IncomingMessageEventArgs(fromServer));
                        }
                    }
                });

                _listeningThread.SetApartmentState(ApartmentState.STA);
                _listeningThread.Start();
            }
        }

        protected void OnIncomingMessage(IncomingMessageEventArgs e)
        {
            new Thread(() =>
                {
                    IncomingMessage?.Invoke(this, e);
                }
            ).Start();
        }

        public string ListenOnce()
        {
            bool wasListening = false;
            if (_isListening)
            {
                this.StopListeningContinuously();
                wasListening = true;
            }

            string read = _reader.ReadLine();

            while (read == null)
            {
                read = _reader.ReadLine();
            }


            if (wasListening)
            {
                ListenContinuously();
            }

            return read;
        }

        public void StopListeningContinuously()
        {
            if (_isListening)
            {
                _isListening = false;
                if (_listeningThread != null)
                {
                    _listeningThread.Abort();
                    _listeningThread = null;
                }
            }
        }

        public void SendMessage(string message)
        {
            //encrypt message
            //add identification code
            //bool wasListening = false;
            //if (_isListening)
            //{
            //    this.StopListeningContinuously();
            //    wasListening = true;
            //}

            if (this._identificationCode != null)
            {
                _writer.WriteLine("$$IC=" + _identificationCode + "" + message);
            }
            else
            {
                _writer.WriteLine(message);
            }

            //if (wasListening)
            //{
            //    ListenContinuously();
            //}
        }

        public void SetIc(string ic)
        {
            this._identificationCode = ic;
        }
    }
}
