using crude_http_server.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpResponse
{
    public class ResponseManager
    {
        /*
         * RFC-2616 Response (HTTP/1.1)
         * 
         * Response
         * *(message header CRLF)
         * CRLF
         * [ message body ]
         */

        //internal StringBuilder _Response;

        public string Body { get; set; }

        public string Response { 
            get
            {
                return this.GenerateResponse();
            }
        }

        private ResponseHeaderFields _HeaderField;
        public ResponseHeaderFields HeaderField { 
            get
            {
                if(_HeaderField == null)
                {
                    _HeaderField = new ResponseHeaderFields();
                }
                return _HeaderField;
            }
        }

        public ResponseCode StatusCode { get; set; }

        protected string GenerateResponse()
        {
            StringBuilder _Response = new StringBuilder("");
            string StatusLine = $"{Constants.Protocol} {(int)StatusCode} {StatusCode.GetAttribute<DisplayAttribute>().Name}\r\n";
            _Response.Append(StatusLine);
            HeaderField.ContentLength = string.IsNullOrEmpty(Body) ? 0 : Body.Length;

            _Response.Append(HeaderField.GenerateHeaderFields());

            if (!string.IsNullOrEmpty(Body))
            {
                _Response.Append($"{Body}");
            }

            return _Response.ToString();
        }
    }
}
