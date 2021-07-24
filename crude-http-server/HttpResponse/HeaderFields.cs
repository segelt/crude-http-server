using crude_http_server.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpResponse
{
    public class HeaderFields
    {
        [Display(Name ="Date")]
        public string Date { get

            {
                return DateTime.Now.ToUniversalTime().ToString("r"); //Conform to rfc 1123 format
            }
        }

        [Display(Name ="Server")]
        public string Server { get; set; } = "crudehttpserver/1.0.0"; //todo - obtain version using reflection

        [Display(Name = "Content-Language")]
        public string ContentLanguage { get; set; } = "en";

        #region Content types
        //full list obtained via https://stackoverflow.com/questions/23714383/what-are-all-the-possible-values-for-http-content-type-header

        public enum ContentTypes
        {
            [Display(Name= "application")]
            application,
            [Display(Name = "audio")]
            audio,
            [Display(Name = "image")]
            image,
            [Display(Name = "text")]
            text,
            [Display(Name = "video")]
            video
        }

        public enum ApplicationTypes
        {
            [Display(Name = "octet-stream")]
            octetstream,
            
            [Display(Name = "ogg")]
            ogg,
            
            [Display(Name = "pdf")]
            pdf,

            [Display(Name = "xhtml+xml")]
            xhtmlxml,

            [Display(Name = "json")]
            json,

            [Display(Name = "xml")]
            xml,

            [Display(Name = "zip")]
            zip,
            
            [Display(Name = "x-www-form-urlencoded")]
            urlencoded,
        }

        public enum AudioTypes
        {
            [Display(Name = "mpeg")]
            mpeg,

            [Display(Name = "x-wav")]
            wav,
        }

        public enum ImageTypes
        {
            [Display(Name = "gif")]
            gif,

            [Display(Name = "jpeg")]
            jpeg,

            [Display(Name = "png")]
            png,

            [Display(Name = "tiff")]
            tiff,

            [Display(Name = "svg+xml")]
            svg,
        }

        public enum TextTypes
        {
            [Display(Name = "css")]
            css,

            [Display(Name = "csv")]
            csv,

            [Display(Name = "html")]
            html,

            [Display(Name = "javascript")]
            javascript,

            [Display(Name = "plain")]
            plain,
            
            [Display(Name = "xml")]
            xml,
        }

        public enum VideoTypes
        {
            [Display(Name = "mpeg")]
            mpeg,

            [Display(Name = "mp4")]
            mp4,

            [Display(Name = "quicktime")]
            quicktime,

            [Display(Name = "webm")]
            webm
        }

        //Auto-initialize to text/plain
        public string ResponseContentType { get; set; } = ContentTypes.text.GetAttribute<DisplayNameAttribute>().DisplayName;
        public string ResponseSubType { get; set; } = TextTypes.plain.GetAttribute<DisplayNameAttribute>().DisplayName;

        [Display(Name = "Content-Type")]
        public string ContentType
        {
            get
            {
                return $"{ResponseContentType}/{ResponseSubType}";
            }
        }
        #endregion
    
        [Display(Name = "Content-Length")]
        public int ContentLength { get; set; }

        public enum ConnectionType
        {
            [Display(Name = "keep-alive")]
            keepalive,

            [Display(Name = "close")]
            close,
        }

        public ConnectionType ResponseConnectionType { get; set; } = ConnectionType.close; //default close

        [Display(Name = "Connection")]
        private string ResponseConnectionTypeStr { 
            get
            {
                return ResponseConnectionType.GetAttribute<DisplayAttribute>().Name;
            }
        }

        public HeaderFields()
        {

        }

        public string GenerateHeaderFields()
        {
            StringBuilder _HeaderFields = new StringBuilder("");

            _HeaderFields.Append($"{GetPropertyDisplayName("Date")} : {Date}\r\n");
            _HeaderFields.Append($"{GetPropertyDisplayName("Server")} : {Server}\r\n");
            _HeaderFields.Append($"{GetPropertyDisplayName("ContentLanguage")} : {ContentLanguage}\r\n");
            _HeaderFields.Append($"{GetPropertyDisplayName("ContentType")} : {ContentType}\r\n");
            _HeaderFields.Append($"{GetPropertyDisplayName("ResponseConnectionTypeStr")} : {ResponseConnectionTypeStr}\r\n");

            return _HeaderFields.ToString();
        }

        private string GetPropertyDisplayName(string propertyName)
        {
            MemberInfo property = typeof(HeaderFields).GetProperty(propertyName);
            return property.GetCustomAttribute<DisplayAttribute>()?.Name;
        }
    }
}
