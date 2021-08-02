using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chatik.Server
{
    public class ChatikServer
    {
        private readonly int port;
        private TcpListener server;

        private Dictionary<int, ChatSession> sessions = new Dictionary<int, ChatSession>();

        public ChatikServer(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            try
            {
                var localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();

                Console.WriteLine("Server started at {0}:{1}", localAddr, port);

                var id = 1;

                //accept multiple clients
                while (true)
                {
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected {0}", client.Client.RemoteEndPoint);

                    var session = new ChatSession(id, client);

                    session.OnMessageReceived += onMessage;

                    session.OnSessionClosed += () =>
                    {
                        Console.WriteLine("Disconnected {0}", client.Client.RemoteEndPoint);
                        sessions.Remove(session.Id);
                    };

                    sessions.Add(id++, session);

                    new Thread(() => session.OnConnected()).Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Stop()
        {
            if(server != null)
            {
                server.Stop();
            }
        }

        void onMessage(int sender, string message)
        {
            broadcastMessage(sender, message);
        }

        void broadcastMessage(int sender, string message)
        {
            foreach (var session in sessions)
            {
                if(session.Value.Id != sender)
                    session.Value.PostMessage($"{sender}: {message}");
            }
        }
    }
}
