using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Resolves a Type to its equivalent GraphType.
        /// </summary>
        public static Type ToGraphType(this Type type)
        {
            if (type.IsGraphType())
                return type;

            if (type.IsArray)
                return typeof(ListGraphType<>).MakeGenericType(type.GetElementType().ToGraphType());
                
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return typeof(ListGraphType<>).MakeGenericType(type.GetGenericArguments()[0].ToGraphType());

            var graphType = Nullable.GetUnderlyingType(type)?.GetGraphTypeFromType(true);
            if (graphType != null)
                return graphType;

            graphType = GetGraphTypeFromAttribute(type) ?? type.GetGraphTypeFromType(true);
            return type.GetTypeInfo().IsValueType ? typeof(NonNullGraphType<>).MakeGenericType(graphType) : graphType;
        }

        /// <summary>
        /// Retrieves all the annotated types in the same namespace as the given type.
        /// All types should exist in the same namespace to prevent naming conflicts.
        /// </summary>
        public static IEnumerable<Type> GraphTypesInNamespace(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var assembly = typeInfo.Assembly;
            return assembly.GetTypes()
                .Where(t => t.Namespace.Equals(type.Namespace))
                .Select(GetGraphTypeFromAttribute)
                .ToList();
        }

        private static Type GetGraphTypeFromAttribute(Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<GraphQLTypeAttribute>()?.GraphType.MakeGenericType(type);
        }
    }
}
