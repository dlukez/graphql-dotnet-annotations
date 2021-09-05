using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQL.Annotations
{
    public class MethodResolver : IFieldResolver
    {
        private readonly MethodInfo _methodInfo;
        private readonly object[] _argumentsTemplate;
        private readonly bool _passContext;
        private readonly int _queryParametersStartPosition;
        private readonly IEnumerable<QueryArgumentParameterInfo> _queryParameters;

        public MethodResolver(MethodInfo methodInfo)
            : this (methodInfo, null, null)
        {
        }

        public MethodResolver(MethodInfo methodInfo, object[] dependenciesToInject)
            : this(methodInfo, dependenciesToInject, null)
        {
        }

        public MethodResolver(MethodInfo methodInfo, object[] dependenciesToInject, IEnumerable<QueryArgumentParameterInfo> queryParameters)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            _methodInfo = methodInfo;

            var methodParameters = methodInfo.GetParameters();
            _queryParameters = queryParameters ?? Enumerable.Empty<QueryArgumentParameterInfo>();
            _argumentsTemplate = new object[methodParameters.Length];
            _queryParametersStartPosition = 0;

            // Validate signature:
            //     object SomeMethod([IResolveFieldContext], [dependencies...], [queryParameters...])

            // Return type
            if (methodInfo.ReturnType == typeof(void))
                throw new ArgumentException("Method has no return value");

            // ResolveFieldContext
            if (methodInfo.GetParameters().FirstOrDefault()?.ParameterType == typeof(IResolveFieldContext))
            {
                _passContext = true;
                _queryParametersStartPosition = 1;
            }

            // Dependencies
            if (dependenciesToInject != null)
            {
                dependenciesToInject.CopyTo(_argumentsTemplate, _queryParametersStartPosition);
                _queryParametersStartPosition += dependenciesToInject.Length;
            }

            // Query parameters
            var invalidParameters = _queryParameters.Where(p => p.Position < _queryParametersStartPosition);
            if (invalidParameters.Any())
            {
                var badParameterMessages = string.Join(", ", invalidParameters.Select(p => $"{p.Name} (position {p.Position})"));
                throw new ArgumentException($"Query argument parameters must be specified last in the method definition. Offending parameters: {badParameterMessages}");
            }
        }

        private object ResolveInternal(IResolveFieldContext context)
        {
            // Prepopulated with dependencies
            var arguments = (object[])_argumentsTemplate.Clone();

            // Set IResolveFieldContext
            if (_passContext) arguments[0] = context;

            // Fill in parameters
            foreach (var param in _queryParameters)
                arguments[param.Position] = ConvertArgument(context.GetArgument<object>(param.Name), param.ParameterType);

            return _methodInfo.Invoke(_methodInfo.IsStatic ? null : context.Source, arguments);
        }

        public object Resolve(IResolveFieldContext context)
        {
            return ResolveInternal(context);
        }

        object IFieldResolver.Resolve(IResolveFieldContext context)
        {
            return ResolveInternal(context);
        }

        private static object ConvertArgument(object argument, Type type)
        {
            // object[] => T[]
            var rawArray = argument as object[];
            if (rawArray != null)
            {
                var arrayType = type.GetElementType();
                var typedArray = Array.CreateInstance(arrayType, rawArray.Length);
                Array.Copy(rawArray, typedArray, rawArray.Length);
                return typedArray;
            }

            // Dictionary<string, object> => T
            var dictionary = argument as Dictionary<string, object>;
            if (dictionary != null)
            {
                var instance = Activator.CreateInstance(type);
                foreach (var kvp in dictionary)
                {
                    var property = type.GetProperty(kvp.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    property.SetValue(instance, ConvertArgument(kvp.Value, property.PropertyType));
                }
                return instance;
            }

            return argument;
        }
    }
}
