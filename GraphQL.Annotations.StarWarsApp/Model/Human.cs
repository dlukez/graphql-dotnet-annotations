using System.Collections.Generic;
using System.Linq;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;

namespace GraphQL.Annotations.StarWarsApp.Model
{
    [GraphQLObject]
    public class Human : ICharacter
    {
        [GraphQLField]
        public int HumanId { get; set; }

        [GraphQLField]
        public string Name { get; set; }

        [GraphQLField]
        public string HomePlanet { get; set; }

        [GraphQLFunc]
        public IEnumerable<ICharacter> Friends(ResolveFieldContext context)
        {
            var db = context.GetDataContext();
            return db.Friendships
                .Where(f => f.HumanId == ((Human)context.Source).HumanId)
                .Select(f => f.Droid);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {HumanId}";
        }
    }
}
