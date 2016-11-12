using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations.StarWarsApp.Model
{
    [GraphQLObject]
    public class QueryRoot : IDisposable
    {
        public StarWarsContext Db = new StarWarsContext();

        [GraphQLFunc]
        public IEnumerable<Droid> Droids(ResolveFieldContext context)
        {
            var db = context.GetDataContext();
            return db.Droids.ToList();
        }

        [GraphQLFunc]
        public IEnumerable<Human> Humans(ResolveFieldContext context)
        {
            var db = context.GetDataContext();
            return db.Humans.ToList();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}