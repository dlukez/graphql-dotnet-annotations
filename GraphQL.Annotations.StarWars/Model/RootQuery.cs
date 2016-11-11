using System.Collections.Generic;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;
using System.Linq;

namespace GraphQL.Annotations.StarWars
{
    [GraphQLObject]
    public class RootQuery
    {
        [GraphQLFunc]
        public IEnumerable<Droid> Droids(ResolveFieldContext<RootQuery> context)
        {
            var db = (StarWarsContext) context.RootValue;
            return db.Droids.ToList();
        }

        [GraphQLFunc]
        public IEnumerable<Human> Humans(ResolveFieldContext<RootQuery> context)
        {
            var db = (StarWarsContext) context.RootValue;
            return db.Humans.ToList();
        }
    }
}