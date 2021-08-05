using crude_http_server.HttpRequest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace crude_http_server.HttpRequest.RequestResolver
{
    public static class ResolveManager
    {
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

        public static string ResolveRequestURI(string RequestURI)
        {
            string AbsolutePath = RequestURI;
            //Absolute Path or Relative Path
            if (RequestURI.IndexOf("://") > 0)
            {
                Uri AbsoluteURI = new Uri(RequestURI);
                AbsolutePath = AbsoluteURI.AbsolutePath;
            }

            return AbsolutePath;
        }

        public static void ParseRequestPath(
            ref RequestPath Request,
            string RequestAbsolutePath)
        {
            int QueryStringIndex = RequestAbsolutePath.IndexOf('?');
            string Directory = RequestAbsolutePath;

            Request.Path = Directory
                .Substring(0, QueryStringIndex) //Get only the parts before '?'
                .TrimStart('/') //Trim trailing '/' (if there is any)
                .Split('/')
                .ToList();

            //If this URI also contains query parameters
            if(QueryStringIndex != -1)
            {
                //Get the query parameters as well
                string QueryParametersOfURI = Directory.Substring(QueryStringIndex);

                Request.QueryParameters = DecodeQueryParameters(QueryParametersOfURI);
            }

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
    }
}
