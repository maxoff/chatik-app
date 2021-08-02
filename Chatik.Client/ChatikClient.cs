using System;
using System.Net.Sockets;
using System.Threading;

namespace Chatik.Client
{
    public class ChatikClient : IDisposable
    {
        private readonly string server;
        private readonly int port;
        TcpClient tcp;

        public event Action<string> OnMessageReceived;

        public ChatikClient(string server, int port)
        {
            this.server = server;
            this.port = port;
        }

        public void Connect()
        {
            try
            {
                tcp = new TcpClient(server, port);
                new Thread(() => MessageListener()).Start();
            }
            catch (SocketException e)
            {
                throw new ApplicationException("Network error");
            }
        }

        public void Publish(string message)
        {
            var stream = tcp.GetStream();
            var data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Disconnect()
        {

        }

        public void MessageListener()
        {
            var stream = tcp.GetStream();
            var data = new byte[256];

            while (tcp.Connected)
            {
                try
                {
                    var bytes = stream.Read(data, 0, data.Length);
                    var message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    OnMessageReceived?.Invoke(message);
                }
                catch
                {
                    if (!tcp.Connected)
                    {
                        break;
                    }
                }
            }

            stream.Close();
        }

        public void Dispose()
        {
            if(tcp != null)
            {
                tcp.Close();
                tcp.Dispose();
            }
        }
    }
}
