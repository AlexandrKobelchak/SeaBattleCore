using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SeaBattleServer
{
    class Server
    {
        TcpListener _listener;
        public Server(IPEndPoint endPoint)
        {
            _listener = new TcpListener(endPoint);
        }
        public void StartListen()
        {
            try
            {
                _listener.Start(10);
                _listener.BeginAcceptTcpClient(MyCallback, _listener);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }
        }

        private void MyCallback(IAsyncResult ar)
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
                            if (login != null)
                            {
                                writer.Write($"Hello {login}! Enter command => ");
                            }
                            break;

                        case "to":
                        case "list":
                        case "count":
                        case "get":

                        case "quit":
                            if (login != null)
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

        private string Authentication(StreamReader reader, StreamWriter writer)
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
