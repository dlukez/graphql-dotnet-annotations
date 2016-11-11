using System;
using GraphQL.Annotations.Types;

namespace GraphQL.Annotations.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class GraphQLEnumAttribute : GraphQLTypeAttribute
    {
        public GraphQLEnumAttribute() : base(typeof(EnumerationGraphType<>))
        {
        }
    }
}
