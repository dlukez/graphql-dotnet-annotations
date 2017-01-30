using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GraphQL.Annotations.StarWarsApp;

namespace GraphQL.Annotations.StarWarsApp.Migrations
{
    [DbContext(typeof(StarWarsContext))]
    partial class StarWarsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.Droid", b =>
                {
                    b.Property<int>("DroidId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("PrimaryFunction");

                    b.HasKey("DroidId");

                    b.ToTable("Droids");
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.DroidAppearance", b =>
                {
                    b.Property<int>("DroidAppearanceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DroidId");

                    b.Property<int>("Episode");

                    b.Property<string>("Title");

                    b.HasKey("DroidAppearanceId");

                    b.HasIndex("DroidId");

                    b.ToTable("DroidAppearances");
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.Friendship", b =>
                {
                    b.Property<int>("FriendshipId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DroidId");

                    b.Property<int>("HumanId");

                    b.HasKey("FriendshipId");

                    b.HasIndex("DroidId");

                    b.HasIndex("HumanId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.Human", b =>
                {
                    b.Property<int>("HumanId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HomePlanet");

                    b.Property<string>("Name");

                    b.HasKey("HumanId");

                    b.ToTable("Humans");
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.HumanAppearance", b =>
                {
                    b.Property<int>("HumanAppearanceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Episode");

                    b.Property<int>("HumanId");

                    b.Property<string>("Title");

                    b.HasKey("HumanAppearanceId");

                    b.HasIndex("HumanId");

                    b.ToTable("HumanAppearances");
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.DroidAppearance", b =>
                {
                    b.HasOne("GraphQL.Annotations.StarWarsApp.Model.Droid", "Droid")
                        .WithMany("Appearances")
                        .HasForeignKey("DroidId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.Friendship", b =>
                {
                    b.HasOne("GraphQL.Annotations.StarWarsApp.Model.Droid", "Droid")
                        .WithMany()
                        .HasForeignKey("DroidId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GraphQL.Annotations.StarWarsApp.Model.Human", "Human")
                        .WithMany()
                        .HasForeignKey("HumanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GraphQL.Annotations.StarWarsApp.Model.HumanAppearance", b =>
                {
                    b.HasOne("GraphQL.Annotations.StarWarsApp.Model.Human", "Human")
                        .WithMany("Appearances")
                        .HasForeignKey("HumanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
