using crude_http_server.Exceptions;
using crude_http_server.HttpRequest.RequestResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        //public string AbsoluteURI { get; set; }
        public RequestPath _RequestPath;
        public string HttpVersion { get; set; }

        public string ValidUriPattern = @"^(?:(?:http|https):\/\/)?([a-zA-Z0-9.:]*)((?!\/{2,})\/[a-zA-Z0-9\/]*)?(\?.*)?$";

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

        public void ParseRequest(string Request)
        {
            if (string.IsNullOrEmpty(Request))
            {
                throw new RequestNotParsedException(RequestNotParsedException.RequestLineEmpty);
            }

            var lines = Request.Split("\r\n");

            string RequestLine = lines[0];

            ParseRequestLine(RequestLine);

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
        }

        /// <summary>
        /// Utility method to parse the request line
        /// E.g. "GET /index HTTP/1.1"
        /// </summary>
        /// <param name="requestLine"></param>
        /// <returns></returns>
        private void ParseRequestLine(string requestLine)
        {
            var components = requestLine.Split(" ");

            if (components.Length != 3)
            {
                throw new RequestNotParsedException(RequestNotParsedException.RequestLineInvalid);
            }

            string RequestMethod = components[0];
            string RequestURI = components[1];
            string RequestVersion = components[2];

            //validate
            if(!Utils.Constants.RequestMethods.Contains(RequestMethod))
            {
                throw new RequestNotParsedException(RequestNotParsedException.RequestMethodInvalid);
            }

            //resolve URI
            if(RequestVersion != Utils.Constants.Protocol)
            {
                throw new RequestNotParsedException(RequestNotParsedException.RequestProtocolInvalid);
            }

            HttpVersion = RequestVersion;
            Method = RequestMethod;

            //Parse the request URI
            if (!ParseRequestURI(RequestURI))
            {
                throw new RequestNotParsedException(RequestNotParsedException.RequestURIInvalid);
            }

        }
        
        #region Parsing Request URI

        /// <summary>
        /// Parses a given request uri and extracts host, absolute path, and query string parameters
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="RequestUrl"></param>
        public bool ParseRequestURI(
            string RequestUrl)
        {
            if (!ValidateRequestUri(RequestUrl))
            {
                return false;
            }


            if (this._RequestPath == null) {
                this._RequestPath = new RequestPath();
            }

            _RequestPath.FullURI = RequestUrl;

            //To do -- implement unit tests for this method
            Match RegexMatch = Regex.Match(RequestUrl, ValidUriPattern);

            string RequestDomain = RegexMatch.Groups[1].ToString();
            this._RequestPath.Host = RequestDomain;

            string AbsolutePath = RegexMatch.Groups[2].ToString();
            if (string.IsNullOrEmpty(AbsolutePath))
            {
                this._RequestPath.Path = new List<string>() { "/" };
            }
            else
            {
                this._RequestPath.Path = AbsolutePath
                .TrimStart('/')
                .Split('/')
                .ToList();
            }

            string QueryString = RegexMatch.Groups[3].ToString();
            if (!string.IsNullOrEmpty(QueryString))
            {
                this._RequestPath.QueryParameters = DecodeQueryParameters(QueryString);
            }

            return true;
        }

        public Dictionary<string, string> DecodeQueryParameters(string RequestURI)
        {
            int QueryIndex = RequestURI.IndexOf('?');

            if (string.IsNullOrEmpty(RequestURI) ||
                QueryIndex == -1 ||
                QueryIndex >= RequestURI.Length - 1)
            {
                return new Dictionary<string, string>();
            }

            string QueryStart = RequestURI.Substring(QueryIndex + 1);

            return QueryStart
                            .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(parts => parts[0],
                                     parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                            .ToDictionary(grouping => grouping.Key,
                                          grouping => string.Join(",", grouping));
        }

        /// <summary>
        /// Validates if a given string is to be accepted as a request uri. http | https schemes are supported (even though this server implementation
        /// is specifically http)
        /// </summary>
        /// <param name="RequestUri"></param>
        /// <returns></returns>
        public bool ValidateRequestUri(string RequestUri)
        {
            if (string.IsNullOrEmpty(RequestUri))
            {
                return false;
            }

            return Regex.Match(RequestUri, ValidUriPattern).Success;
        }

        #endregion

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
