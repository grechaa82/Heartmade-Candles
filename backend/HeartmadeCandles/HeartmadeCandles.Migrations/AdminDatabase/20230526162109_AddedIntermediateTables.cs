using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeartmadeCandles.Migrations.AdminDatabase
{
    /// <inheritdoc />
    public partial class AddedIntermediateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandleDecor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    candleId = table.Column<int>(type: "integer", nullable: false),
                    decorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandleDecor", x => x.id);
                    table.ForeignKey(
                        name: "FK_CandleDecor_Candle_candleId",
                        column: x => x.candleId,
                        principalTable: "Candle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandleDecor_Decor_decorId",
                        column: x => x.decorId,
                        principalTable: "Decor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandleLayerColor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    candleId = table.Column<int>(type: "integer", nullable: false),
                    layerColorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandleLayerColor", x => x.id);
                    table.ForeignKey(
                        name: "FK_CandleLayerColor_Candle_candleId",
                        column: x => x.candleId,
                        principalTable: "Candle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandleLayerColor_LayerColor_layerColorId",
                        column: x => x.layerColorId,
                        principalTable: "LayerColor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandleNumberOfLayer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    candleId = table.Column<int>(type: "integer", nullable: false),
                    numberOfLayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandleNumberOfLayer", x => x.id);
                    table.ForeignKey(
                        name: "FK_CandleNumberOfLayer_Candle_candleId",
                        column: x => x.candleId,
                        principalTable: "Candle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandleNumberOfLayer_NumberOfLayer_numberOfLayerId",
                        column: x => x.numberOfLayerId,
                        principalTable: "NumberOfLayer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandleSmell",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    candleId = table.Column<int>(type: "integer", nullable: false),
                    smellId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandleSmell", x => x.id);
                    table.ForeignKey(
                        name: "FK_CandleSmell_Candle_candleId",
                        column: x => x.candleId,
                        principalTable: "Candle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandleSmell_Smell_smellId",
                        column: x => x.smellId,
                        principalTable: "Smell",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandleWick",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    candleId = table.Column<int>(type: "integer", nullable: false),
                    wickId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandleWick", x => x.id);
                    table.ForeignKey(
                        name: "FK_CandleWick_Candle_candleId",
                        column: x => x.candleId,
                        principalTable: "Candle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandleWick_Wick_wickId",
                        column: x => x.wickId,
                        principalTable: "Wick",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandleDecor_candleId",
                table: "CandleDecor",
                column: "candleId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleDecor_decorId",
                table: "CandleDecor",
                column: "decorId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleLayerColor_candleId",
                table: "CandleLayerColor",
                column: "candleId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleLayerColor_layerColorId",
                table: "CandleLayerColor",
                column: "layerColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleNumberOfLayer_candleId",
                table: "CandleNumberOfLayer",
                column: "candleId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleNumberOfLayer_numberOfLayerId",
                table: "CandleNumberOfLayer",
                column: "numberOfLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleSmell_candleId",
                table: "CandleSmell",
                column: "candleId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleSmell_smellId",
                table: "CandleSmell",
                column: "smellId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleWick_candleId",
                table: "CandleWick",
                column: "candleId");

            migrationBuilder.CreateIndex(
                name: "IX_CandleWick_wickId",
                table: "CandleWick",
                column: "wickId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandleDecor");

            migrationBuilder.DropTable(
                name: "CandleLayerColor");

            migrationBuilder.DropTable(
                name: "CandleNumberOfLayer");

            migrationBuilder.DropTable(
                name: "CandleSmell");

            migrationBuilder.DropTable(
                name: "CandleWick");
        }
    }
}
