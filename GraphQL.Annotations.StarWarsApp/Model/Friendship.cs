using GraphQL.Annotations.Attributes;

namespace GraphQL.Annotations.StarWarsApp.Model
{
    [GraphQLObject]
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public int HumanId { get; set; }
        public int DroidId { get; set; }
        public Human Human { get; set; }
        public Droid Droid { get; set; }
    }
}