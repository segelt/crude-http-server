using crude_http_server.HttpRequest.Attributes;
using crude_http_server.HttpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static crude_http_server.HttpResponse.ResponseHeaderFields;

namespace crude_http_server.Controllers
{
    [Controller(RoutePath = "implementation")]
    public class ImplementationController
    {
        [Route(RequestMethod = "GET", RoutePath = "getdayofweek")]
        public ResponseManager<int> GetDayOfWeek()
        {
            ResponseManager<int> Response = new ResponseManager<int>();

            Response.StatusCode = ResponseCode.Accepted;
            Response.HeaderField.ResponseType = ContentTypes.text;
            Response.HeaderField.TextType = TextTypes.plain;
            Response.Body = (int)DateTime.Now.DayOfWeek;

            return Response;
        }

        [Route(RequestMethod = "GET", RoutePath = "PrintInput")]
        public ResponseManager<string> PrintInput(string Input)
        {
            string Result =  $"Hello from controller. I have received {Input} as input!";

            ResponseManager<string> Response = new ResponseManager<string>();

            Response.StatusCode = ResponseCode.Accepted;
            Response.HeaderField.ResponseType = ContentTypes.text;
            Response.HeaderField.TextType = TextTypes.plain;
            Response.Body = Result;

            return Response;
        }

        [Route(RequestMethod = "GET", RoutePath = "MultiplyAndPrint")]
        public ResponseManager<string> MultiplyAndPrint(int param1, int param2, string InterpolateParam)
        {
            string Result =  $"Multiplication of {param1} and {param2} is {param1 * param2}. This is a test: {InterpolateParam}!";

            ResponseManager<string> Response = new ResponseManager<string>();

            Response.StatusCode = ResponseCode.Accepted;
            Response.HeaderField.ResponseType = ContentTypes.text;
            Response.HeaderField.TextType = TextTypes.plain;
            Response.Body = Result;

            return Response;
        }
    }
}
