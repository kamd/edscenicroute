﻿// <auto-generated />
using EDScenicRouteCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDScenicRouteCore.Migrations
{
    [DbContext(typeof(GalacticSystemContext))]
    [Migration("20180923154435_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065");

            modelBuilder.Entity("EDScenicRouteCoreModels.GalacticPOI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("DistanceFromSol");

                    b.Property<string>("GalMapSearch");

                    b.Property<string>("GalMapUrl");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("GalacticPOIs");
                });

            modelBuilder.Entity("EDScenicRouteCoreModels.GalacticSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("GalacticSystems");
                });

            modelBuilder.Entity("EDScenicRouteCoreModels.GalacticPOI", b =>
                {
                    b.OwnsOne("EDScenicRouteCoreModels.Vector3", "Coordinates", b1 =>
                        {
                            b1.Property<int>("GalacticPOIId");

                            b1.Property<float>("X");

                            b1.Property<float>("Y");

                            b1.Property<float>("Z");

                            b1.ToTable("GalacticPOIs");

                            b1.HasOne("EDScenicRouteCoreModels.GalacticPOI")
                                .WithOne("Coordinates")
                                .HasForeignKey("EDScenicRouteCoreModels.Vector3", "GalacticPOIId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("EDScenicRouteCoreModels.GalacticSystem", b =>
                {
                    b.OwnsOne("EDScenicRouteCoreModels.Vector3", "Coordinates", b1 =>
                        {
                            b1.Property<int>("GalacticSystemId");

                            b1.Property<float>("X");

                            b1.Property<float>("Y");

                            b1.Property<float>("Z");

                            b1.ToTable("GalacticSystems");

                            b1.HasOne("EDScenicRouteCoreModels.GalacticSystem")
                                .WithOne("Coordinates")
                                .HasForeignKey("EDScenicRouteCoreModels.Vector3", "GalacticSystemId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
