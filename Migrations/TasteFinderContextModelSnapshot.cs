﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TasteFinder.Models;

#nullable disable

namespace TasteFinder.Migrations
{
    [DbContext(typeof(TasteFinderContext))]
    partial class TasteFinderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TasteFinder.Models.Contribution", b =>
                {
                    b.Property<int>("ContributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContributionId"));

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ReviewId")
                        .HasColumnType("int");

                    b.Property<bool>("Up")
                        .HasColumnType("bit");

                    b.HasKey("ContributionId");

                    b.HasIndex("AuthorEmail");

                    b.HasIndex("ReviewId");

                    b.ToTable("Contributions");
                });

            modelBuilder.Entity("TasteFinder.Models.Keyword", b =>
                {
                    b.Property<int>("PossessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PossessionId"));

                    b.Property<string>("RestaurantEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PossessionId");

                    b.HasIndex("RestaurantEmail");

                    b.ToTable("Possessions");
                });

            modelBuilder.Entity("TasteFinder.Models.Photo", b =>
                {
                    b.Property<int>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhotoId"));

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

                    b.Property<bool>("Healthy")
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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Seats")
                        .HasColumnType("bit");

                    b.Property<double>("TotalStars")
                        .HasColumnType("float");

                    b.Property<bool>("Vegan")
                        .HasColumnType("bit");

                    b.Property<double>("WeightedReviewScore")
                        .HasColumnType("float");

                    b.HasKey("Email");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("TasteFinder.Models.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("RestaurantEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReviewId");

                    b.HasIndex("AuthorEmail");

                    b.HasIndex("RestaurantEmail");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("TasteFinder.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

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

            modelBuilder.Entity("TasteFinder.Models.Contribution", b =>
                {
                    b.HasOne("TasteFinder.Models.User", "Author")
                        .WithMany("Contributions")
                        .HasForeignKey("AuthorEmail");

                    b.HasOne("TasteFinder.Models.Review", "Review")
                        .WithMany("Contributions")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Author");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("TasteFinder.Models.Keyword", b =>
                {
                    b.HasOne("TasteFinder.Models.Restaurant", "Restaurant")
                        .WithMany("Keywords")
                        .HasForeignKey("RestaurantEmail")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Restaurant");
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

                    b.HasOne("TasteFinder.Models.Restaurant", "Restaurant")
                        .WithMany()
                        .HasForeignKey("RestaurantEmail")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Author");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("TasteFinder.Models.Restaurant", b =>
                {
                    b.Navigation("Keywords");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("TasteFinder.Models.Review", b =>
                {
                    b.Navigation("Contributions");
                });

            modelBuilder.Entity("TasteFinder.Models.User", b =>
                {
                    b.Navigation("Contributions");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
