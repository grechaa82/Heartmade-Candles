#nullable disable

using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeartmadeCandles.Migrations.AdminDatabase;

/// <inheritdoc />
public partial class UpdatedImages : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "imageURL",
            "Candle");

        migrationBuilder.AddColumn<ImageEntity[]>(
            "images",
            "Candle",
            "jsonb",
            nullable: false,
            defaultValue: new ImageEntity[0]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "images",
            "Candle");

        migrationBuilder.AddColumn<string>(
            "imageURL",
            "Candle",
            "text",
            nullable: false,
            defaultValue: "");
    }
}