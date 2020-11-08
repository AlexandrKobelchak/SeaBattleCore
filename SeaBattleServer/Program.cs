using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SeaBattleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = Dns.GetHostAddresses(Environment.MachineName);

            Server srv = new Server(new IPEndPoint(
                        arr[1],//IPAddress.Parse("31.42.190.96"),
                        1024));

            srv.StartListen();

            Console.ReadKey();
        }
    }
}
