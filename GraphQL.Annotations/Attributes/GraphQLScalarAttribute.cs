using System;
using GraphQL.Annotations.Types;

namespace GraphQL.Annotations.Attributes
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = false)]
    public class GraphQLScalarAttribute : GraphQLTypeAttribute
    {
        public GraphQLScalarAttribute() : base(typeof(ScalarGraphType<>))
        {
        }
    }
}
