﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectASP.Models;

namespace ProjectASP.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20221114084141_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProjectASP.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("Birthday");

                    b.Property<DateTime>("DayCreated");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<bool>("Role");

                    b.HasKey("AccountID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("account");
                });

            modelBuilder.Entity("ProjectASP.Models.Cart", b =>
                {
                    b.Property<int>("CartID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountID");

                    b.Property<int>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("CartID");

                    b.HasIndex("AccountID");

                    b.HasIndex("ProductID");

                    b.ToTable("cart");
                });

            modelBuilder.Entity("ProjectASP.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.HasKey("CategoryID");

                    b.HasIndex("CategoryName")
                        .IsUnique()
                        .HasFilter("[CategoryName] IS NOT NULL");

                    b.ToTable("category");
                });

            modelBuilder.Entity("ProjectASP.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountID");

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int>("ProductID");

                    b.Property<int>("Quantity");

                    b.Property<int>("Status");

                    b.Property<int>("TypeOrder");

                    b.HasKey("OrderID");

                    b.HasIndex("AccountID");

                    b.HasIndex("ProductID");

                    b.ToTable("order_detail");
                });

            modelBuilder.Entity("ProjectASP.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryID");

                    b.Property<DateTime>("DayMaking");

                    b.Property<string>("Description");

                    b.Property<string>("MadeIn")
                        .IsRequired();

                    b.Property<float>("Price");

                    b.Property<string>("ProductImage");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<float>("SalePrice");

                    b.Property<bool>("Status");

                    b.HasKey("ProductID");

                    b.HasIndex("CategoryID");

                    b.ToTable("product");
                });

            modelBuilder.Entity("ProjectASP.Models.Cart", b =>
                {
                    b.HasOne("ProjectASP.Models.Account", "Account")
                        .WithMany("Carts")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectASP.Models.Product", "Product")
                        .WithMany("Carts")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectASP.Models.OrderDetail", b =>
                {
                    b.HasOne("ProjectASP.Models.Account", "Account")
                        .WithMany("OrderDetails")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectASP.Models.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectASP.Models.Product", b =>
                {
                    b.HasOne("ProjectASP.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
