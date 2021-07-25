using crude_http_server.HttpRequest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.Controllers
{
    [Controller(RoutePath = "implementation")]
    public class ImplementationController
    {
        [Route(RequestMethod = "GET", RoutePath = "getdayofweek")]
        public int GetDayOfWeek()
        {
            return (int)DateTime.Now.DayOfWeek;
        }
    }
}
