using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Already a graph type
            if (type.IsGraphType())
                return type;

            // Collection types
            var enumerableType = type.IsArray ? type.GetElementType() : type.GetEnumerableType();
            if (enumerableType != null)
                return typeof(ListGraphType<>).MakeGenericType(enumerableType.ToGraphType());

            // Nullable primitives
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                return type.GraphTypeFromAttribute<GraphQLScalarAttribute>() ?? type.GetGraphTypeFromType(true);

            // Get the graph type
            var graphType = type.GraphTypeFromAttribute() ?? type.GetGraphTypeFromType(true);
            return type.GetTypeInfo().IsValueType
                ? typeof(NonNullGraphType<>).MakeGenericType(graphType)
                : graphType;
        }

        public static Type GetEnumerableType(this Type type)
        {
            if (type == typeof(string))
                return null;

            if (type.IsGenericEnumerable())
                return type.GetGenericArguments()[0];

            return type
                .GetInterfaces()
                .Where(t => t.IsGenericEnumerable())
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();
        }

        private static bool IsGenericEnumerable(this Type type)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
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
                .Select(GraphTypeFromAttribute)
                .Where(t => t != null)
                .ToList();
        }

        /// <summary>
        /// Resolve the GraphType for a Type annotated with a GraphQLTypeAttribute.
        /// </summary>
        public static Type GraphTypeFromAttribute(this Type type)
        {
            return type.GraphTypeFromAttribute<GraphQLTypeAttribute>();
        }

        /// <summary>
        /// Resolve the GraphType for a Type annotated with a GraphQLTypeAttribute.
        /// </summary>
        public static Type GraphTypeFromAttribute<TAttribute>(this Type type)
            where TAttribute : GraphQLTypeAttribute
        {
            return type.GetTypeInfo().GetCustomAttribute<TAttribute>()?.GraphType.MakeGenericType(type);
        }
    }
}
