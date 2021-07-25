using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static crude_http_server.HttpResponse.ResponseHeaderFields;

namespace crude_http_server
{
    public class Program
    {
        const int PORT_NO = 11000;
        const string SERVER_IP = "127.0.0.1";
        private static bool listen = true;

        private static void HandleClient(TcpClient client)
        {
            using NetworkStream networkStream = client.GetStream();

            byte[] responseBuffer = new byte[client.ReceiveBufferSize];
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);

            //Generate Response
            ResponseManager _ResponseManager = new ResponseManager();
            _ResponseManager.StatusCode = ResponseCode.Accepted;
            _ResponseManager.HeaderField.ResponseType = ContentTypes.text;
            _ResponseManager.HeaderField.TextType = TextTypes.plain;
            _ResponseManager.Body = "Test client";

            string returnMessage = _ResponseManager.Response;
            byte[] responseBytes = Encoding.UTF8.GetBytes(returnMessage);

            Thread.Sleep(5000);
            networkStream.Write(responseBytes, 0, responseBytes.Length);
            networkStream.Close();
            client.Close();
            return;
        }

        public static void Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            listener.Start();

            while (listen)
            {
                TcpClient client = listener.AcceptTcpClient();

                //For each client, start a new background thread
                new Thread(() =>
                {
                    HandleClient(client);
                }).Start();
            }

            listener.Stop();

            return;
        }
    }
}