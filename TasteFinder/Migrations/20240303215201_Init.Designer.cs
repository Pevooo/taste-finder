﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TasteFinder.Models;

#nullable disable

namespace TasteFinder.Migrations
{
    [DbContext(typeof(TasteFinderContext))]
    [Migration("20240303215201_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TasteFinder.Models.Photo", b =>
                {
                    b.Property<int>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhotoId"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("OwnerEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("PhotoData")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("PhotoId");

                    b.HasIndex("OwnerEmail");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("TasteFinder.Models.Restaurant", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Buffet")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CloseTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("Delivery")
                        .HasColumnType("bit");

                    b.Property<bool>("Desserts")
                        .HasColumnType("bit");

                    b.Property<bool>("Drinks")
                        .HasColumnType("bit");

                    b.Property<bool>("Food")
                        .HasColumnType("bit");

                    b.Property<bool>("KidsArea")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OpenAir")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OpenTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<bool>("Seats")
                        .HasColumnType("bit");

                    b.HasKey("Email");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("TasteFinder.Models.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"), 1L, 1);

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Contribution")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReviewId");

                    b.HasIndex("AuthorEmail");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("TasteFinder.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Contribution")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TasteFinder.Models.Photo", b =>
                {
                    b.HasOne("TasteFinder.Models.Restaurant", "Owner")
                        .WithMany("Photos")
                        .HasForeignKey("OwnerEmail")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TasteFinder.Models.Review", b =>
                {
                    b.HasOne("TasteFinder.Models.User", "Author")
                        .WithMany("Reviews")
                        .HasForeignKey("AuthorEmail")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Author");
                });

            modelBuilder.Entity("TasteFinder.Models.Restaurant", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("TasteFinder.Models.User", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
