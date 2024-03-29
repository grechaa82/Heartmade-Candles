﻿// <auto-generated />
using System;
using HeartmadeCandles.UserAndAuth.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeartmadeCandles.Migrations.UserAndAuthDatabase
{
    [DbContext(typeof(UserAndAuthDbContext))]
    partial class UserAndAuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HeartmadeCandles.UserAndAuth.DAL.Entities.SessionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expireAt");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refreshToken");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("HeartmadeCandles.UserAndAuth.DAL.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passwordHash");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registrationDate");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("userName");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HeartmadeCandles.UserAndAuth.DAL.Entities.SessionEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.UserAndAuth.DAL.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
