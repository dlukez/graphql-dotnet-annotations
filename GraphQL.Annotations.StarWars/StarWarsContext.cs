using GraphQL.Annotations.Attributes;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Annotations.StarWars
{
    [GraphQLObject]
    public class StarWarsContext : DbContext
    {
        [GraphQLField]
        public DbSet<Human> Humans { get; set; }

        [GraphQLField]
        public DbSet<Droid> Droids { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./starwars.db");
        }
    }

    public static class ContextExtensions
    {
        public static TDataContext GetDataContext<TDataContext, TGraphType>(this ResolveFieldContext<TGraphType> resolveContext) where TDataContext : DbContext
        {
            return resolveContext.RootValue as TDataContext;
        }
    }
}