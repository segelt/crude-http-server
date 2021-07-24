using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpResponse
{
    public class ResponseManager
    {
        /*
         * RFC-2616 Response (HTTP/1.1)
         * 
         * Response
         * *(message header CRLF)
         * CRLF
         * [ message body ]
         */

        internal StringBuilder _Response;

        public string GetResponse { 
            get
            {
                return _Response.ToString();
            }
        }

        public HttpStatusCode StatusCode { get; set; }

    }
}
