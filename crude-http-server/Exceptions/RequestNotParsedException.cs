using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.Exceptions
{
    public class RequestNotParsedException : HttpExceptionBase
    {
        public static string RequestLineInvalid = "Request line was invalid.";
        public static string RequestURIInvalid = "Request URI was invalid.";
        public static string RequestMethodInvalid = "Supplied request method was invalid.";
        public static string RequestProtocolInvalid = "Invalid request protocol.";
        public static string RequestLineEmpty = "No request has been given.";

        public RequestNotParsedException(string Message) : base(Message)
        {
            this.HttpResponseCode = ResponseCode.NotFound;
        }
    }
}
