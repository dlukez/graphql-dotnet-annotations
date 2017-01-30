using GraphQL.Annotations.Attributes;

namespace GraphQL.Annotations.StarWarsApp.Model
{
    [GraphQLEnum]
    public enum Episode
    {
        ANewHope = 4,
        EmpireStrikesBack = 5,
        ReturnOfTheJedi = 6
    }
}