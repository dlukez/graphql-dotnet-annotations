# GraphQL.Annotations

Attribute-based schema definitions for [GraphQL](https://github.com/graphql-dotnet/graphql-dotnet) in .NET.

Have a look through the sample app and leave some feedback.

## Running the sample app

```
cd src/GraphQL.Annotations.StarWarsApp/
dotnet ef migrations add InitialSetup
dotnet ef database update
dotnet run
```

## Annotating your models

[Full example](./tree/master/GraphQL.Annotations/)

```csharp
// Model/QueryRoot.cs

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

// Model/Droid.cs

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
}
```

## Todo
+ Include Scalar example.
+ Fill out this readme.
