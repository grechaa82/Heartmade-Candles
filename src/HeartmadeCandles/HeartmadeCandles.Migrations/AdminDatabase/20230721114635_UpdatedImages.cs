using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartmadeCandles.Migrations.AdminDatabase
{
    /// <inheritdoc />
    public partial class UpdatedImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageURL",
                table: "Candle");

            migrationBuilder.AddColumn<ImageEntity[]>(
                name: "images",
                table: "Candle",
                type: "jsonb",
                nullable: false,
                defaultValue: new ImageEntity[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "images",
                table: "Candle");

            migrationBuilder.AddColumn<string>(
                name: "imageURL",
                table: "Candle",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
