using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.Exceptions
{
    public abstract class HttpExceptionBase : Exception
    {
        public ResponseCode HttpResponseCode { get; set; }
        public string RequestURI { get; private set; }
        public virtual string Description { get; set; }

        public HttpExceptionBase() : base() { }
        public HttpExceptionBase(string Description, string RequestURI) : base($"{Description} Request URI: {RequestURI}") {
            this.SetRequestURI(RequestURI);
        }
        //public HttpExceptionBase(string message, string requestUri) : base($"{message} Request URI: {requestUri}") { }
        //public HttpExceptionBase(string message, Exception inner) : base(message, inner) { }
        //public HttpExceptionBase(string message, string requestUri, Exception inner) : base($"{message} Request URI: {requestUri}", inner) { }

        public void SetRequestURI(string _RequestURI)
        {
            this.RequestURI = _RequestURI;
        }
    }
}
