using System;

namespace Chatik.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new ChatikClient("127.0.0.1", 12345))
            {
                try
                {
                    client.Connect();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                client.OnMessageReceived += m => Console.WriteLine(m);

                while (true)
                {
                    var message = Console.ReadLine();
                    if (message.Length == 0)
                    {
                        client.Disconnect();
                        return;
                    }

                    client.Publish(message);
                }
            }
        }
    }
}
