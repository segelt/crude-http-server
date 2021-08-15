using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest
{
    public class RequestPath
    {
        public string FullURI { get; set; }
        public List<string> Path { get; set; }
        public string Host { get; set; }

        private Dictionary<string, string> _QueryParameters;
        public Dictionary<string, string> QueryParameters { 
            get
            {
                if (_QueryParameters == null)
                {
                    _QueryParameters = new Dictionary<string, string>();
                }

                return _QueryParameters;
            }
            set
            {
                _QueryParameters = value;
            }
        }

        public RequestPath()
        {

        }

        public RequestPath(string resource, params string[] args)
        {
            this.Host = resource;

            if(args == null || args.Length == 0)
            {
                this.Path = new List<string>() { "/" };
            }
            else
            {
                this.Path = args.ToList();
            }
        }
    }
}
