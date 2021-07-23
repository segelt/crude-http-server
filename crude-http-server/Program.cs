using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace crude_http_server
{
    public class Program
    {
        const int PORT_NO = 11000;
        const string SERVER_IP = "127.0.0.1";
        private static bool listen = true;

        public static void Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            listener.Start();

            while (listen)
            {
                var client = listener.AcceptTcpClient();
                Console.WriteLine("New connection established.");

                var childSocketThread = new Thread(() =>
                {
                    using NetworkStream networkStream = client.GetStream();

                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received : {message}\nSize: {bytesRead} bytes.");

                    Thread.Sleep(5000);

                    Console.WriteLine("Sending back : " + message);
                    networkStream.Write(buffer, 0, bytesRead);
                    client.Close();
                });

                childSocketThread.Start();
            }

            listener.Stop();

            return;
        }
    }
}
