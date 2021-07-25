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
