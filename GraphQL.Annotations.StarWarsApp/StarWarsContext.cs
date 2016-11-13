using GraphQL.Annotations.Attributes;
using GraphQL.Annotations.StarWarsApp.Model;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Annotations.StarWarsApp
{
    [GraphQLObject]
    public class StarWarsContext : DbContext
    {
        [GraphQLField]
        public DbSet<Human> Humans { get; set; }

        [GraphQLField]
        public DbSet<Droid> Droids { get; set; }

        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<HumanAppearance> HumanAppearances { get; set; }
        public DbSet<DroidAppearance> DroidAppearances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./starwars.db");
        }
    }

    public static class ContextExtensions
    {
        public static StarWarsContext GetDataContext(this ResolveFieldContext context)
        {
            return context.RootValue as StarWarsContext ?? (context.RootValue as QueryRoot)?.Db;
        }
    }
}