using System;

namespace Chatik.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ChatikServer(12345);
            
            try
            {
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
