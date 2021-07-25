using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.Utils
{
    public static class Constants
    {
        public static readonly string Protocol = "HTTP/1.1";

        public static string[] RequestMethods = new string[]
        {
            "OPTIONS",
            "GET",
            "HEAD",
            "POST",
            "PUT",
            "DELETE",
            "TRACE",
            "CONNECT"
        };
    }
}
