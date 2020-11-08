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

            TcpListener listener =
                new TcpListener(
                    new IPEndPoint(
                        arr[1],//IPAddress.Parse("31.42.190.96"),
                        1024));
            listener.Start(10);
            listener.BeginAcceptTcpClient(MyCallback, listener);
            Console.ReadKey();
        }

        private static void MyCallback(IAsyncResult ar)
        {
            TcpListener listener = ar.AsyncState as TcpListener;
            TcpClient client = listener.EndAcceptTcpClient(ar);
            listener.BeginAcceptTcpClient(MyCallback, listener);

            try
            {
                StreamReader reader =
                    new StreamReader(client.GetStream(), Encoding.ASCII);
                StreamWriter writer =
                    new StreamWriter(client.GetStream(), Encoding.ASCII) { AutoFlush = true };

                string command = String.Empty;

                IPAddress remoteAddr = (client.Client.RemoteEndPoint as IPEndPoint).Address;

                string login = null;

                Console.WriteLine($"Connect from {remoteAddr}");
                //writer.WriteLine($"Hello my friend from ip {remoteAddr}");
                do
                {
                    //writer.Write("Enter command => ");
                    command = reader.ReadLine().ToLower();
                    switch (command)
                    {   
                        case "auth":
                            login = Authentication(reader, writer);
                            if(login != null)
                            {
                                writer.Write($"Hello {login}! Enter command => ");
                            }
                            break;

                        case "to":
                        case "list":
                        case "count":
                        case "get":

                        case "quit":
                            if(login!=null)
                                writer.WriteLine("GOOD BY");
                            break;
                    }
                } while (command.ToLower().CompareTo("quit") != 0);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }
            client.Close();

        }
        private static string Authentication(StreamReader reader, StreamWriter writer)
        {
            //writer.Write("Enter Login => ");
            string login = reader.ReadLine();
            //writer.Write("Password => ");
            string pwd = reader.ReadLine();
            if (Auth.Check(login, pwd) != null)
            {
                //writer.WriteLine($"Hello {login}");                
                return login;
            }
            return null;
        }
    }
}
