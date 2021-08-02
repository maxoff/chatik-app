using System;
using System.Net.Sockets;

namespace Chatik.Server
{
    public class ChatSession
    {
        public int Id { get; }
        public TcpClient Client { get; }

        public event Action<int,string> OnMessageReceived;

        public event Action OnSessionClosed;

        public ChatSession(int id, TcpClient client)
        {
            Id = id;
            Client = client;
        }

        public void OnConnected()
        {
            NetworkStream stream = Client.GetStream();
            var bytes = new byte[256];
            string data;

            while (Client.Connected)
            {
                try
                {
                    var c = stream.Read(bytes, 0, bytes.Length);

                    //socket closed
                    if(c == 0)
                    {
                        OnSessionClosed?.Invoke();
                        return;
                    }

                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, c);

                    OnMessageReceived?.Invoke(Id, data);
                 }
                catch (Exception e)
                {
                    if (!Client.Connected)
                    {
                        OnSessionClosed?.Invoke();
                    }
                }
            }
        }

        public void PostMessage(string message)
        {
            if (Client.Connected)
            {
                var stream = Client.GetStream();
                var data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
