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

        public static bool ResolveMethod(RequestManager ReceivedRequest)
        {
            //Todo - seperate the server implementation
            var types = GetTypesWithControllerAttribute(Assembly.GetExecutingAssembly());

            string MainRoute = "";
            string SubPath = "";
            RequestPath InputPath = ReceivedRequest._RequestPath;

            if(InputPath.Path != null && InputPath.Path.Count > 0)
            {
                MainRoute = InputPath.Path[0];

                if(InputPath.Path.Count > 1)
                {
                    SubPath = string.Join("/", InputPath
                        .Path
                        .ToArray()[1..]);
                }
            }

            //Get only the class that has the corresponding route path in its controller attribute
            Type TargetClass = types
                .FirstOrDefault(e => e.GetCustomAttribute<ControllerAttribute>().RoutePath == MainRoute);

            if (TargetClass == null) return false; //Todo - Handle 404

            var TargetMethod = TargetClass.GetMethods()
                .Where(e => e.IsDefined(typeof(RouteAttribute), false))
                .FirstOrDefault(e => e.GetCustomAttribute<RouteAttribute>().RoutePath == SubPath);

            if (TargetMethod == null) return false; //Todo - Handle 404

            //Invoke the controller action
            //1- Create instance of the controller
            var _TargetClassInstance = Activator.CreateInstance(TargetClass);

            //Prepare parameters
            //var parameters = TargetMethod
            //    .GetParameters()
            //    .Select((e, index) => new
            //    {
            //        index = index,
            //        parameterName = e.Name,
            //        parameterType = e.ParameterType
            //    })
            //    .ToArray();
            var parameters = TargetMethod.GetParameters();
            object[] targetParameters = null;
            if(parameters.Length > 0)
            {
                targetParameters = new object[parameters.Length];
                for(int index = 0; index < parameters.Length; index++)
                {
                    ParameterInfo parameter = parameters[index];
                    bool keyExists = ReceivedRequest
                        ._RequestPath
                        .QueryParameters
                        .TryGetValue(parameter.Name, out string ParamValue);

                    if (keyExists)
                    {
                        switch (parameter.ParameterType)
                        {
                            case Type type when type == typeof(char):
                                break;
                            default:
                                break;
                        }
                        //try and parse the value
                        switch (parameter.ParameterType)
                        {
                            case Type type when type == typeof(int):
                                bool parsableInt = Int32.TryParse(ParamValue, out int _ResultInt);
                                if (!parsableInt)
                                {
                                    //return parse exception
                                    return false;
                                    //break;
                                }
                                targetParameters[index] = _ResultInt;
                                break;
                            case Type type when type == typeof(string):
                                targetParameters[index] = ParamValue;
                                break;
                            case Type type when type == typeof(char):
                                bool parsableChar = char.TryParse(ParamValue, out char _ResultChar);
                                if (!parsableChar)
                                {
                                    //return parse exception
                                    return false;
                                    //break;
                                }
                                targetParameters[index] = _ResultChar;
                                break;
                        }
                    }
                    else
                    {
                        if (parameter.HasDefaultValue)
                        {
                            targetParameters[index] = parameter.DefaultValue;
                        }
                        else
                        {
                            //return parameter exception
                            return false;
                        }
                    }
                }
            }

            if(TargetMethod.ReturnType == typeof(void))
            {
                TargetMethod.Invoke(_TargetClassInstance, targetParameters);
                return true;
            }
            else
            {
                var response = TargetMethod.Invoke(_TargetClassInstance, targetParameters);
            }
            //TargetMethod.Invoke();

            return true;
        } 

        #endregion
    }
}
