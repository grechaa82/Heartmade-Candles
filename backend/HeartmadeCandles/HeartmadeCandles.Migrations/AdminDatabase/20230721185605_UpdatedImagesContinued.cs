#nullable disable

using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeartmadeCandles.Migrations.AdminDatabase;

/// <inheritdoc />
public partial class UpdatedImagesContinued : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "imageURL",
            "Wick");

        migrationBuilder.DropColumn(
            "imageURL",
            "LayerColor");

        migrationBuilder.DropColumn(
            "imageURL",
            "Decor");

        migrationBuilder.AddColumn<ImageEntity[]>(
            "images",
            "Wick",
            "jsonb",
            nullable: false,
            defaultValue: new ImageEntity[0]);

        migrationBuilder.AddColumn<ImageEntity[]>(
            "images",
            "LayerColor",
            "jsonb",
            nullable: false,
            defaultValue: new ImageEntity[0]);

        migrationBuilder.AddColumn<ImageEntity[]>(
            "images",
            "Decor",
            "jsonb",
            nullable: false,
            defaultValue: new ImageEntity[0]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "images",
            "Wick");

        migrationBuilder.DropColumn(
            "images",
            "LayerColor");

        migrationBuilder.DropColumn(
            "images",
            "Decor");

        migrationBuilder.AddColumn<string>(
            "imageURL",
            "Wick",
            "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "imageURL",
            "LayerColor",
            "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "imageURL",
            "Decor",
            "text",
            nullable: false,
            defaultValue: "");
    }
}