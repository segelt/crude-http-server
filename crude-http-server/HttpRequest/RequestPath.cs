using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest
{
    public class RequestPath
    {
        public string Path { get; set; }
        public string Resource { get; set; }

        public RequestPath()
        {

        }

        public RequestPath(string path, string resource)
        {
            this.Path = path;
            this.Resource = resource;
        }
    }
}
