using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
                var HeaderLineSplitted = headerline.Split(":");
                this.SetHeaderField(
                    HeaderLineSplitted[0], 
                    HeaderLineSplitted[1].TrimStart()); //Trim the SP after the colon
            }

            int RequestBodyIndex = Request.IndexOf("\r\n\r\n");
            Body = Request.Substring(RequestBodyIndex + 4);
            return true;
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

        /// <summary>
        /// Sets the relevant property of HeaderField object, based on the request header field name.
        /// For instance, Content-Type header field on the request is resolved to ContentType property and its value is set to the value parameter.
        /// </summary>
        /// <param name="HeaderName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetHeaderField(string HeaderName, string value)
        {
            PropertyInfo targetProperty = typeof(RequestHeaderFields).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<DescriptionAttribute>().Description == HeaderName);

            if(targetProperty != null)
            {
                targetProperty.SetValue(HeaderField, value);
            }

            return true;
        }
    }
}
