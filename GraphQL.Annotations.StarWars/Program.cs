using System;
using GraphQL.Annotations.StarWars;
using GraphQL.Annotations.Types;
using System.Linq;
using GraphQL.Http;

namespace GraphQL.Annotations.StarWarsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitTestData();
            RunQuery();
            Console.ReadKey();
        }

        private static void RunQuery()
        {
            var query = @" {
                droids {
                    droidId
                    name
                    primaryFunction
                    friends {
                        name
                        ... on Human {
                            humanId
                            homePlanet
                            friends1 {
                                friends {
                                    name
                                }
                            }
                            friends2 {
                                name
                                friends {
                                    friends {
                                        name
                                    }
                                }
                            }
                            friends3 {
                                name
                            }
                            friends4 {
                                name
                            }
                        }
                    }
                }
            }";

            using (var db = new StarWarsContext())
            {
                var schema = new Schema<RootQuery>();
                var executer = new DocumentExecuter();
                var writer = new DocumentWriter(true);
                var result = executer.ExecuteAsync(schema, db, query, null).Result;
                var output = writer.Write(result);
                Console.WriteLine(output);
            }
        }

        private static void InitTestData()
        {
            using (var db = new StarWarsContext())
            {
                if (db.Humans.Any())
                    return;

                db.Humans.RemoveRange(db.Humans);
                db.Droids.RemoveRange(db.Droids);
                db.Friendships.RemoveRange(db.Friendships);

                var luke = new Human
                {
                    HumanId = 1,
                    Name = "Luke",
                    HomePlanet = "Tatooine"
                };

                var vader = new Human
                {
                    HumanId = 2,
                    Name = "Vader",
                    HomePlanet = "Tatooine"
                };

                var ash = new Human
                {
                    HumanId = 3,
                    Name = "Ash",
                    HomePlanet = "Cromwell"
                };

                var r2d2 = new Droid
                {
                    DroidId = 1,
                    Name = "R2-D2",
                    PrimaryFunction = "Astromech"
                };

                var c3p0 = new Droid
                {
                    DroidId = 2,
                    Name = "C-3PO",
                    PrimaryFunction = "Protocol"
                };

                db.Humans.Add(luke);
                db.Humans.Add(vader);
                db.Humans.Add(ash);
                db.Droids.Add(r2d2);
                db.Droids.Add(c3p0);

                db.Friendships.Add(new Friendship
                {
                    HumanId = luke.HumanId,
                    DroidId = r2d2.DroidId
                });

                db.Friendships.Add(new Friendship
                {
                    HumanId = luke.HumanId,
                    DroidId = c3p0.DroidId
                });

                db.Friendships.Add(new Friendship
                {
                    HumanId = vader.HumanId,
                    DroidId = r2d2.DroidId
                });

                db.Friendships.Add(new Friendship
                {
                    HumanId = ash.HumanId,
                    DroidId = c3p0.DroidId
                });

                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
            }
        }
    }
}
