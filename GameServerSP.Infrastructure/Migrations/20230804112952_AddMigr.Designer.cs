﻿// <auto-generated />
using System;
using GameServerSP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameServerSP.Infrastructure.Migrations
{
    [DbContext(typeof(GameServerContext))]
    [Migration("20230804112952_AddMigr")]
    partial class AddMigr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.20");

            modelBuilder.Entity("GameServerSP.Domain.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            Id = new Guid("95b76c18-3e66-44b0-b692-95e8c0dd3de8"),
                            PlayerId = 1
                        },
                        new
                        {
                            Id = new Guid("3a82c65a-c527-414f-8be4-54aaac85e7bf"),
                            PlayerId = 2
                        },
                        new
                        {
                            Id = new Guid("5dbf2f74-7dba-4555-a321-ab87e9f1db3e"),
                            PlayerId = 3
                        },
                        new
                        {
                            Id = new Guid("92847dd1-e313-4ee6-9e24-05b3e61e22af"),
                            PlayerId = 4
                        });
                });

            modelBuilder.Entity("GameServerSP.Domain.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Coins")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Rolls")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Coins = 10,
                            Name = "Pavel",
                            Rolls = 10
                        },
                        new
                        {
                            Id = 2,
                            Coins = 15,
                            Name = "Ivan",
                            Rolls = 15
                        },
                        new
                        {
                            Id = 3,
                            Coins = 20,
                            Name = "Maria",
                            Rolls = 20
                        },
                        new
                        {
                            Id = 4,
                            Coins = 25,
                            Name = "Nadina",
                            Rolls = 25
                        });
                });

            modelBuilder.Entity("GameServerSP.Domain.Entities.Device", b =>
                {
                    b.HasOne("GameServerSP.Domain.Entities.Player", "Player")
                        .WithMany("Devices")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("GameServerSP.Domain.Entities.Player", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
