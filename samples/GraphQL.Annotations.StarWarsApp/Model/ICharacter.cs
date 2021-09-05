﻿using System.Collections.Generic;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations.StarWarsApp.Model
{
    [GraphQLInterface]
    public interface ICharacter
    {
        [GraphQLField]
        string Name { get; set; }

        [GraphQLFunc]
        IEnumerable<ICharacter> Friends(IResolveFieldContext context);
    }
}