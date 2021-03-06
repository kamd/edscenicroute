﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EDScenicRouteWeb.Server.Data.Migrations
{
    [DbContext(typeof(GalacticSystemContext))]
    [Migration("20191124225907_Create")]
    partial class Create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("EDScenicRouteCoreModels.GalacticPOI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("DistanceFromSol");

                    b.Property<string>("GalMapSearch")
                        .HasMaxLength(600);

                    b.Property<string>("GalMapUrl")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(200) UNIQUE")
                        .HasMaxLength(200);

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("GalacticPOIs");
                });

            modelBuilder.Entity("EDScenicRouteCoreModels.GalacticSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(200) UNIQUE")
                        .HasMaxLength(200);

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

                            b1.HasKey("GalacticPOIId");

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

                            b1.HasKey("GalacticSystemId");

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
