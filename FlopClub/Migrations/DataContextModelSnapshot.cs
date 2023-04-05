﻿// <auto-generated />
using System;
using FlopClub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlopClub.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FlopClub.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int?>("GameId1")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("Suit")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("GameId1");

                    b.HasIndex("PlayerId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("FlopClub.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActivePlayers")
                        .HasColumnType("int");

                    b.Property<decimal>("BigBlindValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Dealer")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("Pot")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SmallBlindValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("FlopClub.Models.Lobby", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Lobbies");
                });

            modelBuilder.Entity("FlopClub.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Chips")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("Hand")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("TimeToAct")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("FlopClub.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("HandsPlayed")
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<decimal>("TotalWinnings")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LobbyUser", b =>
                {
                    b.Property<int>("LobbiesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("LobbiesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("LobbyUser");
                });

            modelBuilder.Entity("FlopClub.Models.Card", b =>
                {
                    b.HasOne("FlopClub.Models.Game", null)
                        .WithMany("Board")
                        .HasForeignKey("GameId");

                    b.HasOne("FlopClub.Models.Game", null)
                        .WithMany("Deck")
                        .HasForeignKey("GameId1");

                    b.HasOne("FlopClub.Models.Player", null)
                        .WithMany("Cards")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("FlopClub.Models.Lobby", b =>
                {
                    b.HasOne("FlopClub.Models.Game", null)
                        .WithOne("Lobby")
                        .HasForeignKey("FlopClub.Models.Lobby", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlopClub.Models.Player", b =>
                {
                    b.HasOne("FlopClub.Models.Game", null)
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LobbyUser", b =>
                {
                    b.HasOne("FlopClub.Models.Lobby", null)
                        .WithMany()
                        .HasForeignKey("LobbiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlopClub.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlopClub.Models.Game", b =>
                {
                    b.Navigation("Board");

                    b.Navigation("Deck");

                    b.Navigation("Lobby")
                        .IsRequired();

                    b.Navigation("Players");
                });

            modelBuilder.Entity("FlopClub.Models.Player", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
