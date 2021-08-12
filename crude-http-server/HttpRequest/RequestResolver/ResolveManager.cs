using crude_http_server.HttpRequest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest.RequestResolver
{
    public static class ResolveManager
    {
        public static string ValidUriPattern = @"^(?:(?:http|https):\/\/)?([a-zA-Z0-9.:]*)((?!\/{2,})\/[a-zA-Z0-9\/]*)?(\?.*)?$";
        
        /// <summary>
        /// Validates if a given string is to be accepted as a request uri. http | https schemes are supported (even though this server implementation
        /// is specifically http)
        /// </summary>
        /// <param name="RequestUri"></param>
        /// <returns></returns>
        public static bool ValidateRequestUri(string RequestUri)
        {
            if(string.IsNullOrEmpty(RequestUri))
            {
                return false;
            }

            return Regex.Match(RequestUri, ValidUriPattern).Success;
        }

        /// <summary>
        /// Parses a given request uri and extracts host, absolute path, and query string parameters
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="RequestUrl"></param>
        public static RequestPath ParseRequest(
            string RequestUrl)
        {
            if (!ValidateRequestUri(RequestUrl))
            {
                return null;
            }


            RequestPath Request = new RequestPath();

            //To do -- implement unit tests for this method
            Match RegexMatch = Regex.Match(RequestUrl, ValidUriPattern);

            string RequestDomain = RegexMatch.Groups[1].ToString();
            Request.Host = RequestDomain;

            string AbsolutePath = RegexMatch.Groups[2].ToString();
            if (string.IsNullOrEmpty(AbsolutePath))
            {
                Request.Path = new List<string>(){ "/" };
            }
            else
            {
                Request.Path = AbsolutePath
                .TrimStart('/')
                .Split('/')
                .ToList();
            }

            string QueryString = RegexMatch.Groups[3].ToString();
            if (!string.IsNullOrEmpty(QueryString))
            {
                Request.QueryParameters = DecodeQueryParameters(QueryString);
            }

            return Request;
        }

        public static Dictionary<string, string> DecodeQueryParameters(string RequestURI)
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

        #region Resolving server methods

        static IEnumerable<Type> GetTypesWithControllerAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(ControllerAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        public static bool ResolveMethod(string Path, string SubPath, string RequestMethod)
        {
            //Todo - seperate the server implementation
            var types = GetTypesWithControllerAttribute(Assembly.GetExecutingAssembly());

            //Get only the class that has the corresponding route path in its controller attribute
            Type TargetClass = types
                .FirstOrDefault(e => e.GetCustomAttribute<ControllerAttribute>().RoutePath == Path);

            if (TargetClass == null) return false; //Todo - Handle 404

            var TargetMethod = TargetClass.GetMethods()
                .Where(e => e.IsDefined(typeof(RouteAttribute), false))
                .FirstOrDefault(e => e.GetCustomAttribute<RouteAttribute>().RoutePath == SubPath);

            if (TargetMethod == null) return false; //Todo - Handle 404

            //Invoke the controller action
            //TargetMethod.Invoke();

            return true;
        } 

        #endregion
    }
}//Regex.Match(url, @"(http:|https:)\/\/(.*?)\/");
