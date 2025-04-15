﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShieldJWT.Context;

#nullable disable

namespace ShieldJWT.Migrations
{
    [DbContext(typeof(ShieldDbContext))]
    partial class ShieldDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShieldJWTLib.Models.ChangedPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConfimationCode")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("IdCompany")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NewHash")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("IdCompany");

                    b.ToTable("changed_password");
                });

            modelBuilder.Entity("ShieldJWTLib.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CNPJ")
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("company");
                });

            modelBuilder.Entity("ShieldJWTLib.Models.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Endpoint")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("IdCompany")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("InputTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Method")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime?>("OutputTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReturnType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdCompany");

                    b.ToTable("log");
                });

            modelBuilder.Entity("ShieldJWTLib.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Hash")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("IdCompany")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("IdCompany");

                    b.ToTable("user");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "noreply.shieldjwt@gmail.com",
                            EmailConfirmed = true,
                            Hash = "",
                            IdCompany = new Guid("00000000-0000-0000-0000-000000000000"),
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("ShieldJWTLib.Models.ChangedPassword", b =>
                {
                    b.HasOne("ShieldJWTLib.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("ShieldJWTLib.Models.Log", b =>
                {
                    b.HasOne("ShieldJWTLib.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("ShieldJWTLib.Models.User", b =>
                {
                    b.HasOne("ShieldJWTLib.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });
#pragma warning restore 612, 618
        }
    }
}
