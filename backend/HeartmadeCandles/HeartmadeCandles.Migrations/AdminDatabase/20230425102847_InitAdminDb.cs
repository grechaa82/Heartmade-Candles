#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HeartmadeCandles.Migrations.AdminDatabase;

/// <inheritdoc />
public partial class InitAdminDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Decor",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>("character varying(48)", maxLength: 48, nullable: false),
                description = table.Column<string>("character varying(256)", maxLength: 256, nullable: false),
                price = table.Column<decimal>("numeric", nullable: false),
                imageURL = table.Column<string>("text", nullable: false),
                isActive = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Decor", x => x.id); });

        migrationBuilder.CreateTable(
            "LayerColor",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>("character varying(48)", maxLength: 48, nullable: false),
                description = table.Column<string>("character varying(256)", maxLength: 256, nullable: false),
                pricePerGram = table.Column<decimal>("numeric", nullable: false),
                imageURL = table.Column<string>("text", nullable: false),
                isActive = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_LayerColor", x => x.id); });

        migrationBuilder.CreateTable(
            "NumberOfLayer",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                number = table.Column<int>("integer", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_NumberOfLayer", x => x.id); });

        migrationBuilder.CreateTable(
            "Smell",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>("character varying(48)", maxLength: 48, nullable: false),
                description = table.Column<string>("character varying(256)", maxLength: 256, nullable: false),
                price = table.Column<decimal>("numeric", nullable: false),
                isActive = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Smell", x => x.id); });

        migrationBuilder.CreateTable(
            "TypeCandle",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>("character varying(32)", maxLength: 32, nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_TypeCandle", x => x.id); });

        migrationBuilder.CreateTable(
            "Wick",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>("character varying(48)", maxLength: 48, nullable: false),
                description = table.Column<string>("character varying(256)", maxLength: 256, nullable: false),
                price = table.Column<decimal>("numeric", nullable: false),
                imageURL = table.Column<string>("text", nullable: false),
                isActive = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Wick", x => x.id); });

        migrationBuilder.CreateTable(
            "Candle",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>("character varying(48)", maxLength: 48, nullable: false),
                description = table.Column<string>("character varying(256)", maxLength: 256, nullable: false),
                weightGrams = table.Column<int>("integer", nullable: false),
                imageURL = table.Column<string>("text", nullable: false),
                isActive = table.Column<bool>("boolean", nullable: false),
                typeCandleId = table.Column<int>("integer", nullable: false),
                createdAt = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Candle", x => x.id);
                table.ForeignKey(
                    "FK_Candle_TypeCandle_typeCandleId",
                    x => x.typeCandleId,
                    "TypeCandle",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Candle_typeCandleId",
            "Candle",
            "typeCandleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Candle");

        migrationBuilder.DropTable(
            "Decor");

        migrationBuilder.DropTable(
            "LayerColor");

        migrationBuilder.DropTable(
            "NumberOfLayer");

        migrationBuilder.DropTable(
            "Smell");

        migrationBuilder.DropTable(
            "Wick");

        migrationBuilder.DropTable(
            "TypeCandle");
    }
}