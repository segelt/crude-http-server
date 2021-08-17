using crude_http_server.Exceptions;
using crude_http_server.HttpRequest.Attributes;
using crude_http_server.HttpResponse;
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

        /// <summary>
        /// Returns the actual resolved method
        /// </summary>
        /// <param name="ReceivedRequest"></param>
        /// <returns></returns>
        public static ResponseManager ResolveMethod(RequestManager ReceivedRequest)
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

            if (TargetClass == null) throw new MethodNotResolvedException(ReceivedRequest._RequestPath.FullURI); //Todo - Handle 404

            var TargetMethod = TargetClass.GetMethods()
                .Where(e => e.IsDefined(typeof(RouteAttribute), false))
                .FirstOrDefault(e => e.GetCustomAttribute<RouteAttribute>().RoutePath == SubPath);

            if (TargetMethod == null) throw new MethodNotResolvedException(ReceivedRequest._RequestPath.FullURI); //Todo - Handle 404

            //Invoke the controller action
            //1- Create instance of the controller
            var _TargetClassInstance = Activator.CreateInstance(TargetClass);

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
                        //try and parse the value
                        switch (parameter.ParameterType)
                        {
                            case Type type when type == typeof(int):
                                bool parsableInt = Int32.TryParse(ParamValue, out int _ResultInt);
                                if (!parsableInt)
                                {
                                    //return parse exception
                                    throw new ParameterNotResolvedException(ReceivedRequest._RequestPath.FullURI);
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
                                    throw new ParameterNotResolvedException(ReceivedRequest._RequestPath.FullURI);
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
                            throw new ParameterNotResolvedException(ReceivedRequest._RequestPath.FullURI);
                        }
                    }
                }
            }

            Type ReturnedType = TargetMethod.ReturnType;
            if(ReturnedType == typeof(ResponseManager) || ReturnedType.IsSubclassOf(typeof(ResponseManager)))
            {
                var response = TargetMethod.Invoke(_TargetClassInstance, targetParameters);

                return (ResponseManager)response; //Works with generic type as well.

                ////Check if it is generic
                //if (ReturnedType.IsGenericType)
                //{
                //    //Type GenericType = ReturnedType.GetGenericArguments()[0];
                //    return (ResponseManager)response;
                //}
                //else
                //{
                //    return (ResponseManager)response;
                //}
            }
            else
            {
                throw new Exception();
            }
        } 
        
        public static ResponseManager ResolveRequest(string Request)
        {
            RequestManager _Request = new RequestManager();

            try
            {
                _Request.ParseRequest(Request);

                return ResolveMethod(_Request);
            }
            catch(HttpExceptionBase ex)
            {
                // return error page
                return null;
            }
        }
        #endregion
    }
}
