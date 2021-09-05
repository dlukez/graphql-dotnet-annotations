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

        public List<HumanAppearance> Appearances { get; set; }

        [GraphQLFunc]
        public IEnumerable<Episode> AppearsIn(IResolveFieldContext context)
        {
            var db = context.GetDataContext();
            var human = this;
            return db.HumanAppearances
                    .Where(a => human.HumanId == a.HumanId)
                    .Select(a => a.Episode);
        }

        [GraphQLFunc]
        public IEnumerable<ICharacter> Friends(IResolveFieldContext context)
        {
            var db = context.GetDataContext();
            return db.Friendships
                .Where(f => f.HumanId == HumanId)
                .Select(f => f.Droid);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {HumanId}";
        }
    }
}
