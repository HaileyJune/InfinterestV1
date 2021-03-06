﻿// <auto-generated />
using System;
using Infinterest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infinterest.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Infinterest.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("addressLine1");

                    b.Property<string>("addressLine2");

                    b.Property<string>("city");

                    b.Property<string>("postalCode");

                    b.Property<string>("state");

                    b.HasKey("AddressId");

                    b.ToTable("address");
                });

            modelBuilder.Entity("Infinterest.Models.Area", b =>
                {
                    b.Property<int>("AreaId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EventId");

                    b.Property<int?>("VendorUserId");

                    b.Property<string>("area");

                    b.HasKey("AreaId");

                    b.HasIndex("EventId");

                    b.HasIndex("VendorUserId");

                    b.ToTable("areas");

                    b.HasData(
                        new { AreaId = 1, area = "Basement" },
                        new { AreaId = 2, area = "Bathroom" },
                        new { AreaId = 3, area = "Bedroom" },
                        new { AreaId = 4, area = "Dining Room" },
                        new { AreaId = 5, area = "Family / Media Room" },
                        new { AreaId = 6, area = "Garage" },
                        new { AreaId = 7, area = "Kitchen" },
                        new { AreaId = 8, area = "Living Room" },
                        new { AreaId = 9, area = "Office" },
                        new { AreaId = 10, area = "Patio" },
                        new { AreaId = 11, area = "Yard" },
                        new { AreaId = 12, area = "Other" }
                    );
                });

            modelBuilder.Entity("Infinterest.Models.Business", b =>
                {
                    b.Property<int>("BusinessId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("VendorUserId");

                    b.Property<string>("business");

                    b.HasKey("BusinessId");

                    b.HasIndex("VendorUserId");

                    b.ToTable("business");

                    b.HasData(
                        new { BusinessId = 1, business = "Art" },
                        new { BusinessId = 2, business = "Baby and Kids" },
                        new { BusinessId = 3, business = "Entertainment and Music" },
                        new { BusinessId = 4, business = "Fashion and Apparel" },
                        new { BusinessId = 5, business = "Food/Nutrition and Beverage" },
                        new { BusinessId = 6, business = "Furnishings" },
                        new { BusinessId = 7, business = "Health and Wellness" },
                        new { BusinessId = 8, business = "Home Based Business" },
                        new { BusinessId = 9, business = "Interior Design" },
                        new { BusinessId = 10, business = "Jewelery" },
                        new { BusinessId = 11, business = "Landscapers and Contractors" },
                        new { BusinessId = 12, business = "Pets and Animals" },
                        new { BusinessId = 13, business = "Photographers" },
                        new { BusinessId = 14, business = "Technology and Security" },
                        new { BusinessId = 15, business = "Wedding" },
                        new { BusinessId = 16, business = "Other" }
                    );
                });

            modelBuilder.Entity("Infinterest.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrokerId");

                    b.Property<bool>("Confirmed");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ListingId");

                    b.Property<DateTime>("OpenHouseDateTime");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("EventId");

                    b.HasIndex("BrokerId");

                    b.HasIndex("ListingId");

                    b.ToTable("events");
                });

            modelBuilder.Entity("Infinterest.Models.Listing", b =>
                {
                    b.Property<int>("ListingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<bool>("Availible");

                    b.Property<int>("BrokerId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImgUrl");

                    b.Property<string>("MLSLink");

                    b.Property<int>("Price");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Zip");

                    b.HasKey("ListingId");

                    b.HasIndex("AddressId");

                    b.HasIndex("BrokerId");

                    b.ToTable("listings");
                });

            modelBuilder.Entity("Infinterest.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AffiliateCode");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserType");

                    b.HasKey("UserId");

                    b.ToTable("users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Infinterest.Models.VendorToEvent", b =>
                {
                    b.Property<int>("VendorId");

                    b.Property<int>("EventId");

                    b.Property<string>("RequestStatus");

                    b.HasKey("VendorId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("eventvendors");
                });

            modelBuilder.Entity("Infinterest.Models.Broker", b =>
                {
                    b.HasBaseType("Infinterest.Models.User");

                    b.Property<string>("Bio");

                    b.Property<string>("Company")
                        .IsRequired();

                    b.Property<string>("Contact")
                        .IsRequired();

                    b.Property<string>("ImgUrl")
                        .IsRequired();

                    b.Property<string>("Website");

                    b.ToTable("Broker");

                    b.HasDiscriminator().HasValue("Broker");
                });

            modelBuilder.Entity("Infinterest.Models.Vendor", b =>
                {
                    b.HasBaseType("Infinterest.Models.User");

                    b.Property<string>("Bio")
                        .HasColumnName("Vendor_Bio");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnName("Vendor_Company");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnName("Vendor_Contact");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnName("Vendor_ImgUrl");

                    b.Property<string>("Website")
                        .HasColumnName("Vendor_Website");

                    b.ToTable("Vendor");

                    b.HasDiscriminator().HasValue("Vendor");
                });

            modelBuilder.Entity("Infinterest.Models.Area", b =>
                {
                    b.HasOne("Infinterest.Models.Event")
                        .WithMany("AreaOfHouse")
                        .HasForeignKey("EventId");

                    b.HasOne("Infinterest.Models.Vendor")
                        .WithMany("AreaOfHouse")
                        .HasForeignKey("VendorUserId");
                });

            modelBuilder.Entity("Infinterest.Models.Business", b =>
                {
                    b.HasOne("Infinterest.Models.Vendor")
                        .WithMany("BusinessCategory")
                        .HasForeignKey("VendorUserId");
                });

            modelBuilder.Entity("Infinterest.Models.Event", b =>
                {
                    b.HasOne("Infinterest.Models.Broker", "Broker")
                        .WithMany("Events")
                        .HasForeignKey("BrokerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infinterest.Models.Listing", "Listing")
                        .WithMany("Events")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infinterest.Models.Listing", b =>
                {
                    b.HasOne("Infinterest.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infinterest.Models.Broker", "Broker")
                        .WithMany("Listings")
                        .HasForeignKey("BrokerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infinterest.Models.VendorToEvent", b =>
                {
                    b.HasOne("Infinterest.Models.Vendor", "Vendor")
                        .WithMany("Events")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infinterest.Models.Event", "Event")
                        .WithMany("EventVendors")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
