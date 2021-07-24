using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static crude_http_server.HttpResponse.HeaderFields;

namespace crude_http_server
{
    public class Program
    {
        const int PORT_NO = 11000;
        const string SERVER_IP = "127.0.0.1";
        private static bool listen = true;

        private static async Task<bool> HandleClient(TcpClient client)
        {
            //await Task.Yield();
            using NetworkStream networkStream = client.GetStream();

            byte[] responseBuffer = new byte[client.ReceiveBufferSize];

            //Generate Response
            ResponseManager _ResponseManager = new ResponseManager();
            _ResponseManager.StatusCode = ResponseCode.Accepted;
            _ResponseManager.HeaderField.ResponseType = ContentTypes.text;
            _ResponseManager.HeaderField.TextType = TextTypes.plain;
            //_ResponseManager.HeaderField.ContentLength = 0;
            _ResponseManager.Body = "Test client";

            string returnMessage = _ResponseManager.Response;
            byte[] responseBytes = Encoding.UTF8.GetBytes(returnMessage);

            networkStream.Write(responseBytes, 0, responseBytes.Length);
            return true;
        }

        public static async Task Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            listener.Start();

            while (listen)
            {
                Task.Run(async () => await HandleClient(await listener.AcceptTcpClientAsync()));
            }

            listener.Stop();

            return;
        }
    }
}