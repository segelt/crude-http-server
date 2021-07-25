using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest
{
    public class RequestManager
    {
        /*
         * request-line CRLF
         * headers CRLF
         * CRLF
         * message-body
         */

        #region Request Line

        public string Method { get; set; }
        public string AbsoluteURI { get; set; }
        public string HttpVersion { get; set; }

        #endregion

        #region Header Fields
        private RequestHeaderFields _HeaderField;
        public RequestHeaderFields HeaderField { 
            get
            {
                if (_HeaderField == null)
                {
                    _HeaderField = new RequestHeaderFields();
                }
                return _HeaderField;
            }
            set
            {
                _HeaderField = value;
            }
        }
        #endregion

        public string Body { get; set; }

        public bool ParseRequest(string Request)
        {
            if (string.IsNullOrEmpty(Request))
            {
                return false;
            }

            var lines = Request.Split("\r\n");

            string RequestLine = lines[0];
            
            if (!ParseRequestLine(RequestLine))
            {
                return false;
            }

            foreach(var headerline in lines.Skip(1))
            {
                if (String.IsNullOrWhiteSpace(headerline)) break;

                //Resolve headerline
                var splitted = headerline.Split(":");
            }

            return false;
        }

        public bool ParseRequestLine(string requestLine)
        {
            var components = requestLine.Split(" ");

            if (components.Length != 3) return false;

            string RequestMethod = components[0];
            string RequestURI = components[1];
            string RequestVersion = components[2];

            //validate
            if(!Utils.Constants.RequestMethods.Contains(RequestMethod))
            {
                return false;
            }

            //resolve URI
            if(RequestVersion != Utils.Constants.Protocol)
            {
                return false;
            }

            HttpVersion = RequestVersion;
            Method = RequestMethod;
            AbsoluteURI = RequestURI;
            return true;
        }
    }
}
