﻿using System;
using ImperitWASM.Server.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#pragma warning disable 612, 618

namespace ImperitWASM.Server.Migrations
{
    [DbContext(typeof(Context))]
    partial class ImperitContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ImperitWASM.Client.Data.Session", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("P")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("P");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerName");

                    b.ToTable("Action");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Action");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Current")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Player", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Alive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsHuman")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Money")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StringPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShapeId1")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ShapeId1");

                    b.ToTable("Point");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Power", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Alive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Final")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Income")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Money")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Soldiers")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Powers");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Province", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerName")
                        .HasColumnType("TEXT");

                    b.Property<int>("SoldiersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameId", "RegionId");

                    b.HasIndex("PlayerName");

                    b.HasIndex("RegionId");

                    b.HasIndex("SoldiersId")
                        .IsUnique();

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Regiment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SoldiersId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TypeSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SoldiersId");

                    b.HasIndex("TypeSymbol");

                    b.ToTable("Regiment");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Region", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Color_A")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Color_B")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Color_G")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Color_R")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ShapeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SoldiersId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("StrokeWidth")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ShapeId")
                        .IsUnique();

                    b.HasIndex("SoldiersId")
                        .IsUnique();

                    b.ToTable("Region");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Region");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.RegionSoldierType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SoldierTypeSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.HasIndex("SoldierTypeSymbol");

                    b.ToTable("RegionSoldierType");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Shape", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CenterId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CenterId")
                        .IsUnique();

                    b.ToTable("Shape");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.SoldierType", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<int>("AttackPower")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefensePower")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Weight")
                        .HasColumnType("INTEGER");

                    b.HasKey("Symbol");

                    b.ToTable("SoldierType");

                    b.HasDiscriminator<string>("Discriminator").HasValue("SoldierType");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Soldiers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Soldiers");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Loan", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Action");

                    b.Property<int>("Debt")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Loan");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Manoeuvre", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Action");

                    b.Property<int>("RegionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SoldiersId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("SoldiersId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("Manoeuvre");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Land", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Region");

                    b.Property<int>("DefaultInstabilityInt")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Earnings")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasPort")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFinal")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsStart")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Land");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Mountains", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Region");

                    b.HasDiscriminator().HasValue("Mountains");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Sea", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Region");

                    b.HasDiscriminator().HasValue("Sea");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Pedestrian", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.SoldierType");

                    b.HasDiscriminator().HasValue("Pedestrian");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Ship", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.SoldierType");

                    b.Property<int>("Capacity")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Capacity");

                    b.HasDiscriminator().HasValue("Ship");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Elephant", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Pedestrian");

                    b.Property<int>("Capacity")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Capacity");

                    b.Property<int>("Speed")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Speed");

                    b.HasDiscriminator().HasValue("Elephant");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.OutlandishShip", b =>
                {
                    b.HasBaseType("ImperitWASM.Shared.Data.Ship");

                    b.Property<int>("Speed")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Speed");

                    b.HasDiscriminator().HasValue("OutlandishShip");
                });

            modelBuilder.Entity("ImperitWASM.Client.Data.Session", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Player", null)
                        .WithMany()
                        .HasForeignKey("P")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Action", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Player", null)
                        .WithMany("ActionList")
                        .HasForeignKey("PlayerName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Player", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Point", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Shape", null)
                        .WithMany("Points")
                        .HasForeignKey("ShapeId1");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Province", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImperitWASM.Shared.Data.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImperitWASM.Shared.Data.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImperitWASM.Shared.Data.Soldiers", "Soldiers")
                        .WithOne()
                        .HasForeignKey("ImperitWASM.Shared.Data.Province", "SoldiersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Region");

                    b.Navigation("Soldiers");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Regiment", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Soldiers", null)
                        .WithMany("Regiments")
                        .HasForeignKey("SoldiersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImperitWASM.Shared.Data.SoldierType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Region", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Shape", "Shape")
                        .WithOne()
                        .HasForeignKey("ImperitWASM.Shared.Data.Region", "ShapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImperitWASM.Shared.Data.Soldiers", "Soldiers")
                        .WithOne()
                        .HasForeignKey("ImperitWASM.Shared.Data.Region", "SoldiersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shape");

                    b.Navigation("Soldiers");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.RegionSoldierType", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Region", null)
                        .WithMany("RegionSoldierTypes")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImperitWASM.Shared.Data.SoldierType", "SoldierType")
                        .WithMany()
                        .HasForeignKey("SoldierTypeSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SoldierType");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Shape", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Point", "Center")
                        .WithOne()
                        .HasForeignKey("ImperitWASM.Shared.Data.Shape", "CenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Center");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Manoeuvre", b =>
                {
                    b.HasOne("ImperitWASM.Shared.Data.Soldiers", "Soldiers")
                        .WithOne()
                        .HasForeignKey("ImperitWASM.Shared.Data.Manoeuvre", "SoldiersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Soldiers");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Player", b =>
                {
                    b.Navigation("ActionList");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Region", b =>
                {
                    b.Navigation("RegionSoldierTypes");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Shape", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("ImperitWASM.Shared.Data.Soldiers", b =>
                {
                    b.Navigation("Regiments");
                });
#pragma warning restore 612, 618
        }
    }
}
