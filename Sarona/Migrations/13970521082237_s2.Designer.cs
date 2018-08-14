﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sarona.Models;

namespace Sarona.Migrations
{
    [DbContext(typeof(SaronaContext))]
    [Migration("13970521082237_s2")]
    partial class s2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.LinkSequence", "'LinkSequence', '', '1', '2', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sarona.Models.Abbreviation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abb")
                        .IsRequired();

                    b.Property<int?>("Area");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("Abb")
                        .IsUnique();

                    b.ToTable("Abbreviations");
                });

            modelBuilder.Entity("Sarona.Models.Link", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelNo");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("Direction");

                    b.Property<long>("End1Id");

                    b.Property<long>("End2Id");

                    b.Property<long>("OtherLinkId");

                    b.Property<string>("Remark");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("End1Id");

                    b.HasIndex("End2Id");

                    b.HasIndex("OtherLinkId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Sarona.Models.Misc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Miscs");
                });

            modelBuilder.Entity("Sarona.Models.NetworkElement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AbbreviationId");

                    b.Property<int>("InstalledCapacity");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("Model");

                    b.Property<string>("Name");

                    b.Property<long?>("ParentId");

                    b.Property<string>("Remark");

                    b.Property<string>("Type");

                    b.Property<int>("UsedCapacity");

                    b.HasKey("Id");

                    b.HasIndex("AbbreviationId");

                    b.HasIndex("ParentId");

                    b.ToTable("NetworkElements");
                });

            modelBuilder.Entity("Sarona.Models.NumberingPool", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChargingCase");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("Dgsb");

                    b.Property<DateTime>("ExpireDate");

                    b.Property<long>("From");

                    b.Property<byte>("Max");

                    b.Property<byte>("Min");

                    b.Property<string>("NumberType");

                    b.Property<string>("Remark");

                    b.Property<string>("RoutingType");

                    b.Property<long>("To");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("NumberingPools");
                });

            modelBuilder.Entity("Sarona.Models.NumberingPoolNetworkElement", b =>
                {
                    b.Property<long>("NumberingPoolId");

                    b.Property<long>("NetworkElementId");

                    b.HasKey("NumberingPoolId", "NetworkElementId");

                    b.HasIndex("NetworkElementId");

                    b.ToTable("NumberingPoolNetworkElement");
                });

            modelBuilder.Entity("Sarona.Models.Link", b =>
                {
                    b.HasOne("Sarona.Models.NetworkElement", "End1")
                        .WithMany("LinksOnEnd1")
                        .HasForeignKey("End1Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sarona.Models.NetworkElement", "End2")
                        .WithMany("LinksOnEnd2")
                        .HasForeignKey("End2Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sarona.Models.Link", "OtherLink")
                        .WithMany()
                        .HasForeignKey("OtherLinkId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sarona.Models.NetworkElement", b =>
                {
                    b.HasOne("Sarona.Models.Abbreviation", "Abbreviation")
                        .WithMany("NetworkElements")
                        .HasForeignKey("AbbreviationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sarona.Models.NetworkElement", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Sarona.Models.NumberingPoolNetworkElement", b =>
                {
                    b.HasOne("Sarona.Models.NetworkElement", "Element")
                        .WithMany("NumberingPoolNetworkElements")
                        .HasForeignKey("NetworkElementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sarona.Models.NumberingPool", "Numbering")
                        .WithMany("NumberingPoolNetworkElements")
                        .HasForeignKey("NumberingPoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
