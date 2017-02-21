using System;
using System.Reflection;
using GraphQL.Types;

namespace GraphQL.Annotations
{
    public class QueryArgumentParameterInfo : QueryArgument
    {
        public QueryArgumentParameterInfo(ParameterInfo parameter) : base (parameter.ParameterType.ToGraphType(parameter.HasDefaultValue))
        {
            Name = parameter.Name;
            DefaultValue = parameter.HasDefaultValue ? parameter.DefaultValue : null;
            Position = parameter.Position;
            ParameterType = parameter.ParameterType;
        }

        public int Position { get; set; }
        public Type ParameterType { get; set; }
    }
}