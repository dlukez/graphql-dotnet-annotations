using System;
using System.Linq;
using GraphQL.Annotations.StarWars.Model;
using GraphQL.Annotations.Types;
using GraphQL.Http;

namespace GraphQL.Annotations.StarWars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitTestData();
            RunQuery();
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
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
                        }
                    }
                }
            }";

            var executer = new DocumentExecuter();
            var writer = new DocumentWriter(true);
            string output1, output2;

            // Example 1 - a QueryRoot.
            using (var root = new QueryRoot())
            using (var schema = new Schema<QueryRoot>())
            {
                var result = executer.ExecuteAsync(schema, root, query, null).Result;
                output1 = writer.Write(result);
                Console.WriteLine("Example 1 output (QueryRoot):");
                Console.WriteLine("-----------------------------");
                Console.WriteLine(output1);
                Console.WriteLine();
            }

            // Example 2 - annotating the context directly
            // I get the feeling there are reasons why wouldn't
            // want to do this but for simple scenarios it seems to suffice.
            using (var db = new StarWarsContext())
            using (var schema = new Schema<StarWarsContext>())
            {
                var result = executer.ExecuteAsync(schema, db, query, null).Result;
                output2 = writer.Write(result);
                Console.WriteLine("Example 2 output (StarWarsContext):");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine(output2);
                Console.WriteLine();
            }

            // Confirm we got the same result, just 'cause...
            if (output1 == output2)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("✓ Outputs are the same");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("× Outputs are different");
            }

            Console.ForegroundColor = ConsoleColor.Black;
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
