using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.Exceptions
{
    public class MethodNotResolvedException : HttpExceptionBase
    {
        public MethodNotResolvedException(string RequestURI) : base("The given method was not found.", RequestURI)
        {
            this.HttpResponseCode = ResponseCode.NotFound;
            this.SetRequestURI(RequestURI);
        }
    }
}
