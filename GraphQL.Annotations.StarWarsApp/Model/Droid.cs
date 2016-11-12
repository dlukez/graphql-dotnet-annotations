using System.Collections.Generic;
using System.Linq;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations.StarWarsApp.Model
{
    [GraphQLObject]
    public class Droid : ICharacter
    {
        [GraphQLField]
        public int DroidId { get; set; }

        [GraphQLField]
        public string Name { get; set; }

        [GraphQLField]
        public string PrimaryFunction { get; set; }

        [GraphQLFunc]
        public IEnumerable<ICharacter> Friends(ResolveFieldContext context)
        {
            var db = context.GetDataContext();
            return db.Friendships
                .Where(f => f.DroidId == ((Droid)context.Source).DroidId)
                .Select(f => f.Human);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {DroidId}";
        }
    }
}