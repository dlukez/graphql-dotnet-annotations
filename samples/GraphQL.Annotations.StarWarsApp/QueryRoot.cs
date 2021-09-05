using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Annotations.Attributes;
using GraphQL.Annotations.StarWarsApp.Model;
using GraphQL.Types;

namespace GraphQL.Annotations.StarWarsApp
{
    [GraphQLObject]
    public class QueryRoot : IDisposable
    {
        public StarWarsContext Db { get; } = new StarWarsContext();

        [GraphQLFunc]
        public IEnumerable<Droid> Droids(IResolveFieldContext context)
        {
            var db = context.GetDataContext();
            return db.Droids.ToList();
        }

        [GraphQLFunc]
        public IEnumerable<Human> Humans(IResolveFieldContext context)
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