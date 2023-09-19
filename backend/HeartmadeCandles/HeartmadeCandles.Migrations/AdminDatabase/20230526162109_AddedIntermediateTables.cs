#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HeartmadeCandles.Migrations.AdminDatabase;

/// <inheritdoc />
public partial class AddedIntermediateTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "CandleDecor",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                candleId = table.Column<int>("integer", nullable: false),
                decorId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandleDecor", x => x.id);
                table.ForeignKey(
                    "FK_CandleDecor_Candle_candleId",
                    x => x.candleId,
                    "Candle",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_CandleDecor_Decor_decorId",
                    x => x.decorId,
                    "Decor",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "CandleLayerColor",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                candleId = table.Column<int>("integer", nullable: false),
                layerColorId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandleLayerColor", x => x.id);
                table.ForeignKey(
                    "FK_CandleLayerColor_Candle_candleId",
                    x => x.candleId,
                    "Candle",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_CandleLayerColor_LayerColor_layerColorId",
                    x => x.layerColorId,
                    "LayerColor",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "CandleNumberOfLayer",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                candleId = table.Column<int>("integer", nullable: false),
                numberOfLayerId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandleNumberOfLayer", x => x.id);
                table.ForeignKey(
                    "FK_CandleNumberOfLayer_Candle_candleId",
                    x => x.candleId,
                    "Candle",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_CandleNumberOfLayer_NumberOfLayer_numberOfLayerId",
                    x => x.numberOfLayerId,
                    "NumberOfLayer",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "CandleSmell",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                candleId = table.Column<int>("integer", nullable: false),
                smellId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandleSmell", x => x.id);
                table.ForeignKey(
                    "FK_CandleSmell_Candle_candleId",
                    x => x.candleId,
                    "Candle",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_CandleSmell_Smell_smellId",
                    x => x.smellId,
                    "Smell",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "CandleWick",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation(
                        "Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                candleId = table.Column<int>("integer", nullable: false),
                wickId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandleWick", x => x.id);
                table.ForeignKey(
                    "FK_CandleWick_Candle_candleId",
                    x => x.candleId,
                    "Candle",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_CandleWick_Wick_wickId",
                    x => x.wickId,
                    "Wick",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_CandleDecor_candleId",
            "CandleDecor",
            "candleId");

        migrationBuilder.CreateIndex(
            "IX_CandleDecor_decorId",
            "CandleDecor",
            "decorId");

        migrationBuilder.CreateIndex(
            "IX_CandleLayerColor_candleId",
            "CandleLayerColor",
            "candleId");

        migrationBuilder.CreateIndex(
            "IX_CandleLayerColor_layerColorId",
            "CandleLayerColor",
            "layerColorId");

        migrationBuilder.CreateIndex(
            "IX_CandleNumberOfLayer_candleId",
            "CandleNumberOfLayer",
            "candleId");

        migrationBuilder.CreateIndex(
            "IX_CandleNumberOfLayer_numberOfLayerId",
            "CandleNumberOfLayer",
            "numberOfLayerId");

        migrationBuilder.CreateIndex(
            "IX_CandleSmell_candleId",
            "CandleSmell",
            "candleId");

        migrationBuilder.CreateIndex(
            "IX_CandleSmell_smellId",
            "CandleSmell",
            "smellId");

        migrationBuilder.CreateIndex(
            "IX_CandleWick_candleId",
            "CandleWick",
            "candleId");

        migrationBuilder.CreateIndex(
            "IX_CandleWick_wickId",
            "CandleWick",
            "wickId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "CandleDecor");

        migrationBuilder.DropTable(
            "CandleLayerColor");

        migrationBuilder.DropTable(
            "CandleNumberOfLayer");

        migrationBuilder.DropTable(
            "CandleSmell");

        migrationBuilder.DropTable(
            "CandleWick");
    }
}