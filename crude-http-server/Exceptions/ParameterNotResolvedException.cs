using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.Exceptions
{
    public class ParameterNotResolvedException : HttpExceptionBase
    {
        public ParameterNotResolvedException(string RequestUri) : base("Parameters could not be resolved for the target method.", RequestUri)
        {
            this.HttpResponseCode = ResponseCode.NotFound;
            this.SetRequestURI(RequestURI);
        }
    }
}
