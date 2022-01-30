﻿// <auto-generated />
using System;
using CDN.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CDN.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CDN.Models.AppSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AuditWebhookUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("CDN.Models.CDNEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsDir")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserId");

                    b.ToTable("CDNEntries");
                });

            modelBuilder.Entity("CDN.Models.CDNTempEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EntryId")
                        .HasColumnType("int");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("EntryId")
                        .IsUnique();

                    b.ToTable("CDNTEmpEntries");
                });

            modelBuilder.Entity("CDN.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("TokenHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("TokenSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CDN.Models.CDNEntry", b =>
                {
                    b.HasOne("CDN.Models.CDNEntry", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CDN.Models.User", "UploadedBy")
                        .WithMany("Entries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("UploadedBy");
                });

            modelBuilder.Entity("CDN.Models.CDNTempEntry", b =>
                {
                    b.HasOne("CDN.Models.CDNEntry", "CDNEntry")
                        .WithOne("CDNTempEntry")
                        .HasForeignKey("CDN.Models.CDNTempEntry", "EntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CDNEntry");
                });

            modelBuilder.Entity("CDN.Models.CDNEntry", b =>
                {
                    b.Navigation("CDNTempEntry")
                        .IsRequired();
                });

            modelBuilder.Entity("CDN.Models.User", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
