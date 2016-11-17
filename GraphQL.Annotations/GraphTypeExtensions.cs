using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations
{
    public static class GraphTypeExtensions
    {
        public static void ApplyTypeData<TModelType>(this GraphType instance)
        {
            var type = typeof (TModelType);
            var metadata = type.GetTypeInfo().GetCustomAttribute<GraphQLTypeAttribute>();

            if (metadata == null)
            {
                var message =
                    $"{type.Name} is not marked as a GraphQL type - did you forget to mark it with a GraphQLTypeAttribute?";
                throw new NotSupportedException(message);
            }

            instance.Name = !string.IsNullOrWhiteSpace(metadata.Name) ? metadata.Name : type.Name;
            instance.Description = metadata.Description;
        }

        public static void ApplyProperties<TModelType>(this IComplexGraphType instance)
        {
            var type = typeof (TModelType);
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance))
            {
                var fieldAttr = prop.GetCustomAttribute<GraphQLFieldAttribute>();
                if (fieldAttr == null)
                    continue;

                try
                {
                    instance.AddField(new FieldType
                    {
                        Name = fieldAttr.Name ?? prop.Name.FirstCharacterToLower(),
                        Type = fieldAttr.ReturnType ?? prop.PropertyType.ToGraphType(),
                        Resolver = new PropertyResolver(prop),
                        Description = fieldAttr.Description
                    });
                }
                catch (ArgumentOutOfRangeException exception)
                {
                    throw new NotSupportedException(
                        $"Unable to register field for property {prop.Name}",
                        exception);
                }
            }
        }
        
        public static void ApplyMethods<TModelType>(this IComplexGraphType instance, object[] injectedParameters, bool shouldResolve)
        {
            var type = typeof (TModelType);
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).Where(m => !m.IsSpecialName))
            {
                var funcAttr = method.GetCustomAttribute<GraphQLFuncAttribute>();
                if (funcAttr == null)
                    continue;

                var methodDescription = $"`{method.Name}` on type '{type.Name}'";
                var methodParams = method.GetParameters();
                var parameterArgumentMappings = new Dictionary<ParameterInfo, QueryArgument>();

                try
                {
                    // Ensure query parameters are annotated
                    foreach (var param in methodParams.Skip(injectedParameters.Length + 1))
                    {
                        var paramAttr = param.GetCustomAttribute<GraphQLArgumentAttribute>();
                        if (paramAttr == null)
                            throw new ArgumentException(
                                $"Parameter `{param.Name}` in method {methodDescription} is missing a required GraphQLArgumentAttribute");

                        parameterArgumentMappings.Add(param, new QueryArgument(param.ParameterType.ToGraphType(param.HasDefaultValue))
                        {
                            Name = paramAttr.Name ?? param.Name,
                            Description = paramAttr.Description,
                            DefaultValue = param.HasDefaultValue ? param.DefaultValue : null
                        });
                    }

                    // Void methods aren't allowed
                    if (method.ReturnType == typeof(void))
                        throw new NotSupportedException(
                            $"Invalid return type `void` for {methodDescription} - GraphQL methods must return values as they are used as getters.");

                    // Create the field
                    instance.AddField(new FieldType
                    {
                        Type = funcAttr.ReturnType ?? method.ReturnType.ToGraphType(),
                        Name = funcAttr.Name ?? method.Name.FirstCharacterToLower(),
                        Description = funcAttr.Description,
                        Arguments = new QueryArguments(parameterArgumentMappings.Values),
                        Resolver = shouldResolve
                            ? new MethodResolver(method, injectedParameters, parameterArgumentMappings)
                            : null
                    });
                }
                catch (ArgumentOutOfRangeException exception)
                {
                    throw new NotSupportedException(
                        $"Unable to register field for {methodDescription} - unsupported type in method signature",
                        exception);
                }
            }
        }

        private static string FirstCharacterToLower(this string s)
        {
            return s.Substring(0, 1).ToLower() + s.Substring(1);
        }
    }
}
