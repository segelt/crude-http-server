using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class ControllerAttribute : Attribute
    {
        public string RoutePath { get; set; }
    }
}
