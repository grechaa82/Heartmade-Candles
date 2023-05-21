using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartmadeCandles.Migrations.AdminDatabase
{
    /// <inheritdoc />
    public partial class AddedThePriceFieldForTheCandleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Candle",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "Candle");
        }
    }
}
