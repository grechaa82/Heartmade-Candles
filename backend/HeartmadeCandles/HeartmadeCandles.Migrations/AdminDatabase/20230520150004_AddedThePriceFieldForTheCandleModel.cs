#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HeartmadeCandles.Migrations.AdminDatabase;

/// <inheritdoc />
public partial class AddedThePriceFieldForTheCandleModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            "price",
            "Candle",
            "numeric",
            nullable: false,
            defaultValue: 0m);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "price",
            "Candle");
    }
}