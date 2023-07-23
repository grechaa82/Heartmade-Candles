﻿// <auto-generated />
using System;
using HeartmadeCandles.Admin.DAL;
using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeartmadeCandles.Migrations.AdminDatabase
{
    [DbContext(typeof(AdminDbContext))]
    [Migration("20230721114635_UpdatedImages")]
    partial class UpdatedImages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdAt");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<ImageEntity[]>("Images")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("images");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isActive");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("title");

                    b.Property<int>("TypeCandleId")
                        .HasColumnType("integer")
                        .HasColumnName("typeCandleId");

                    b.Property<int>("WeightGrams")
                        .HasColumnType("integer")
                        .HasColumnName("weightGrams");

                    b.HasKey("Id");

                    b.HasIndex("TypeCandleId");

                    b.ToTable("Candle");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityDecorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CandleId")
                        .HasColumnType("integer")
                        .HasColumnName("candleId");

                    b.Property<int>("DecorId")
                        .HasColumnType("integer")
                        .HasColumnName("decorId");

                    b.HasKey("Id");

                    b.HasIndex("CandleId");

                    b.HasIndex("DecorId");

                    b.ToTable("CandleDecor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityLayerColorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CandleId")
                        .HasColumnType("integer")
                        .HasColumnName("candleId");

                    b.Property<int>("LayerColorId")
                        .HasColumnType("integer")
                        .HasColumnName("layerColorId");

                    b.HasKey("Id");

                    b.HasIndex("CandleId");

                    b.HasIndex("LayerColorId");

                    b.ToTable("CandleLayerColor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityNumberOfLayerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CandleId")
                        .HasColumnType("integer")
                        .HasColumnName("candleId");

                    b.Property<int>("NumberOfLayerId")
                        .HasColumnType("integer")
                        .HasColumnName("numberOfLayerId");

                    b.HasKey("Id");

                    b.HasIndex("CandleId");

                    b.HasIndex("NumberOfLayerId");

                    b.ToTable("CandleNumberOfLayer");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntitySmellEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CandleId")
                        .HasColumnType("integer")
                        .HasColumnName("candleId");

                    b.Property<int>("SmellId")
                        .HasColumnType("integer")
                        .HasColumnName("smellId");

                    b.HasKey("Id");

                    b.HasIndex("CandleId");

                    b.HasIndex("SmellId");

                    b.ToTable("CandleSmell");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityWickEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CandleId")
                        .HasColumnType("integer")
                        .HasColumnName("candleId");

                    b.Property<int>("WickId")
                        .HasColumnType("integer")
                        .HasColumnName("wickId");

                    b.HasKey("Id");

                    b.HasIndex("CandleId");

                    b.HasIndex("WickId");

                    b.ToTable("CandleWick");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.DecorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("imageURL");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isActive");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("Decor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.LayerColorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("imageURL");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isActive");

                    b.Property<decimal>("PricePerGram")
                        .HasColumnType("numeric")
                        .HasColumnName("pricePerGram");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("LayerColor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.NumberOfLayerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    b.HasKey("Id");

                    b.ToTable("NumberOfLayer");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.SmellEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isActive");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("Smell");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.TypeCandleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("TypeCandle");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.WickEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("imageURL");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isActive");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("Wick");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.TypeCandleEntity", "TypeCandle")
                        .WithMany("Candles")
                        .HasForeignKey("TypeCandleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TypeCandle");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityDecorEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", "Candle")
                        .WithMany("CandleDecor")
                        .HasForeignKey("CandleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.DecorEntity", "Decor")
                        .WithMany("CandleDecor")
                        .HasForeignKey("DecorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candle");

                    b.Navigation("Decor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityLayerColorEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", "Candle")
                        .WithMany("CandleLayerColor")
                        .HasForeignKey("CandleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.LayerColorEntity", "LayerColor")
                        .WithMany("CandleLayerColor")
                        .HasForeignKey("LayerColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candle");

                    b.Navigation("LayerColor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityNumberOfLayerEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", "Candle")
                        .WithMany("CandleNumberOfLayer")
                        .HasForeignKey("CandleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.NumberOfLayerEntity", "NumberOfLayer")
                        .WithMany("CandleNumberOfLayer")
                        .HasForeignKey("NumberOfLayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candle");

                    b.Navigation("NumberOfLayer");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntitySmellEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", "Candle")
                        .WithMany("CandleSmell")
                        .HasForeignKey("CandleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.SmellEntity", "Smell")
                        .WithMany("CandleSmell")
                        .HasForeignKey("SmellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candle");

                    b.Navigation("Smell");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntityWickEntity", b =>
                {
                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", "Candle")
                        .WithMany("CandleWick")
                        .HasForeignKey("CandleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeartmadeCandles.Admin.DAL.Entities.WickEntity", "Wick")
                        .WithMany("CandleWick")
                        .HasForeignKey("WickId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candle");

                    b.Navigation("Wick");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.CandleEntity", b =>
                {
                    b.Navigation("CandleDecor");

                    b.Navigation("CandleLayerColor");

                    b.Navigation("CandleNumberOfLayer");

                    b.Navigation("CandleSmell");

                    b.Navigation("CandleWick");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.DecorEntity", b =>
                {
                    b.Navigation("CandleDecor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.LayerColorEntity", b =>
                {
                    b.Navigation("CandleLayerColor");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.NumberOfLayerEntity", b =>
                {
                    b.Navigation("CandleNumberOfLayer");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.SmellEntity", b =>
                {
                    b.Navigation("CandleSmell");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.TypeCandleEntity", b =>
                {
                    b.Navigation("Candles");
                });

            modelBuilder.Entity("HeartmadeCandles.Admin.DAL.Entities.WickEntity", b =>
                {
                    b.Navigation("CandleWick");
                });
#pragma warning restore 612, 618
        }
    }
}
