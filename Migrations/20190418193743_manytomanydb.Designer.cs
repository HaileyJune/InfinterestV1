﻿// <auto-generated />
using System;
using Infinterest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infinterest.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20190418193743_manytomanydb")]
    partial class manytomanydb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Infinterest.Models.ConfirmedVendors", b =>
                {
                    b.Property<int>("VendorId");

                    b.Property<int>("EventId");

                    b.HasKey("VendorId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("confirmedVendors");
                });

            modelBuilder.Entity("Infinterest.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AreaOfHouseToFeature");

                    b.Property<int?>("BrokerUserId");

                    b.Property<bool>("Confirmed");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ListingId");

                    b.Property<DateTime>("OpenHouseDate");

                    b.Property<DateTime>("OpenHouseTime");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("EventId");

                    b.HasIndex("BrokerUserId");

                    b.HasIndex("ListingId");

                    b.ToTable("events");
                });

            modelBuilder.Entity("Infinterest.Models.Listing", b =>
                {
                    b.Property<int>("ListingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrokerId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImgUrl");

                    b.Property<string>("MLSLink");

                    b.Property<int>("Price");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Zip");

                    b.HasKey("ListingId");

                    b.HasIndex("BrokerId");

                    b.ToTable("listings");
                });

            modelBuilder.Entity("Infinterest.Models.PendingVendors", b =>
                {
                    b.Property<int>("VendorId");

                    b.Property<int>("EventId");

                    b.HasKey("VendorId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("pendingVendors");
                });

            modelBuilder.Entity("Infinterest.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AffiliateCode");

                    b.Property<string>("Bio");

                    b.Property<string>("Company");

                    b.Property<string>("Contact");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("ImgUrl");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserType");

                    b.Property<string>("Website");

                    b.HasKey("UserId");

                    b.ToTable("users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Infinterest.Models.Broker", b =>
                {
                    b.HasBaseType("Infinterest.Models.User");


                    b.ToTable("Broker");

                    b.HasDiscriminator().HasValue("Broker");
                });

            modelBuilder.Entity("Infinterest.Models.Vendor", b =>
                {
                    b.HasBaseType("Infinterest.Models.User");


                    b.ToTable("Vendor");

                    b.HasDiscriminator().HasValue("Vendor");
                });

            modelBuilder.Entity("Infinterest.Models.ConfirmedVendors", b =>
                {
                    b.HasOne("Infinterest.Models.Vendor", "Vendor")
                        .WithMany("ConfirmedEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infinterest.Models.Event", "Event")
                        .WithMany("ConfirmedVendors")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infinterest.Models.Event", b =>
                {
                    b.HasOne("Infinterest.Models.Broker")
                        .WithMany("Events")
                        .HasForeignKey("BrokerUserId");

                    b.HasOne("Infinterest.Models.Listing", "Listing")
                        .WithMany("Events")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infinterest.Models.Listing", b =>
                {
                    b.HasOne("Infinterest.Models.Broker", "Broker")
                        .WithMany("Listings")
                        .HasForeignKey("BrokerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infinterest.Models.PendingVendors", b =>
                {
                    b.HasOne("Infinterest.Models.Vendor", "Vendor")
                        .WithMany("RequestedEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infinterest.Models.Event", "Event")
                        .WithMany("RequestedVendors")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}