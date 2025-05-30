﻿// <auto-generated />
using System;
using EventApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250515191247_Add_Reminder_Update_Event")]
    partial class Add_Reminder_Update_Event
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("PWAApi.ApiService.Models.Reminder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("CompletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriorityLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("PWAApi.ApiService.Models.Taxonomy.Taxonomy", b =>
                {
                    b.Property<int>("TaxonKey")
                        .HasColumnType("int");

                    b.Property<string>("AcceptedScientificName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AcceptedTaxonKey")
                        .HasColumnType("int");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ClassKey")
                        .HasColumnType("int");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FamilyKey")
                        .HasColumnType("int");

                    b.Property<string>("Genus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GenusKey")
                        .HasColumnType("int");

                    b.Property<string>("IUCNRedListCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kingdom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("KingdomKey")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfOccurences")
                        .HasColumnType("int");

                    b.Property<string>("Order")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderKey")
                        .HasColumnType("int");

                    b.Property<string>("Phylum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PhylumKey")
                        .HasColumnType("int");

                    b.Property<string>("ScientificName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SpeciesKey")
                        .HasColumnType("int");

                    b.Property<string>("TaxonRank")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxonomicStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaxonKey");

                    b.ToTable("Taxonomy");
                });

            modelBuilder.Entity("PWAApi.ApiService.Models.Taxonomy.VernacularName", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsPreferredName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LifeStage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TaxonKey")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TaxonKey");

                    b.ToTable("VernacularNames");
                });

            modelBuilder.Entity("PWAApi.ApiService.Models.Taxonomy.VernacularName", b =>
                {
                    b.HasOne("PWAApi.ApiService.Models.Taxonomy.Taxonomy", "Taxonomy")
                        .WithMany("VernacularNames")
                        .HasForeignKey("TaxonKey");

                    b.Navigation("Taxonomy");
                });

            modelBuilder.Entity("PWAApi.ApiService.Models.Taxonomy.Taxonomy", b =>
                {
                    b.Navigation("VernacularNames");
                });
#pragma warning restore 612, 618
        }
    }
}
