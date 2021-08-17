using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest
{
    public class RequestHeaderFields
    {
        [Description("Accept")]
        public string Accept { get; set; }

        [Description("Accept-Charset")]
        public string AcceptCharset { get; set; }

        [Description("Accept-Encoding")]
        public string AcceptEncoding { get; set; }

        [Description("Accept-Language")]
        public string AcceptLanguage { get; set; }

        [Description("Authorization")]
        public string Authorization { get; set; }

        [Description("Connection")]
        public string Connection { get; set; }

        [Description("Content-Type")]
        public string ContentType { get; set; }

        [Description("From")]
        public string From { get; set; }

        [Description("Host")]
        public string Host { get; set; }

        [Description("Range")]
        public string Range { get; set; }

        [Description("User-Agent")]
        public string UserAgent { get; set; }
    }
}
