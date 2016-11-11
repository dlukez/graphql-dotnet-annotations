using System.Collections.Generic;
using GraphQL.Annotations.Attributes;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Annotations.StarWars
{
    public class StarWarsContext : DbContext
    {
        public DbSet<Human> Humans { get; set; }
        public DbSet<Droid> Droids { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./starwars.db");
        }
    }

    [GraphQLInterface]
    public interface ICharacter
    {
        string Name { get; set; }
        List<Episode> AppearsIn { get; set; }
        List<Friendship> Friendships { get; set; }
    }

    [GraphQLObject]
    public class Human : ICharacter
    {
        public List<Episode> AppearsIn { get; set; }
        public List<Friendship> Friendships { get; set; }

        [GraphQLField]
        public int HumanId { get; set; }

        [GraphQLField]
        public string Name { get; set; }

        [GraphQLField]
        public string HomePlanet { get; set; }

        [GraphQLFunc]
        public static List<Droid> Friends(ResolveFieldContext<Human> context)
        {

        }

        public override string ToString()
        {
            return HumanId.ToString();
        }
    }

    [GraphQLObject]
    public class Droid : ICharacter
    {
        [GraphQLField]
        public int DroidId { get; set; }

        [GraphQLField]
        public string Name { get; set; }

        [GraphQLField]
        public string PrimaryFunction { get; set; }

        public List<Episode> AppearsIn { get; set; }

        public List<Friendship> Friendships { get; set; }

        public override string ToString()
        {
            return DroidId.ToString();
        }
    }

    [GraphQLObject]
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public int HumanId { get; set; }
        public int DroidId { get; set; }
        public Human Human { get; set; }
        public Droid Droid { get; set; }
    }

    [GraphQLEnum]
    public enum Episode
    {
        NEWHOPE = 4,
        EMPIRE = 5,
        JEDI = 6
    }
}
