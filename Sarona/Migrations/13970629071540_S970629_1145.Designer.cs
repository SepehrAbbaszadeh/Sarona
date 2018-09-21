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
    [Migration("13970629071540_S970629_1145")]
    partial class S970629_1145
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.AccessSequence", "'AccessSequence', '', '1', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.PbxSequence", "'PbxSequence', '', '1', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.RemoteSequence", "'RemoteSequence', '', '1', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sarona.Models.Abbreviation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abb")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("Abb")
                        .IsUnique();

                    b.ToTable("Abbreviations");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Abbreviation");
                });

            modelBuilder.Entity("Sarona.Models.Link", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Channels");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("Direction");

                    b.Property<long>("End1Id");

                    b.Property<long>("End2Id");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<long?>("OtherLinkId");

                    b.Property<string>("Remark");

                    b.Property<int>("Type");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("End1Id");

                    b.HasIndex("End2Id");

                    b.HasIndex("OtherLinkId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Sarona.Models.LinkHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Channels");

                    b.Property<int>("Direction");

                    b.Property<long>("LinkId");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Remark");

                    b.Property<int>("Type");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("LinkId");

                    b.ToTable("LinkHistories");
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

                    b.Property<DateTime>("CreatedOn");

                    b.Property<long?>("CustomerId");

                    b.Property<long>("ExchangeId");

                    b.Property<int>("InstalledCapacity");

                    b.Property<string>("Manufacturer")
                        .IsRequired();

                    b.Property<string>("Model")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<int>("NetworkType");

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<long?>("ParentId");

                    b.Property<string>("Remark");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<int>("UsedCapacity");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ExchangeId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("ParentId");

                    b.ToTable("NetworkElements");
                });

            modelBuilder.Entity("Sarona.Models.NumberingPool", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChargingCase")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<byte>("Dgsb");

                    b.Property<DateTime>("ExpireDate");

                    b.Property<bool>("IsFloat");

                    b.Property<byte>("Max");

                    b.Property<byte>("Min");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("NumberType")
                        .IsRequired();

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<string>("Prefix")
                        .IsRequired();

                    b.Property<string>("Remark");

                    b.Property<string>("RoutingType")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("Prefix")
                        .IsUnique();

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

            modelBuilder.Entity("Sarona.Models.Customer", b =>
                {
                    b.HasBaseType("Sarona.Models.Abbreviation");


                    b.ToTable("Customer");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("Sarona.Models.Exchange", b =>
                {
                    b.HasBaseType("Sarona.Models.Abbreviation");

                    b.Property<int>("Area");

                    b.Property<string>("Providence");

                    b.ToTable("Exchange");

                    b.HasDiscriminator().HasValue("Exchange");
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

            modelBuilder.Entity("Sarona.Models.LinkHistory", b =>
                {
                    b.HasOne("Sarona.Models.Link")
                        .WithMany("Histories")
                        .HasForeignKey("LinkId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sarona.Models.NetworkElement", b =>
                {
                    b.HasOne("Sarona.Models.Customer", "Customer")
                        .WithMany("Owned")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sarona.Models.Exchange", "Exchange")
                        .WithMany("NetworkElements")
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sarona.Models.NetworkElement", "Parent")
                        .WithMany("NetworkElements")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sarona.Models.NumberingPoolNetworkElement", b =>
                {
                    b.HasOne("Sarona.Models.NetworkElement", "Element")
                        .WithMany("NumberingPoolNetworkElements")
                        .HasForeignKey("NetworkElementId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sarona.Models.NumberingPool", "Numbering")
                        .WithMany("NumberingPoolNetworkElements")
                        .HasForeignKey("NumberingPoolId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
