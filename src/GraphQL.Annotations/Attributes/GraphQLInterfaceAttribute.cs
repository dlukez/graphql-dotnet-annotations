using System;
using GraphQL.Annotations.Types;

namespace GraphQL.Annotations.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class GraphQLInterfaceAttribute : GraphQLTypeAttribute
    {
        public GraphQLInterfaceAttribute() : base(typeof(InterfaceGraphType<>))
        {
        }
    }
}