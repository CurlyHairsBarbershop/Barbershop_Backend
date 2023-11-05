﻿// <auto-generated />
using System;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231105170636_replies")]
    partial class replies
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppointmentFavor", b =>
                {
                    b.Property<int>("AppointmentId")
                        .HasColumnType("integer");

                    b.Property<int>("FavorsId")
                        .HasColumnType("integer");

                    b.HasKey("AppointmentId", "FavorsId");

                    b.HasIndex("FavorsId");

                    b.ToTable("AppointmentFavor");
                });

            modelBuilder.Entity("Core.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Core.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("At")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("BarberId")
                        .HasColumnType("integer");

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PlacedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2023, 11, 5, 17, 6, 36, 836, DateTimeKind.Utc).AddTicks(1290));

                    b.HasKey("Id");

                    b.HasIndex("BarberId");

                    b.HasIndex("ClientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Core.Favor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<double>("Cost")
                        .HasColumnType("double precision");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Favors");
                });

            modelBuilder.Entity("Core.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("BarberId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PublisherId")
                        .HasColumnType("integer");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int?>("ReplyTo")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BarberId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "Barber",
                            NormalizedName = "BARBER"
                        },
                        new
                        {
                            Id = -2,
                            Name = "Client",
                            NormalizedName = "CLIENT"
                        },
                        new
                        {
                            Id = -3,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Core.Admin", b =>
                {
                    b.HasBaseType("Core.ApplicationUser");

                    b.Property<string>("AdminAlias")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Core.Barber", b =>
                {
                    b.HasBaseType("Core.ApplicationUser");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.ToTable("Barbers");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "2d24168d-5711-41ed-9a21-72d45bc4a34f",
                            Email = "johnjj@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "John",
                            LastName = "Johnson",
                            LockoutEnabled = false,
                            PhoneNumber = "927804723",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            Id = -2,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "45c3ebdb-6f61-449b-a44f-a8ea7f2570d3",
                            Email = "alext@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Alex",
                            LastName = "Thompson",
                            LockoutEnabled = false,
                            PhoneNumber = "9478920606",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            Id = -3,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "b47852a1-ea9c-4272-b961-082439876168",
                            Email = "maxbobryk@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Maksym",
                            LastName = "Bobryk",
                            LockoutEnabled = false,
                            PhoneNumber = "48934909434",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            Id = -4,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "bc1ad83a-205e-4895-b13e-59774176c2ec",
                            Email = "20werasdf@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Mykhailo",
                            LastName = "Tkachenko",
                            LockoutEnabled = false,
                            PhoneNumber = "0970790944",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false
                        });
                });

            modelBuilder.Entity("Core.Client", b =>
                {
                    b.HasBaseType("Core.ApplicationUser");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("AppointmentFavor", b =>
                {
                    b.HasOne("Core.Appointment", null)
                        .WithMany()
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Favor", null)
                        .WithMany()
                        .HasForeignKey("FavorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Appointment", b =>
                {
                    b.HasOne("Core.Barber", "Barber")
                        .WithMany("Appointments")
                        .HasForeignKey("BarberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Client", "Client")
                        .WithMany("Appointments")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Barber");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Core.Review", b =>
                {
                    b.HasOne("Core.Barber", "Barber")
                        .WithMany("Reviews")
                        .HasForeignKey("BarberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Client", "Publisher")
                        .WithMany("Reviews")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Barber");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Core.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Core.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Core.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Admin", b =>
                {
                    b.HasOne("Core.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("Core.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Barber", b =>
                {
                    b.HasOne("Core.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("Core.Barber", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Client", b =>
                {
                    b.HasOne("Core.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("Core.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Barber", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Core.Client", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}