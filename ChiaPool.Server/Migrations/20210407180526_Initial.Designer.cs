﻿// <auto-generated />
using System;
using System.Net;
using ChiaPool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ChiaPool.Migrations
{
    [DbContext(typeof(MinerContext))]
    [Migration("20210407180526_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ChiaPool.Models.Miner", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<IPAddress>("LastAddress")
                        .HasColumnType("inet");

                    b.Property<short>("LastPlotCount")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("NextIncrement")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long>("PlotMinutes")
                        .HasColumnType("bigint");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("Miners");
                });

            modelBuilder.Entity("ChiaPool.Models.PlotTransfer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Cost")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DownloadAddress")
                        .HasColumnType("text");

                    b.Property<long>("MinerId")
                        .HasColumnType("bigint");

                    b.Property<long>("PlotterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("PlotTransfers");
                });

            modelBuilder.Entity("ChiaPool.Models.Plotter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<short>("AvailablePlots")
                        .HasColumnType("smallint");

                    b.Property<short>("Capacity")
                        .HasColumnType("smallint");

                    b.Property<IPAddress>("LastAddress")
                        .HasColumnType("inet");

                    b.Property<DateTimeOffset>("LastTick")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long>("PlotMinutes")
                        .HasColumnType("bigint");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Plotters");
                });

            modelBuilder.Entity("ChiaPool.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChiaPool.Models.Miner", b =>
                {
                    b.HasOne("ChiaPool.Models.User", "Owner")
                        .WithMany("Miners")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ChiaPool.Models.Plotter", b =>
                {
                    b.HasOne("ChiaPool.Models.User", "Owner")
                        .WithMany("Plotters")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ChiaPool.Models.User", b =>
                {
                    b.Navigation("Miners");

                    b.Navigation("Plotters");
                });
#pragma warning restore 612, 618
        }
    }
}