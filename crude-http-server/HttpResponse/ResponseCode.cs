using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpResponse
{
    public enum ResponseCode
    {
        [Display(Name= "Continue")]
        Continue = 100,
        
        SwitchingProtocols = 101,

        [Display(Name = "OK")]
        OK = 200,
        [Display(Name = "Created")]
        Created = 201,
        [Display(Name = "Accepted")]
        Accepted = 202,
        [Display(Name = "Non-Authoritative Information")]
        NonAuthoritativeInformation = 203,
        [Display(Name = "No Content")]
        NoContent = 204,
        [Display(Name = "Reset Content")]
        ResetContent = 205,
        [Display(Name = "Partial Content")]
        PartialContent = 206,

        [Display(Name = "Multiple Choices")]
        MultipleChoices = 300,
        [Display(Name = "Moved Permanently")]
        MovedPermanently = 301,
        [Display(Name = "Found")]
        Found = 302,
        [Display(Name = "Redirect")]
        Redirect = 302,
        [Display(Name = "See Other")]
        SeeOther = 303,
        [Display(Name = "Not Modified")]
        NotModified = 304,
        [Display(Name = "Use Proxy")]
        UseProxy = 305,
        [Display(Name = "Unused")]
        Unused = 306,
        [Display(Name = "Temporary Redirect")]
        TemporaryRedirect = 307,

        [Display(Name = "Bad Request")]
        BadRequest = 400,
        [Display(Name = "Unauthorized")]
        Unauthorized = 401,
        [Display(Name = "Payment Required")]
        PaymentRequired = 402,
        [Display(Name = "Forbidden")]
        Forbidden = 403,
        [Display(Name = "Not Found")]
        NotFound = 404,
        [Display(Name = "Method Not Allowed")]
        MethodNotAllowed = 405,
        [Display(Name = "Not Acceptable")]
        NotAcceptable = 406,
        [Display(Name = "Proxy Authentication Required")]
        ProxyAuthenticationRequired = 407,
        [Display(Name = "Request Timeout")]
        RequestTimeout = 408,
        [Display(Name = "Conflict")]
        Conflict = 409,
        [Display(Name = "Gone")]
        Gone = 410,
        [Display(Name = "Length Required")]
        LengthRequired = 411,
        [Display(Name = "Precondition Failed")]
        PreconditionFailed = 412,
        [Display(Name = "Request Entity Too Large")]
        RequestEntityTooLarge = 413,
        [Display(Name = "Request Uri Too Long")]
        RequestUriTooLong = 414,
        [Display(Name = "Unsupported Media Type")]
        UnsupportedMediaType = 415,
        [Display(Name = "Requested Range Not Satisfiable")]
        RequestedRangeNotSatisfiable = 416,
        [Display(Name = "Expectation Failed")]
        ExpectationFailed = 417,

        UpgradeRequired = 426,

        [Display(Name = "Internal Server Error")]
        InternalServerError = 500,
        [Display(Name = "Not Implemented")]
        NotImplemented = 501,
        [Display(Name = "Bad Gateway")]
        BadGateway = 502,
        [Display(Name = "Service Unavailable")]
        ServiceUnavailable = 503,
        [Display(Name = "Gateway Timeout")]
        GatewayTimeout = 504,
        [Display(Name = "Http Version Not Supported")]
        HttpVersionNotSupported = 505,
    }
}
