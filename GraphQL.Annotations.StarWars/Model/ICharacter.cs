using System.Collections.Generic;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations.StarWars
{
    [GraphQLInterface]
    public interface ICharacter
    {
        string Name { get; set; }
        IEnumerable<ICharacter> Friends(ResolveFieldContext context);
    }
}