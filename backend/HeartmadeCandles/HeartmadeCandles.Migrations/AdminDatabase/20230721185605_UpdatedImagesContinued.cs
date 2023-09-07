using HeartmadeCandles.Admin.DAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartmadeCandles.Migrations.AdminDatabase
{
    /// <inheritdoc />
    public partial class UpdatedImagesContinued : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageURL",
                table: "Wick");

            migrationBuilder.DropColumn(
                name: "imageURL",
                table: "LayerColor");

            migrationBuilder.DropColumn(
                name: "imageURL",
                table: "Decor");

            migrationBuilder.AddColumn<ImageEntity[]>(
                name: "images",
                table: "Wick",
                type: "jsonb",
                nullable: false,
                defaultValue: new ImageEntity[0]);

            migrationBuilder.AddColumn<ImageEntity[]>(
                name: "images",
                table: "LayerColor",
                type: "jsonb",
                nullable: false,
                defaultValue: new ImageEntity[0]);

            migrationBuilder.AddColumn<ImageEntity[]>(
                name: "images",
                table: "Decor",
                type: "jsonb",
                nullable: false,
                defaultValue: new ImageEntity[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "images",
                table: "Wick");

            migrationBuilder.DropColumn(
                name: "images",
                table: "LayerColor");

            migrationBuilder.DropColumn(
                name: "images",
                table: "Decor");

            migrationBuilder.AddColumn<string>(
                name: "imageURL",
                table: "Wick",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "imageURL",
                table: "LayerColor",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "imageURL",
                table: "Decor",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
