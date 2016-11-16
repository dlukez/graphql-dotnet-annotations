using System;
using System.Collections.Generic;
using System.Reflection;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQL.Annotations
{
    public class MethodResolver : IFieldResolver
    {
        private readonly MethodInfo _methodInfo;
        private readonly object[] _injectedParameters;
        private readonly Dictionary<ParameterInfo, QueryArgument> _argumentMap;

        public MethodResolver(MethodInfo methodInfo, object[] injectedParameters, Dictionary<ParameterInfo, QueryArgument> argumentMap)
        {
            _methodInfo = methodInfo;
            _injectedParameters = injectedParameters;
            _argumentMap = argumentMap;
        }

        private object ResolveInternal(ResolveFieldContext context)
        {
            var arguments = new object[_injectedParameters.Length + _argumentMap.Count + 1];

            // Context is always first argument
            arguments[0] = context;

            // then injected parameters
            _injectedParameters.CopyTo(arguments, 1);

            // then query arguments.
            foreach (var param in _argumentMap)
            {
                arguments[param.Key.Position] =
                    ConvertArgument(
                        context.GetArgument<object>(param.Value.Name),
                        param.Key.ParameterType);
            }

            return _methodInfo.Invoke(_methodInfo.IsStatic ? null : context.Source, arguments);
        }

        public object Resolve(ResolveFieldContext context)
        {
            return ResolveInternal(context);
        }

        object IFieldResolver.Resolve(ResolveFieldContext context)
        {
            return ResolveInternal(context);
        }

        private static object ConvertArgument(object argument, Type type)
        {
            // object[] => T[]
            //var rawArray = argument as object[];
            //if (rawArray != null)
            //{
            //    var arrayType = type.GetElementType();
            //    var typedArray = Array.CreateInstance(arrayType, rawArray.Length);
            //    Array.Copy(rawArray, typedArray, rawArray.Length);
            //    return typedArray;
            //}

            // Dictionary<string, object> => T
            var dictionary = argument as Dictionary<string, object>;
            if (dictionary != null)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var kvp in dictionary)
                {
                    var property = type.GetProperty(kvp.Key,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    property.SetValue(instance, ConvertArgument(kvp.Value, property.PropertyType));
                }
                return instance;
            }

            return argument;
        }
    }
}
